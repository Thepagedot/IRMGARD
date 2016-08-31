using System;
using System.Linq;

using Android.Graphics.Drawables;
using Android.OS;
using Android.Views;
using Android.Widget;

using IRMGARD.Models;
using IRMGARD.Shared;

namespace IRMGARD
{
    public class SelectConceptFragment : BaseConceptLayoutFragment<SelectConcept>
    {
        public override View OnCreateView(LayoutInflater inflater, ViewGroup container, Bundle savedInstanceState)
        {
            View view = inflater.Inflate(Resource.Layout.SelectConcept, container, false);
            InitBaseLayoutView(view);

            return view;
        }

        protected override void InitIteration()
        {
            base.InitIteration();

            var currentIteration = GetCurrentIteration<SelectConceptIteration>();

            // Random select an exercise from this iteration
            exercise = currentIteration.Tasks.PickRandomItems(1).FirstOrDefault();

            // Transform stored task item configuration
            TransformTaskItems();

            // Build task items
            BuildTaskItems();
        }

        protected virtual void TransformTaskItems() { }

        ViewGroup CreateContentContainer(View child)
        {
            var container = (ViewGroup)LayoutInflater.From(Activity.BaseContext).Inflate(Resource.Layout.ContentContainer, null);
            LinearLayout.LayoutParams llLP = new LinearLayout.LayoutParams(ViewGroup.LayoutParams.WrapContent, ViewGroup.LayoutParams.WrapContent);
            llLP.SetMargins(ToPx(2), 0, ToPx(2), 0);
            container.LayoutParameters = llLP;
            container.AddView(child);

            return container;
        }

        protected override View CreateAndInitConceptView(Concept concept)
        {
            var container = CreateContentContainer(CreateConceptView(concept));
            AddSelectHandler(container.GetChildAt(0), concept);

            return container;
        }

        void AddSelectHandler(View view, Concept concept)
        {
            if (concept.IsOption || concept.IsSolution)
            {
                if (concept is Speaker)
                {
                    view.Click -= ConceptView_Click_PlaySound;
                    view.Click += ConceptView_Click_PlaySound_Click_SelectItem;
                }
                else
                {
                    view.Click += ConceptView_Click_SelectItem;
                }
            }
        }

        void RemoveSelectHandler(View view, Concept concept)
        {
            if (concept is Speaker)
            {
                view.Click -= ConceptView_Click_PlaySound_Click_SelectItem;
                view.Click += ConceptView_Click_PlaySound;
            }
            else
            {
                view.Click -= ConceptView_Click_SelectItem;
            }
        }

        void ConceptView_Click_SelectItem(object sender, EventArgs e)
        {
            var view = sender as View;
            var concept = GetTag<Concept>(view, Resource.Id.concept_tag_key);

            var drawable = GetTag<Drawable>(view, Resource.Id.selected_tag_key);
            if (drawable == null)
            {
                SetTag(view, Resource.Id.selected_tag_key, view.Background != null
                    ? view.Background
                    : new ColorDrawable(Android.Graphics.Color.Transparent));
                view.SetBackgroundResource(Resource.Drawable.selected_background);
            }
            else
            {
                if (Env.LollipopSupport)
                {
                    view.Background = drawable;
                }
                else
                {
                    var container = view.Parent as ViewGroup;
                    var layout = container.Parent as ViewGroup;
                    var idx = layout.IndexOfChild(container);
                    layout.RemoveViewAt(idx);
                    layout.AddView(CreateAndInitConceptView(concept), idx);
                }
                SetTag<Drawable>(view, Resource.Id.selected_tag_key, null);
            }

            FireUserInteracted(CheckUserInteracted());
        }

        void ConceptView_Click_PlaySound_Click_SelectItem(object sender, EventArgs e)
        {
            var view = sender as View;
            var concept = GetTag<Concept>(view, Resource.Id.concept_tag_key);

            if (GetTag<bool>(view, Resource.Id.clicked_once_tag_key))
            {
                SetTag(view, Resource.Id.clicked_once_tag_key, false);
                ConceptView_Click_SelectItem(sender, e);
            }
            else
            {
                SetTag(view, Resource.Id.clicked_once_tag_key, true);
                ConceptView_Click_PlaySound(sender, e);
            }
        }

        protected virtual bool CheckUserInteracted()
        {
            // Check if the user is done
            for (int i = 0; i < llTaskItemRows.ChildCount; i++)
            {
                var view = (ViewGroup)llTaskItemRows.GetChildAt(i);
                var llTaskItemRow = view.FindViewById<LinearLayout>(Resource.Id.llTaskItemRow);
                for (int k = 0; k < llTaskItemRow.ChildCount; k++)
                {
                    var containerView = llTaskItemRow.GetChildAt(k) as ViewGroup;
                    var contentView = containerView.GetChildAt(0);
                    if (GetTag<Drawable>(contentView, Resource.Id.selected_tag_key) != null)
                    {
                        return true;
                    }
                }
            }

            return false;
        }

        bool CheckAllTaskItems()
        {
            var correct = true;

            for (int i = 0; i < llTaskItemRows.ChildCount; i++)
            {
                var view = (ViewGroup)llTaskItemRows.GetChildAt(i);
                var llTaskItemRow = view.FindViewById<LinearLayout>(Resource.Id.llTaskItemRow);

                if (!Lesson.HideRack)
                {
                    // Move llTaskItemRow to display success/failure bottom border of a concept view
                    var layoutParams = (LinearLayout.LayoutParams)llTaskItemRow.LayoutParameters;
                    layoutParams.BottomMargin = ToPx(2);
                }

                for (int k = 0; k < llTaskItemRow.ChildCount; k++)
                {
                    var containerView = llTaskItemRow.GetChildAt(k) as ViewGroup;
                    var contentView = containerView.GetChildAt(0);
                    if (contentView != null)
                    {
                        var concept = GetTag<Concept>(contentView, Resource.Id.concept_tag_key);
                        if (concept.IsOption)
                        {
                            RemoveSelectHandler(contentView, concept);
                            if (GetTag<Drawable>(contentView, Resource.Id.selected_tag_key) != null)
                            {
                                containerView.SetBackgroundResource(Resource.Drawable.rectangle_red);
                                correct = false;
                            }
                        }
                        else if (concept.IsSolution)
                        {
                            RemoveSelectHandler(contentView, concept);
                            if (GetTag<Drawable>(contentView, Resource.Id.selected_tag_key) == null)
                            {
                                correct = false;
                            }
                            containerView.SetBackgroundResource(Resource.Drawable.rectangle_green);
                        }
                    }
                }
            }

            return correct;
        }

        public override void CheckSolution()
        {
            if (inReview)
            {
                // User clicked the next button for the second time
                inReview = false;
                llSolutionItems.Visibility = ViewStates.Gone;

                FinishIteration(CheckAllTaskItems());
            }
            else
            {
                // First time solution check
                if (CheckAllTaskItems())
                {
                    // Build all concepts to activate on success
                    if (BuildSolutionItems(GetConceptsToActivateOnCheckSolution(true)))
                    {
                        llSolutionItems.Visibility = ViewStates.Visible;
                        inReview = true;
                        FireUserInteracted(true);
                    }
                    else
                    {
                        FinishIteration(true);
                    }
                }
                else
                {
                    // Build all concepts to activate on mistake
                    if (BuildSolutionItems(GetConceptsToActivateOnCheckSolution(false)))
                    {
                        llSolutionItems.Visibility = ViewStates.Visible;
                    }

                    inReview = true;
                    FireUserInteracted(true);
                }
            }
        }
    }
}

