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

            OnCreateViewConfig();

            InitIteration();
        }

        protected virtual void OnCreateViewConfig() { }

        protected abstract View CreateAndInitConceptView(Concept concept);

        protected virtual void TransformTaskItems() { }

        protected virtual void BuildTaskItems()
        {
            // Replace default top margin
            SetTopMargin(0, (View)llTaskItemRows.Parent);

            // Add task items to view
            llTaskItemRows.RemoveAllViews();
            int i = 0;
            foreach (var taskItemRow in exercise.TaskItems)
            {
                var llTaskItemRowRoot = LayoutInflater.From(Activity.BaseContext).Inflate(Resource.Layout.TaskItemRow, null);
                var llTaskItemRow = llTaskItemRowRoot.FindViewById<LinearLayout>(Resource.Id.llTaskItemRow);

                // Align task row items to the left rather than centered
                if (Lesson.LeftAlignItems != null && Lesson.LeftAlignItems.Length > 0)
                {
                    if (Lesson.LeftAlignItems.Length > i && Lesson.LeftAlignItems[i])
                    {
                        var lp = (llTaskItemRow.LayoutParameters as LinearLayout.LayoutParams);
                        lp.Gravity = GravityFlags.Left;
                    }
                }

                int k = 0;
                bool isActivateOnlyRow = true;
                bool isSpaceOnlyRow = true;
                bool isFootnoteRow = true;
                foreach (var item in taskItemRow)
                {
                    // Exclude special case concepts
                    if (item.ActivateOnSuccess || item.ActivateOnMistake) {
                        continue;
                    } else {
                        isActivateOnlyRow = false;
                    }

                    // Exclude special case concepts
                    if (!(item is IRMGARD.Models.Space)) {
                        isSpaceOnlyRow = false;
                    }

                    if (!(item is IRMGARD.Models.Footnote)) {
                        isFootnoteRow = false;
                    }

                    // Invoke callback
                    var view = CreateAndInitConceptView(item);

                    // Apply two column layout rules
                    if (Lesson.TwoColumns > 0 && k % 2 == 0)
                    {
                        var lp = (view.LayoutParameters as LinearLayout.LayoutParams);
                        lp.Width = ToPx(Lesson.TwoColumns);
                    }

                    // Add container to task items
                    llTaskItemRow.AddView(view);
                    k++;
                }

                // Replace top margin of task item rows
                SetTopMargin(1, llTaskItemRow);

                if (Lesson.HideRack || isActivateOnlyRow || isSpaceOnlyRow || isFootnoteRow)
                {
                    llTaskItemRowRoot.FindViewById<ImageView>(Resource.Id.ivDivider).Visibility = ViewStates.Gone;
                }

                llTaskItemRows.AddView(llTaskItemRowRoot);
                i++;
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
                var conceptView = CreateConceptView(item);
                var conceptContainer = CreateConceptContainer(conceptView);
                conceptContainer.SetBackgroundResource(Resource.Drawable.rectangle_green);

                // Play sound
                if (item is ISound)
                {
                    if (!string.IsNullOrEmpty((item as ISound).SoundPath))
                    {
                        SoundPlayer.PlaySound(Activity.BaseContext, (item as ISound).SoundPath);
                    }
                }

                // Add view to solution items
                llSolutionItems.AddView(conceptContainer);
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
            if (exercise == null || exercise.TaskItems == null)
            {
                return 1;
            }

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

        protected void MoveConceptContainer(ViewGroup conceptContainer)
        {
            if (!Lesson.HideRack)
            {
                // Move view up to fully display its success/failure decoration border
                var layoutParams = conceptContainer.LayoutParameters as LinearLayout.LayoutParams;
                layoutParams.BottomMargin = ToPx(2);  // Set spacing
            }
        }
    }
}

