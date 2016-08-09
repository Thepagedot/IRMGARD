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
    public class DragIntoGapFragment : LessonFragment<DragIntoGap>
    {
        LinearLayout llTaskItemRows;
        FlowLayout flOptionItems;

        List<List<Concept>> currentTaskItems;

        bool dragActionDropHandled;
        bool dragActionStartedHandled;
        bool dragActionEndedHandled;

        public override View OnCreateView (LayoutInflater inflater, ViewGroup container, Bundle savedInstanceState)
        {
            View view = inflater.Inflate(Resource.Layout.DragIntoGap, container, false);
            llTaskItemRows = view.FindViewById<LinearLayout>(Resource.Id.llTaskItemRows);
            flOptionItems = view.FindViewById<FlowLayout>(Resource.Id.flOptionItems);

            InitIteration();

            return view;
        }

        protected override void InitIteration()
        {
            base.InitIteration();

            var currentIteration = GetCurrentIteration<DragIntoGapIteration>();

            // (Random) select task items to display in this iteration
            currentTaskItems = currentIteration.NumberOfTaskItemsToShow > 0
                ? (List<List<Concept>>)currentIteration.TaskItems.PickRandomItems(currentIteration.NumberOfTaskItemsToShow)
                : currentIteration.TaskItems;

            // Build task items
            BuildTaskItems(currentTaskItems);

            // Accumulate and build option items
            var accOptionItems = new List<Concept>();
            foreach (var taskItemRow in currentIteration.TaskItems)
            {
                accOptionItems.AddRange(taskItemRow.Where(ti => !ti.Fixed).Select(ti => ti.DeepCopy()));
            }
            if (currentIteration.OptionItems != null && currentIteration.OptionItems.Count > 0)
            {
                accOptionItems.AddRange(currentIteration.OptionItems);
            }
            BuildOptionItems(accOptionItems);
        }

        void BuildTaskItems(List<List<Concept>> taskItems)
        {
            int blankSize = CalcSizeOfBlanks(taskItems);
            int taskItemCount = CountConceptItems(taskItems);

            // Add task items to view and attach Drag and Drop handler
            llTaskItemRows.RemoveAllViews();
            foreach (var taskItemRow in taskItems)
            {
                var llTaskItemRow = (LinearLayout)LayoutInflater.From(Activity.BaseContext).Inflate(Resource.Layout.TaskItemRow, null);
                foreach (var item in taskItemRow)
                {
                    // Init container
                    var container = CreateContentContainer(Resource.Id.flConceptContainer,
                        (item.Fixed) ? CreateConceptView(item, taskItemCount, true) : CreateBlankView(item, blankSize, taskItemCount));

                    // Add drop handler
                    if (!item.Fixed)
                    {
                        container.Drag += View_Drag;
                    }

                    // Add container to task items
                    llTaskItemRow.AddView(container);
                }
                llTaskItemRows.AddView(llTaskItemRow);
            }
        }

        void BuildOptionItems(List<Concept> optionItems)
        {
            // Shuffle options
            optionItems.Shuffle();

            // Add options to view
            flOptionItems.RemoveAllViews();
            foreach (var item in optionItems)
            {
                // Init container (the additional container is used only to define a padding for IRMGARD.FlowLayout elements)
                var container = CreateContentContainer(Resource.Id.flOptionContainer, CreateConceptView(item, optionItems.Count, true));

                // Add drag handler
                container.GetChildAt(0).Touch += (sender, e) =>
                {
                    var v = (View)sender;
                    v.StartDrag(ClipData.NewPlainText("", ""), new View.DragShadowBuilder(v), v, 0);
                };

                // Add view to option items
                flOptionItems.AddView(container);
            }
        }

        ViewGroup CreateContentContainer(int resId, View child)
        {
            var container = (ViewGroup)LayoutInflater.From(Activity.BaseContext).Inflate(Resource.Layout.ContentContainer, null);
            container.Id = resId;
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
                    if (item is BaseText)
                    {
                        blankSize += (item as BaseText).Text.Length;
                    }
                    conceptItemCounter++;
                }
            }

            return blankSize / conceptItemCounter;
        }

        int CountConceptItems(List<List<Concept>> conceptItems)
        {
            int conceptItemCounter = 0;

            foreach (var conceptItemRow in conceptItems)
            {
                foreach (var item in conceptItemRow)
                {
                    conceptItemCounter++;
                }
            }

            return conceptItemCounter;
        }

        View CreateBlankView(Concept concept, int blankSize, int conceptsCount)
        {
            View view = CreateConceptView(concept, conceptsCount, false);
            if (concept is Word)
            {
                var tvText = view.FindViewById<TextView>(Resource.Id.tvText);
                tvText.Text = string.Concat(Enumerable.Repeat("a", blankSize));
            }
            else if (concept is Letter)
            {
                var tvText = view.FindViewById<TextView>(Resource.Id.tvText);
                tvText.Text = "E";
            }
            view.Visibility = ViewStates.Invisible;

            var blankView = (FrameLayout)LayoutInflater.From(Activity.BaseContext).Inflate(Resource.Layout.Blank, null);
            blankView.AddView(view);

            return blankView;
        }

        View CreateConceptView(Concept concept, int conceptsCount, bool initView)
        {
            View view;

            if (concept is BaseText)
            {
                var c = (concept as BaseText);
                view = LayoutInflater.From(Activity.BaseContext).Inflate(GetTextResource(concept as BaseText), null);
                var tvText = view.FindViewById<TextView>(Resource.Id.tvText);
                tvText.Text = c.Text;
                ApplyLetterTags(view, c);
            }
            else if (concept is Speaker)
            {
                view = LayoutInflater.From(Activity.BaseContext).Inflate(concept.Fixed ? Resource.Layout.Speaker : Resource.Layout.SpeakerCard, null);
            }
            else if (concept is Picture)
            {
                view = LayoutInflater.From(Activity.BaseContext).Inflate(concept.Fixed ? Resource.Layout.Picture : Resource.Layout.PictureCard, null);
                if (!string.IsNullOrEmpty((concept as Picture).ImagePath))
                {
                    var bitmap = BitmapLoader.Instance.LoadBitmap(conceptsCount, Activity.BaseContext, (concept as Picture).ImagePath);
                    if (bitmap != null)
                    {
                        var ivPicture = view.FindViewById<ImageView>(Resource.Id.ivPicture);
                        ivPicture.SetImageBitmap(bitmap);
                    }
                }
            }
            else
            {
                view = LayoutInflater.From(Activity.BaseContext).Inflate(Resource.Layout.Word, null);
                var tvText = view.FindViewById<TextView>(Resource.Id.tvText);
                tvText.Text = string.Format("Concept type {0} does not exist!", concept.GetType().ToString());
            }

            if (initView)
            {
                // Play sound on touch
                if (concept is ISound)
                {
                    if (!string.IsNullOrEmpty((concept as ISound).SoundPath))
                    {
                        view.Touch += (sender, e) =>
                        {
                            SoundPlayer.PlaySound(Activity.BaseContext, (concept as ISound).SoundPath);
                        };
                    }
                }

                // Attach concept object to view
                view.SetTag(Resource.Id.concept_tag_key, new JavaObjectWrapper<Concept>() { Obj = concept });
            }

            return view;
        }

        private static void ApplyLetterTags(View view, BaseText c)
        {
            if (c.LetterTags != null && c.LetterTags.Count > 0)
            {
                if (c is Letter)
                {
                    if (c.LetterTags.First() == LetterTag.Short)
                    {
                        view.FindViewById<View>(Resource.Id.shortIndicator).Visibility = ViewStates.Visible;
                    }
                    else if (c.LetterTags.First() == LetterTag.Long)
                    {
                        view.FindViewById<View>(Resource.Id.longIndicator).Visibility = ViewStates.Visible;
                    }
                }
            }
        }

        int GetTextResource(BaseText concept)
        {
            if (concept is Word)
            {
                return concept.Fixed ? Resource.Layout.Word : Resource.Layout.WordCard;
            }
            else if (concept is Syllable)
            {
                return concept.Fixed ? Resource.Layout.Syllable : Resource.Layout.SyllableCard;
            }
            else if (concept is Letter)
            {
                return concept.Fixed ? Resource.Layout.Letter : Resource.Layout.LetterCard;
            }
            else
            {
                throw new ArgumentException("Cannot find resource for text based concept.");
            }
        }

        void View_Drag(object sender, View.DragEventArgs e)
        {
            var srcView = (View) e.Event.LocalState;
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
                            FireUserInteracted(false);
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
                var llTaskItemRow = (ViewGroup)llTaskItemRows.GetChildAt(i);
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

        public override void CheckSolution()
        {
            var counter = 0;
            for (int i = 0; i < currentTaskItems.Count; i++)
            {
                var row = currentTaskItems[i];
                var llTaskItemRow = (ViewGroup)llTaskItemRows.GetChildAt(i);
                for (int k = 0; k < row.Count; k++)
                {
                    var contentView = GetCCContent(llTaskItemRow.GetChildAt(k) as ViewGroup);
                    if (contentView != null)
                    {
                        var wrapper = contentView.GetTag(Resource.Id.concept_tag_key) as JavaObjectWrapper<Concept>;
                        if (wrapper != null && wrapper.Obj != null)
                        {
                            if (row[k].Equals(wrapper.Obj))
                            {
                                counter++;
                            }
                        }
                    }
                }
            }

            FinishIteration(CountConceptItems(currentTaskItems) == counter);
        }
    }
}

