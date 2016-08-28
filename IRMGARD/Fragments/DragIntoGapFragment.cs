using System;
using System.Collections.Generic;
using System.Linq;

using Android.Content;
using Android.OS;
using Android.Views;
using Android.Widget;

using IRMGARD.Models;
using IRMGARD.Shared;

namespace IRMGARD
{
    public class DragIntoGapFragment : BaseConceptFragment<DragIntoGap>
    {
        FlowLayout flOptionItems;
        List<Concept> optionItems;

        bool dragActionDropHandled;
        bool dragActionStartedHandled;
        bool dragActionEndedHandled;

        public override View OnCreateView(LayoutInflater inflater, ViewGroup container, Bundle savedInstanceState)
        {
            View view = inflater.Inflate(Resource.Layout.DragIntoGap, container, false);
            flOptionItems = view.FindViewById<FlowLayout>(Resource.Id.flOptionItems);

            InitBaseLayoutView(view);

            return view;
        }

        protected override void InitIteration()
        {
            base.InitIteration();

            var currentIteration = GetCurrentIteration<DragIntoGapIteration>();

            // Random select an exercise from this iteration
            exercise = currentIteration.Tasks.PickRandomItems(1).FirstOrDefault();

            // Build task items
            BuildTaskItems();

            var dragIntoGapExercise = exercise as DragIntoGapExercise;

            // Accumulate and build option item collection
            optionItems = new List<Concept>();

            // Add solution task items as options
            if (Lesson.UseOptionItemsOnly)
            {
                AddSolutionConcepts(dragIntoGapExercise);
            }
            else
            {
                foreach (var task in currentIteration.Tasks)
                {
                    AddSolutionConcepts(task as DragIntoGapExercise);
                }
            }

            // Add option items
            if (dragIntoGapExercise.OptionItems != null && dragIntoGapExercise.OptionItems.Count > 0)
            {
                optionItems.AddRange(dragIntoGapExercise.OptionItems);
            }
            if (Lesson.OptionItems != null && Lesson.OptionItems.Count > 0)
            {
                foreach (var item in Lesson.OptionItems)
                {
                    if (!optionItems.Contains(item)) {
                        optionItems.Add(item);
                    }
                }
            }
            BuildOptionItems();
        }

        void AddSolutionConcepts(DragIntoGapExercise task)
        {
            foreach (var taskItemRow in task.TaskItems)
            {
                optionItems.AddRange(taskItemRow.Where(ti => ti.IsSolution).Select(ti => {
                    var c = ti.DeepCopy();
                    c.IsSolution = false;
                    return c;
                }));
            }
        }

        protected override View CreateAndInitConceptView(Concept concept)
        {
            // Init container
            var container = CreateContentContainer(Resource.Id.flConceptContainer,
                (concept.IsSolution) ? CreateBlankView(concept) : CreateConceptView(concept));

            // Add drop handler
            if (concept.IsSolution)
            {
                container.Drag += View_Drag;
            }

            return container;
        }

        void BuildOptionItems()
        {
            // Replace top margin of option items
            SetTopMargin(2, flOptionItems);

            // Shuffle options
            optionItems.Shuffle();

            // Add options to view
            flOptionItems.RemoveAllViews();
            foreach (var item in optionItems)
            {
                // Flag item as an option
                item.IsOption = true;

                // Init container (the additional container is used only to define a padding for IRMGARD.FlowLayout elements)
                var container = CreateContentContainer(Resource.Id.flOptionContainer, CreateConceptView(item));

                // Add drag handler
                container.GetChildAt(0).Touch += ConceptView_Touch_StartDrag;

                // Add view to option items
                flOptionItems.AddView(container);
            }
        }

        protected override int CountPictureItems()
        {
            int pictureItemCounter = base.CountPictureItems();

            if (optionItems != null)
            {
                foreach (var item in optionItems)
                {
                    if (item is Picture)
                    {
                        pictureItemCounter++;
                    }
                }
            }

            return pictureItemCounter;
        }

        void ConceptView_Touch_StartDrag(object sender, EventArgs e)
        {
            var v = sender as View;
            v.StartDrag(ClipData.NewPlainText("", ""), new View.DragShadowBuilder(v), v, 0);
        }

