using System.Collections.Generic;
using System.Linq;

using Android.Content;
using Android.OS;
using Android.Support.V4.Content;
using Android.Text;
using Android.Views;
using Android.Views.InputMethods;
using Android.Widget;

using IRMGARD.Models;
using IRMGARD.Shared;

namespace IRMGARD
{
    public class InputConceptFragment : BaseConceptLayoutFragment<InputConcept>
    {
        bool userInteracted;

        public override View OnCreateView(LayoutInflater inflater, ViewGroup container, Bundle savedInstanceState)
        {
            View view = inflater.Inflate(Resource.Layout.InputConcept, container, false);
            InitBaseLayoutView(view);

            return view;
        }

        protected override void InitIteration()
        {
            base.InitIteration();

            var currentIteration = GetCurrentIteration<InputConceptIteration>();

            if (currentIteration.Tasks != null)
            {
                // Random select an exercise from this iteration
                exercise = currentIteration.Tasks.PickRandomItems(1).FirstOrDefault();
            }

            // Transform stored task item configuration
            TransformTaskItems();

            // Build task items
            BuildTaskItems();

            userInteracted = false;
        }

        protected override View CreateAndInitConceptView(Concept concept)
        {
            var conceptView = CreateConceptView(concept);
            var container = CreateConceptContainer(conceptView);
            container.SetPadding(ToPx(3), ToPx(3), ToPx(3), ToPx(3));

            // Add handlers
            if (concept is InputText)
            {
                (conceptView as EditText).TextChanged += ConceptView_TextChanged;
                (conceptView as EditText).EditorAction += ConceptView_EditorAction;
            }

            return container;
        }

        void ConceptView_TextChanged(object sender, TextChangedEventArgs e)
        {
            if (!userInteracted)
            {
                userInteracted = true;
                FireUserInteracted(true);
            }
        }

        void ConceptView_EditorAction(object sender, TextView.EditorActionEventArgs e)
        {
            var view = sender as View;

            if (e.ActionId == ImeAction.Done)
            {
                InputMethodManager inputMethodManager = Activity.GetSystemService(Context.InputMethodService) as InputMethodManager;
                inputMethodManager.HideSoftInputFromWindow(view.WindowToken, HideSoftInputFlags.None);
                FireCheckSolutionRequested();
            }
        }

        bool CheckAllTaskItems()
        {
            var correct = true;

            for (int i = 0; i < llTaskItemRows.ChildCount; i++)
            {
                var view = (ViewGroup)llTaskItemRows.GetChildAt(i);
                var llTaskItemRow = view.FindViewById<LinearLayout>(Resource.Id.llTaskItemRow);
                for (int k = 0; k < llTaskItemRow.ChildCount; k++)
                {
                    var conceptContainer = llTaskItemRow.GetChildAt(k) as ViewGroup;
                    var conceptView = conceptContainer.GetChildAt(0);
                    if (conceptView != null)
                    {
                        var concept = GetTag<Concept>(conceptView, Resource.Id.concept_tag_key);
                        if (concept.IsSolution)
                        {
                            var input = conceptView as EditText;
                            input.Enabled = false;
                            if (input.Text == null || input.Text.Length == 0 || !input.Text.Equals((concept as InputText).Text))
                            {
                                HighlightWrongLetters(input, concept as InputText);

                                MoveConceptContainer(conceptContainer);
                                conceptContainer.SetBackgroundResource(Resource.Drawable.rectangle_red);

                                correct = false;
                            }
                            else
                            {
                                MoveConceptContainer(conceptContainer);
                                conceptContainer.SetBackgroundResource(Resource.Drawable.rectangle_green);
                            }
                        }
                    }
                }
            }

            return correct;
        }

        private void HighlightWrongLetters(EditText input, InputText solution)
        {
            var hlText = new InputText();
            hlText.Highlights = new List<List<int>>();
            hlText.Text = input.Text;
            for (int m = 0; m < input.Text.Length; m++)
            {
                if (solution.Text.Length > m)
                {
                    if (input.Text[m] != solution.Text[m])
                    {
                        hlText.Highlights.Add(new List<int>() { m, m + 1 });
                    }
                }
                else
                {
                    hlText.Highlights.Add(new List<int>() { m, hlText.Text.Length });
                }
            }

            input.Text = "";
            DecorateText(input, hlText, new Android.Graphics.Color(
                ContextCompat.GetColor(Activity.BaseContext, Resource.Color.irmgard_red)),
                TextDecorationType.Foreground);
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

