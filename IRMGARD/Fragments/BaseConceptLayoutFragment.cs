using System.Collections.Generic;
using System.Linq;

using Android.Views;
using Android.Widget;

using IRMGARD.Models;

namespace IRMGARD
{
    public abstract class BaseConceptLayoutFragment<T> : BaseConceptFragment<T> where T : BaseConcept
    {
        protected LinearLayout llTaskItemRows;
        protected LinearLayout llSolutionItems;

        protected BaseConceptExercise exercise;
        protected bool inReview;

        protected virtual void InitBaseLayoutView(View layoutView)
        {
            llTaskItemRows = layoutView.FindViewById<LinearLayout>(Resource.Id.llTaskItemRows);
            llSolutionItems = layoutView.FindViewById<LinearLayout>(Resource.Id.llSolutionItems);

            InitIteration();
        }

        protected abstract View CreateAndInitConceptView(Concept concept);

        protected void BuildTaskItems()
        {
            // Replace default top margin
            SetTopMargin(0, (View)llTaskItemRows.Parent);

            // Add task items to view
            llTaskItemRows.RemoveAllViews();
            foreach (var taskItemRow in exercise.TaskItems)
            {
                var llTaskItemRowRoot = LayoutInflater.From(Activity.BaseContext).Inflate(Resource.Layout.TaskItemRow, null);
                var llTaskItemRow = llTaskItemRowRoot.FindViewById<LinearLayout>(Resource.Id.llTaskItemRow);
                foreach (var item in taskItemRow)
                {
                    // Exclude special case concepts
                    if (item.ActivateOnSuccess || item.ActivateOnMistake) { continue; }

                    // Invoke callback
                    var view = CreateAndInitConceptView(item);

                    // Add container to task items
                    llTaskItemRow.AddView(view);
                }

                // Replace top margin of task item rows
                SetTopMargin(1, llTaskItemRow);

                if (Lesson.HideRack)
                {
                    llTaskItemRowRoot.FindViewById<ImageView>(Resource.Id.ivDivider).Visibility = ViewStates.Gone;
                }

                llTaskItemRows.AddView(llTaskItemRowRoot);
            }
        }

        protected bool BuildSolutionItems(List<Concept> solutionItems)
        {
            bool itemsCreated = false;

            // Replace top margin of solution items
            SetTopMargin(3, llSolutionItems);

            // Add solution items to view
            llSolutionItems.RemoveAllViews();
            foreach (var item in solutionItems)
            {
                // Init concept view
                var view = CreateConceptView(item);

                // Play sound
                if (item is ISound)
                {
                    if (!string.IsNullOrEmpty((item as ISound).SoundPath))
                    {
                        SoundPlayer.PlaySound(Activity.BaseContext, (item as ISound).SoundPath);
                    }
                }

                // Add view to solution items
                llSolutionItems.AddView(view);
                itemsCreated = true;
            }

            return itemsCreated;
        }

        protected List<Concept> GetConceptsToActivateOnCheckSolution(bool success)
        {
            var concepts = new List<Concept>();
            foreach (var taskItemRow in exercise.TaskItems)
            {
                concepts.AddRange(taskItemRow.Where(ti => (success) ? ti.ActivateOnSuccess : ti.ActivateOnMistake));
            }

            return concepts;
        }

        protected override int CountPictureItems()
        {
            int pictureItemCounter = 0;

            foreach (var conceptItemRow in exercise.TaskItems)
            {
                foreach (var item in conceptItemRow)
                {
                    if (item is Picture)
                    {
                        pictureItemCounter++;
                    }
                }
            }

            return pictureItemCounter;
        }

        protected void SetTopMargin(int idx, View view)
        {
            int topMarginToSet = (Lesson.TopMargins != null && Lesson.TopMargins.Length >= idx + 1) ? Lesson.TopMargins[idx] : -1;

            if (view.LayoutParameters is FrameLayout.LayoutParams)
            {
                var lp = (view.LayoutParameters as FrameLayout.LayoutParams);
                var topMargin = (topMarginToSet > -1) ? topMarginToSet : lp.TopMargin;
                lp.TopMargin = IsSmallHeight() ? topMargin / 2 : topMargin;
            }
            else
            {
                var lp = (view.LayoutParameters as LinearLayout.LayoutParams);
                var topMargin = (topMarginToSet > -1) ? topMarginToSet : lp.TopMargin;
                lp.TopMargin = IsSmallHeight() ? topMargin / 2 : topMargin;
            }
        }
    }
}