        ViewGroup CreateContentContainer(int resId, View child)
        {
            var container = (ViewGroup)LayoutInflater.From(Activity.BaseContext).Inflate(Resource.Layout.ContentContainer, null);
            container.Id = resId;
            switch (resId)
            {
                case Resource.Id.flConceptContainer:
                    LinearLayout.LayoutParams llLP = new LinearLayout.LayoutParams(ViewGroup.LayoutParams.WrapContent, ViewGroup.LayoutParams.WrapContent);
                    llLP.SetMargins(4, 0, 4, 0);
                    container.LayoutParameters = llLP;
                    break;
                case Resource.Id.flOptionContainer:
                    FlowLayout.LayoutParams llFL = new FlowLayout.LayoutParams(ViewGroup.LayoutParams.WrapContent, ViewGroup.LayoutParams.WrapContent);
                    llFL.HorizontalSpacing = 8;
                    llFL.VerticalSpacing = 8;
                    container.LayoutParameters = llFL;
                    break;
            }
            container.AddView(child);

            return container;
        }

        int CalcSizeOfBlanks(List<List<Concept>> conceptItems)
        {
            int blankSize = 0;
            int conceptItemCounter = 0;

            foreach (var conceptItemRow in conceptItems)
            {
                foreach (var item in conceptItemRow)
                {
                    // Exclude special case concepts
                    if (item.ActivateOnSuccess || item.ActivateOnMistake) { continue; }

                    if (item is BaseText)
                    {
                        blankSize += (item as BaseText).Text.Length;
                        conceptItemCounter++;
                    }
                }
            }

            return blankSize / conceptItemCounter;
        }

        string GetBlankText(Concept concept)
        {
            if (concept is Letter) { return "M"; }
            else if (concept is Syllable) { return string.Concat(Enumerable.Repeat("M", CalcSizeOfBlanks(exercise.TaskItems))); }
            else { return string.Concat(Enumerable.Repeat("a", CalcSizeOfBlanks(exercise.TaskItems))); }
        }

        View CreateBlankView(Concept concept)
        {
            View view = CreateConceptView(concept);
            view.Click -= ConceptView_Click_PlaySound;
            view.Visibility = ViewStates.Invisible;

            var blankView = (FrameLayout)LayoutInflater.From(Activity.BaseContext).Inflate(Resource.Layout.Blank, null);
            blankView.AddView(view);

            return blankView;
        }

        protected override string GetTextCallback(BaseText concept)
        {
            return concept.IsSolution ? GetBlankText(concept) : concept.Text;
        }

        View GetCCContent(ViewGroup conceptContainer)
        {
            for (int i = 0; i < conceptContainer.ChildCount; i++)
            {
                var view = conceptContainer.GetChildAt(i);
                if (!view.Id.Equals(Resource.Id.flBlank))
                {
                    return view;
                }
            }

            return null;
        }

        bool CheckUserInteracted()
        {
            // Check if the user is done
            for (int i = 0; i < llTaskItemRows.ChildCount; i++)
            {
                var view = (ViewGroup)llTaskItemRows.GetChildAt(i);
                var llTaskItemRow = view.FindViewById<LinearLayout>(Resource.Id.llTaskItemRow);
                for (int k = 0; k < llTaskItemRow.ChildCount; k++)
                {
                    if (GetCCContent(llTaskItemRow.GetChildAt(k) as ViewGroup) == null)
                    {
                        return false;
                    }
                }
            }

            return true;
        }

