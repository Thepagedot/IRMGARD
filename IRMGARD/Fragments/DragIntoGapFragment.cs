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
        LinearLayout llSolutionItems;

        Exercise exercise;
        bool inReview;

        bool dragActionDropHandled;
        bool dragActionStartedHandled;
        bool dragActionEndedHandled;

        public override View OnCreateView (LayoutInflater inflater, ViewGroup container, Bundle savedInstanceState)
        {
            View view = inflater.Inflate(Resource.Layout.DragIntoGap, container, false);
            llTaskItemRows = view.FindViewById<LinearLayout>(Resource.Id.llTaskItemRows);
            flOptionItems = view.FindViewById<FlowLayout>(Resource.Id.flOptionItems);
            llSolutionItems = view.FindViewById<LinearLayout>(Resource.Id.llSolutionItems);

            InitIteration();

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

            // Accumulate and build option item collection
            var accOptionItems = new List<Concept>();

            // Add blank task items as options
            if (Lesson.UseOptionItemsOnly)
            {
                AddBlankConcepts(accOptionItems, exercise);
            }
            else
            {
                foreach (var task in currentIteration.Tasks)
                {
                    AddBlankConcepts(accOptionItems, task);
                }
            }

            // Add option items
            if (exercise.OptionItems != null && exercise.OptionItems.Count > 0)
            {
                accOptionItems.AddRange(exercise.OptionItems);
            }
            if (Lesson.OptionItems != null && Lesson.OptionItems.Count > 0)
            {
                foreach (var item in Lesson.OptionItems)
                {
                    if (!accOptionItems.Contains(item)) {
                        accOptionItems.Add(item);
                    }
                }
            }
            BuildOptionItems(accOptionItems);
        }

        void AddBlankConcepts(List<Concept> optionItems, Exercise task)
        {
            foreach (var taskItemRow in task.TaskItems)
            {
                optionItems.AddRange(taskItemRow.Where(ti => ti.IsBlank).Select(ti => {
                    var c = ti.DeepCopy();
                    c.IsBlank = false;
                    return c;
                }));
            }
        }

        void BuildTaskItems()
        {
            // Replace default top margin
            SetTopMargin(0, (View)llTaskItemRows.Parent);

            // Add task items to view and attach Drag and Drop handler
            llTaskItemRows.RemoveAllViews();
            foreach (var taskItemRow in exercise.TaskItems)
            {
                var llTaskItemRowRoot = LayoutInflater.From(Activity.BaseContext).Inflate(Resource.Layout.TaskItemRow, null);
                var llTaskItemRow = llTaskItemRowRoot.FindViewById<LinearLayout>(Resource.Id.llTaskItemRow);
                foreach (var item in taskItemRow)
                {
                    // Exclude special case concepts
                    if (item.ActivateOnSuccess || item.ActivateOnMistake) { continue; }

                    // Init container
                    var container = CreateContentContainer(Resource.Id.flConceptContainer,
                        (item.IsBlank)
                            ? CreateBlankView(item)
                            : CreateConceptView(item, CountPictureItems(exercise.TaskItems)));

                    // Add drop handler
                    if (item.IsBlank)
                    {
                        container.Drag += View_Drag;
                    }

                    // Add container to task items
                    llTaskItemRow.AddView(container);
                }

                // Replace top margin of task item rows
                SetTopMargin(1, llTaskItemRow);

                if (Lesson.HideRack)
                {
                    llTaskItemRowRoot.FindViewById<ImageView>(Resource.Id.ivDivider).Visibility = ViewStates.Invisible;
                }

                llTaskItemRows.AddView(llTaskItemRowRoot);
            }
        }

        void BuildOptionItems(List<Concept> optionItems)
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
                var container = CreateContentContainer(Resource.Id.flOptionContainer, CreateConceptView(item, optionItems.Count));

                // Add drag handler
                container.GetChildAt(0).Touch += ConceptView_Touch;

                // Add view to option items
                flOptionItems.AddView(container);
            }
        }

        bool BuildSolutionItems(List<Concept> solutionItems)
        {
            bool itemsCreated = false;

            // Replace top margin of solution items
            SetTopMargin(3, llSolutionItems);

            // Add solution items to view
            llSolutionItems.RemoveAllViews();
            foreach (var item in solutionItems)
            {
                // Init container
                var container = CreateContentContainer(Resource.Id.flConceptContainer, CreateConceptView(item, CountPictureItems(exercise.TaskItems)));

                // Play sound
                if (item is ISound)
                {
                    if (!string.IsNullOrEmpty((item as ISound).SoundPath))
                    {
                        SoundPlayer.PlaySound(Activity.BaseContext, (item as ISound).SoundPath);
                    }
                }

                // Add view to solution items
                llSolutionItems.AddView(container);
                itemsCreated = true;
            }

            return itemsCreated;
        }

        void SetTopMargin(int idx, View view)
        {
            bool isSmallHeight = (Resources.DisplayMetrics.HeightPixels / Resources.DisplayMetrics.Density) < 550;
            int topMarginToSet = (Lesson.TopMargins != null && Lesson.TopMargins.Length >= idx + 1) ? Lesson.TopMargins[idx] : -1;

            if (view.LayoutParameters is FrameLayout.LayoutParams)
            {
                var lp = (view.LayoutParameters as FrameLayout.LayoutParams);
                var topMargin = (topMarginToSet > -1) ? topMarginToSet : lp.TopMargin;
                lp.TopMargin = isSmallHeight ? topMargin / 2 : topMargin;
            }
            else
            {
                var lp = (view.LayoutParameters as LinearLayout.LayoutParams);
                var topMargin = (topMarginToSet > -1) ? topMarginToSet : lp.TopMargin;
                lp.TopMargin = isSmallHeight ? topMargin / 2 : topMargin;
            }
        }

        void ConceptView_Touch(object sender, EventArgs e)
        {
            var v = (View)sender;
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

        int CountPictureItems(List<List<Concept>> conceptItems)
        {
            int pictureItemCounter = 0;

            foreach (var conceptItemRow in conceptItems)
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

        string GetBlankText(Concept concept)
        {
            if (concept is Letter) { return "M"; }
            else if (concept is Syllable) { return string.Concat(Enumerable.Repeat("M", CalcSizeOfBlanks(exercise.TaskItems))); }
            else { return string.Concat(Enumerable.Repeat("a", CalcSizeOfBlanks(exercise.TaskItems))); }
        }

        View CreateBlankView(Concept concept)
        {
            View view = CreateConceptView(concept, CountPictureItems(exercise.TaskItems));
            view.Visibility = ViewStates.Invisible;

            var blankView = (FrameLayout)LayoutInflater.From(Activity.BaseContext).Inflate(Resource.Layout.Blank, null);
            blankView.AddView(view);

            return blankView;
        }

        View CreateConceptView(Concept concept, int conceptsCount)
        {
            View view;

            var inflater = LayoutInflater.From(Activity.BaseContext);
            if (concept is BaseText)
            {
                var baseText = (concept as BaseText);
                if (baseText.LetterTags != null && baseText.LetterTags.Count > 0)
                {
                    view = ApplyLetterTags(inflater, baseText);
                }
                else
                {
                    view = inflater.Inflate(Resource.Layout.BaseText, null);
                    var tvText = view.FindViewById<TextView>(Resource.Id.tvText);
                    tvText.Text = (baseText.IsBlank) ? GetBlankText(baseText) : baseText.Text;
                    SetTextColor(tvText, baseText);
                    AdjustTextSize(tvText, baseText);
                }

                if (baseText is ISound && baseText.SoundPath != null)
                {
                    var speakerDecorator = (ViewGroup)inflater.Inflate(Resource.Layout.BaseTextSpeaker, null);
                    speakerDecorator.AddView(view);
                    view = speakerDecorator;
                }

                if (baseText.IsBlank || baseText.IsOption || concept.ActivateOnSuccess || concept.ActivateOnMistake)
                {
                    var cardView = (FrameLayout)inflater.Inflate(Resource.Layout.BaseTextCard, null);
                    cardView.AddView(view);
                    view = cardView;
                }
                else
                {
                    if (!baseText.ShowAsPlainText)
                    {
                        var borderedView = (FrameLayout)inflater.Inflate(Resource.Layout.BaseTextBordered, null);
                        borderedView.AddView(view);
                        view = borderedView;
                    }
                }
            }
            else if (concept is Speaker)
            {
                view = inflater.Inflate(concept.IsBlank || concept.IsOption ? Resource.Layout.SpeakerCard : Resource.Layout.Speaker, null);
            }
            else if (concept is Picture)
            {
                view = inflater.Inflate((concept.IsBlank || concept.IsOption
                        || concept.ActivateOnSuccess || concept.ActivateOnMistake)
                    ? Resource.Layout.PictureCard : Resource.Layout.Picture, null);

                if (concept.ActivateOnSuccess || concept.ActivateOnMistake)
                {
                    (view as ViewGroup).GetChildAt(0).LayoutParameters = new FrameLayout.LayoutParams(ToPx(150), ToPx(150));
                }

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
            else if (concept is Models.Space)
            {
                view = new Android.Widget.Space(Activity.BaseContext);
                view.LayoutParameters = new LinearLayout.LayoutParams(ToPx((concept as Models.Space).Width), ViewGroup.LayoutParams.MatchParent);
            }
            else
            {
                view = inflater.Inflate(Resource.Layout.BaseText, null);
                var tvText = view.FindViewById<TextView>(Resource.Id.tvText);
                tvText.Text = string.Format("Concept type {0} does not exist!", concept.GetType().ToString());
            }

            // Attach concept object to view
            view.SetTag(Resource.Id.concept_tag_key, new JavaObjectWrapper<Concept>() { Obj = concept });

            if (!concept.IsBlank)
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
            }

            return view;
        }

        int ToPx(int dp)
        {
            return (int)(dp * Resources.DisplayMetrics.Density);
        }

        void AdjustTextSize(TextView tvText, BaseText concept)
        {
            if (concept.TextSize > 0)
            {
                tvText.SetTextSize(Android.Util.ComplexUnitType.Sp, concept.TextSize);
            }
            else
            {
                if (concept is Letter)
                {
                    tvText.SetTextSize(Android.Util.ComplexUnitType.Dip, 28);
                }
                else if (concept is Syllable)
                {
                    tvText.SetTextSize(Android.Util.ComplexUnitType.Dip, 24);
                }
                else if (concept is Word)
                {
                    tvText.SetTextSize(Android.Util.ComplexUnitType.Dip, 18);
                }
            }
        }

        View ApplyLetterTags(LayoutInflater inflater, BaseText concept)
        {
            string text = concept.IsBlank ? GetBlankText(concept) : concept.Text;

            if (concept is Word || concept is Syllable)
            {
                var viewGroup = (ViewGroup)inflater.Inflate(Resource.Layout.BaseTextGroup, null);
                for (int i = 0; i < text.Length; i++)
                {
                    var view = inflater.Inflate(Resource.Layout.BaseText, null);
                    var tvText = view.FindViewById<TextView>(Resource.Id.tvText);
                    tvText.Text = char.ToString(text[i]);
                    SetTextColor(tvText, concept);
                    AdjustTextSize(tvText, concept);
                    EnableIndicator(view, concept, concept.LetterTags[i]);
                    viewGroup.AddView(view);
                }

                return viewGroup;
            }
            else if (concept is Letter)
            {
                var view = inflater.Inflate(Resource.Layout.BaseText, null);
                var tvText = view.FindViewById<TextView>(Resource.Id.tvText);
                tvText.Text = text;
                SetTextColor(tvText, concept);
                AdjustTextSize(tvText, concept);
                EnableIndicator(view, concept, concept.LetterTags.First());

                return view;
            }
            else
            {
                throw new InvalidCastException("Unknown concept of type BaseText!");
            }
        }

        void SetTextColor(TextView tvText, BaseText concept)
        {
            if (!string.IsNullOrEmpty(concept.Color))
            {
                tvText.SetTextColor(Android.Graphics.Color.ParseColor(concept.Color));
            }
        }

        void EnableIndicator(View letterView, BaseText concept, LetterTag letterTag)
        {
            if (letterTag == LetterTag.Short)
            {
                var view = letterView.FindViewById<View>(Resource.Id.shortIndicator);
                SetDrawableColor(view, concept);
                view.Visibility = ViewStates.Visible;
            }
            else if (letterTag == LetterTag.Long)
            {
                var view = letterView.FindViewById<View>(Resource.Id.longIndicator);
                SetDrawableColor(view, concept);
                view.Visibility = ViewStates.Visible;
            }
        }

        void SetDrawableColor(View view, BaseText concept)
        {
            if (!string.IsNullOrEmpty(concept.Color))
            {
                var background = view.Background as Android.Graphics.Drawables.GradientDrawable;
                background.SetColor(Android.Graphics.Color.ParseColor(concept.Color).ToArgb());
            }
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
                            contentView.Touch -= ConceptView_Touch;

                            var contentViewConcept = (contentView.GetTag(Resource.Id.concept_tag_key) as JavaObjectWrapper<Concept>).Obj;
                            var blankViewConcept = (blankView.GetChildAt(1).GetTag(Resource.Id.concept_tag_key) as JavaObjectWrapper<Concept>).Obj;
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
                concepts.AddRange(conceptItemRow.Where(c => c.IsBlank));
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
                    optionView.Touch -= ConceptView_Touch;

                    var optionViewConcept = (optionView.GetTag(Resource.Id.concept_tag_key) as JavaObjectWrapper<Concept>).Obj;
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

        List<Concept> GetConceptsToActivateOnCheckSolution(bool success)
        {
            var concepts = new List<Concept>();
            foreach (var taskItemRow in exercise.TaskItems)
            {
                concepts.AddRange(taskItemRow.Where(ti => (success) ? ti.ActivateOnSuccess : ti.ActivateOnMistake));
            }

            return concepts;
        }

        public override void CheckSolution()
        {
            if (inReview)
            {
                // User touched the next button for the second time
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