        void View_Drag(object sender, View.DragEventArgs e)
        {
            var srcView = (View) e.Event.LocalState;
            if (srcView == null || srcView.Parent == null) return;

            var srcContainer = (srcView.Parent as ViewGroup);
            var destContainer = (FrameLayout) sender;

            switch (e.Event.Action)
            {
                case DragAction.Started:
                    e.Handled = true;

                    if (!dragActionStartedHandled)
                    {
                        dragActionDropHandled = false;
                        dragActionEndedHandled = false;

                        if (srcContainer != null && srcContainer.Id.Equals(Resource.Id.flConceptContainer))
                        {
                            srcView.Visibility = ViewStates.Gone;
                            srcContainer.FindViewById<View>(Resource.Id.flBlank).Visibility = ViewStates.Visible;
                        }
                        else if (srcContainer != null && srcContainer.Id.Equals(Resource.Id.flOptionContainer))
                        {
                            srcView.Visibility = ViewStates.Invisible;
                        }
                        dragActionStartedHandled = true;
                    }

                    break;
                case DragAction.Entered:
                    destContainer.SetBackgroundResource(Resource.Drawable.rectangle_gray);

                    break;
                case DragAction.Exited:
                    destContainer.SetBackgroundResource(0);

                    break;
                case DragAction.Ended:
                    e.Handled = true;
                    destContainer.SetBackgroundResource(0);

                    if (!dragActionEndedHandled)
                    {
                        dragActionStartedHandled = false;

                        if (!dragActionDropHandled)
                        {
                            if (srcContainer != null && srcContainer.Id.Equals(Resource.Id.flConceptContainer))
                            {
                                srcContainer.RemoveView(srcView);
                                flOptionItems.AddView(CreateContentContainer(Resource.Id.flOptionContainer, srcView));
                            }
                            srcView.Visibility = ViewStates.Visible;
                            FireUserInteracted(CheckUserInteracted());
                        }
                        dragActionDropHandled = false;
                        dragActionEndedHandled = true;
                    }

                    break;
                case DragAction.Drop:
                    e.Handled = true;
                    dragActionDropHandled = true;

                    var contentView = GetCCContent(destContainer);
                    if (contentView != null)
                    {
                        // Swap concept view items
                        destContainer.RemoveView(contentView);
                        srcContainer.AddView(contentView);
                    }
                    else
                    {
                        if (srcContainer != null && srcContainer.Id.Equals(Resource.Id.flConceptContainer))
                        {
                            // Hide blank view
                            destContainer.FindViewById<FrameLayout>(Resource.Id.flBlank).Visibility = ViewStates.Gone;
                        }
                        else if (srcContainer != null && srcContainer.Id.Equals(Resource.Id.flOptionContainer))
                        {
                            // Remove dragged view from options list
                            flOptionItems.RemoveView(srcContainer);
                        }
                    }

                    // Add dragged view to task items
                    srcContainer.RemoveView(srcView);
                    destContainer.AddView(srcView);
                    srcView.Visibility = ViewStates.Visible;

                    // Check if the user has completed its task
                    FireUserInteracted(CheckUserInteracted());

                    break;
                default:
                    break;
            }
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
                    var conceptContainer = llTaskItemRow.GetChildAt(k) as ViewGroup;
                    var blankView = conceptContainer.FindViewById<FrameLayout>(Resource.Id.flBlank);
                    if (blankView != null)
                    {
                        var contentView = GetCCContent(conceptContainer);
                        if (contentView != null)
                        {
                            // Remove drag handler
                            contentView.Touch -= ConceptView_Touch_StartDrag;

                            var contentViewConcept = GetTag<Concept>(contentView, Resource.Id.concept_tag_key);
                            var blankViewConcept = GetTag<Concept>(blankView.GetChildAt(1), Resource.Id.concept_tag_key);
                            if (contentViewConcept != null && blankViewConcept != null && contentViewConcept.Equals(blankViewConcept))
                            {
                                conceptContainer.SetBackgroundResource(Resource.Drawable.rectangle_green);
                            }
                            else
                            {
                                conceptContainer.SetBackgroundResource(Resource.Drawable.rectangle_red);
                                correct = false;
                            }
                        }
                    }
                }
            }

            return correct;
        }

        List<Concept> GetCorrectConcepts()
        {
            var concepts = new List<Concept>();
            foreach (var conceptItemRow in exercise.TaskItems)
            {
                concepts.AddRange(conceptItemRow.Where(c => c.IsSolution));
            }

            return concepts;
        }

        void HighlightCorrectOptions(List<Concept> correctConcepts)
        {
            for (int i = 0; i < flOptionItems.ChildCount; i++)
            {
                var conceptContainer = flOptionItems.GetChildAt(i) as ViewGroup;
                var optionView = conceptContainer.GetChildAt(0);
                if (optionView != null)
                {
                    // Remove drag handler
                    optionView.Touch -= ConceptView_Touch_StartDrag;

                    var optionViewConcept = GetTag<Concept>(optionView, Resource.Id.concept_tag_key);
                    foreach (var correctConcept in correctConcepts)
                    {
                        if (correctConcept != null && optionViewConcept != null && correctConcept.Equals(optionViewConcept))
                        {
                            conceptContainer.SetBackgroundResource(Resource.Drawable.rectangle_green);
                            break;
                        }
                    }
                }
            }
        }

        public override void CheckSolution()
        {
            if (inReview)
            {
                // User clicked the next button for the second time
                inReview = false;
                flOptionItems.Visibility = ViewStates.Visible;
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
                        flOptionItems.Visibility = ViewStates.Gone;
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

                    // Highlight correct options
                    if (Lesson.HighlightCorrectOptionsOnMistake)
                    {
                        HighlightCorrectOptions(GetCorrectConcepts());
                    }
                    else
                    {
                        flOptionItems.Visibility = ViewStates.Gone;
                    }

                    inReview = true;
                    FireUserInteracted(true);
                }
            }
        }
    }
}

