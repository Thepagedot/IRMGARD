using System;
using System.Linq;

using Android.Graphics.Drawables;
using Android.OS;
using Android.Views;
using Android.Widget;

using IRMGARD.Models;
using IRMGARD.Shared;
using System.Collections.Generic;

namespace IRMGARD
{
    public static class SelectConceptFragmentFactory
    {
        public static SelectConceptFragment Get(LevelType levelType)
        {
            switch (levelType)
            {
                case LevelType.DefArtFPSC:
                    return new DefArtFPSCFragment();
                case LevelType.ShortLongVowelFPSC:
                    return new ShortLongVowelFPSCFragment();
                case LevelType.FourPicturesSelectConcept:
                    return new FourPicturesSelectConceptFragment();
                case LevelType.WordqueueSC:
                    return new WordqueueSCFragment();
                case LevelType.LetterpuzzleSC:
                    return new LetterpuzzleSCFragment();
                case LevelType.SpeakerSC:
                    return new SpeakerSCFragment();
                case LevelType.SelectConcept:
                default:
                    return new SelectConceptFragment();
            }
        }
    }

    public class WordqueueSCFragment : SelectConceptFragment
    {
        const int Padding = 4;

        const int solutionWordCount = 3;
        const int optionWordCount = 12;
        const int LettersPerRow = 30;

        LinearLayout llHeaderItems;

        protected override void InitBaseLayoutView(View layoutView)
        {
            llHeaderItems = layoutView.FindViewById<LinearLayout>(Resource.Id.llHeaderItems);

            var llTaskItemRows = layoutView.FindViewById<LinearLayout>(Resource.Id.llTaskItemRows);
            llTaskItemRows.SetPadding(ToPx(Padding), ToPx(Padding), ToPx(Padding), ToPx(Padding));    // Set spacing
            llTaskItemRows.SetBackgroundColor(Android.Graphics.Color.White);

            base.InitBaseLayoutView(layoutView);
        }

        protected override void TransformTaskItems()
        {
            var selectConcept = Lesson.DeepCopy();

            var artDef = selectConcept.NamedConcepts["ArtDef"];
            var artUndef = selectConcept.NamedConcepts["ArtUndef"];
            var persPronSing = selectConcept.NamedConcepts["PersPronSing"];
            var persPronPlu = selectConcept.NamedConcepts["PersPronPlu"];
            var nounSing = selectConcept.NamedConcepts["NounSing"];
            var nounPlu = selectConcept.NamedConcepts["NounPlu"];
            var verbPres = selectConcept.NamedConcepts["VerbPres"];
            var verbPast = selectConcept.NamedConcepts["VerbPast"];

            var concepts = new List<Concept>();

            var random = new Random();
            switch (random.Next(0, 8))
            {
                case 0:
                    AddToConcepts(concepts, artDef, artUndef.Concat(persPronSing.Concat(nounSing)).ToList());
                    break;
                case 1:
                    AddToConcepts(concepts, artUndef, artDef.Concat(persPronSing.Concat(nounPlu)).ToList());
                    break;
                case 2:
                    AddToConcepts(concepts, persPronSing, artUndef.Concat(persPronPlu.Concat(nounSing)).ToList());
                    break;
                case 3:
                    AddToConcepts(concepts, persPronPlu, artDef.Concat(persPronPlu.Concat(nounPlu)).ToList());
                    break;
                case 4:
                    AddToConcepts(concepts, nounSing, verbPres.Concat(nounPlu.Concat(persPronSing)).ToList());
                    break;
                case 5:
                    AddToConcepts(concepts, nounPlu, verbPast.Concat(nounSing.Concat(artUndef)).ToList());
                    break;
                case 6:
                    AddToConcepts(concepts, verbPres, nounPlu.Concat(verbPast.Concat(persPronPlu)).ToList());
                    break;
                case 7:
                    AddToConcepts(concepts, verbPast, nounSing.Concat(verbPres.Concat(artDef)).ToList());
                    break;
            }

            if (exercise == null)
            {
                exercise = new SelectConceptExercise();
            }
            exercise.TaskItems = new List<List<Concept>>();
            int letterCount = 0;
            var row = new List<Concept>();
            foreach (var concept in concepts)
            {
                var word = concept as Word;
                word.ShowAsPlainText = true;
                if (letterCount + word.Text.Length > LettersPerRow)
                {
                    exercise.TaskItems.Add(row);
                    letterCount = 0;
                    row = new List<Concept>();
                }

                letterCount += word.Text.Length;
                row.Add(word);
            }
            exercise.TaskItems.Add(row);

            Lesson.LeftAlignItems = Enumerable.Repeat<bool>(true, exercise.TaskItems.Count).ToArray();
        }

        void AddToConcepts(List<Concept> concepts, List<Concept> solutionConcepts, List<Concept> optionConcepts)
        {
            // Add speaker to header items row
            llHeaderItems.RemoveAllViews();
            llHeaderItems.AddView(CreateConceptView(solutionConcepts.FirstOrDefault()));
            llHeaderItems.Visibility = ViewStates.Visible;

            // Add solutions
            concepts.AddRange(solutionConcepts.Where(item => item is Word).PickRandomItems(solutionWordCount).Select(item => { item.IsSolution = true; return item; }));

            // Add options
            concepts.AddRange(optionConcepts.Where(item => item is Word).PickRandomItems(optionWordCount).Select(item => { item.IsOption = true; return item; }));

            concepts.Shuffle();
        }

        protected override View CreateAndInitConceptView(Concept concept)
        {
            var container = base.CreateAndInitConceptView(concept) as ViewGroup;
            var lp = container.LayoutParameters as LinearLayout.LayoutParams;
            lp.SetMargins(0, 0, 0, 0);           // Set spacing
            container.SetPadding(0, 0, 0, 0);    // Set spacing

            return container;
        }

        protected override bool IsTextCardCallback(BaseText concept)
        {
            return false;
        }

        public override void CheckSolution()
        {
            for (int i = 0; i < llTaskItemRows.ChildCount; i++)
            {
                var view = (ViewGroup)llTaskItemRows.GetChildAt(i);
                var llTaskItemRow = view.FindViewById<LinearLayout>(Resource.Id.llTaskItemRow);
                for (int k = 0; k < llTaskItemRow.ChildCount; k++)
                {
                    var conceptContainer = llTaskItemRow.GetChildAt(k) as ViewGroup;
                    var lp = conceptContainer.LayoutParameters as LinearLayout.LayoutParams;
                    conceptContainer.SetPadding(ToPx(2), ToPx(2), ToPx(2), ToPx(2));    // Set spacing
                }
            }

            base.CheckSolution();
        }
    }

    public class LetterpuzzleSCFragment : SelectConceptFragment
    {
        const string RandChars = "WGRKBDRGPNFMRSWHFGWKMGCXOJGZRJDSLMFRJBQRFHNKJDWQKCGSHB";
        const int Cols = 8;
        const int Rows = 8;
        const int Padding = 4;

        char[] randChars;
        int randCharsIdx;
        List<Concept> words;

        protected override void OnCreateViewConfig()
        {
            SelectedBackgroundResourceId = Resource.Drawable.letterpuzzle_marker;
        }

        protected override void InitBaseLayoutView(View layoutView)
        {
            var llTaskItemRows = layoutView.FindViewById<LinearLayout>(Resource.Id.llTaskItemRows);
            llTaskItemRows.SetPadding(ToPx(Padding), ToPx(Padding), ToPx(Padding), ToPx(Padding));    // Set spacing
            llTaskItemRows.SetBackgroundColor(Android.Graphics.Color.White);

            base.InitBaseLayoutView(layoutView);
        }

        protected override void TransformTaskItems()
        {
            exercise = exercise.DeepCopy();

            var random = new Random();

            randChars = RandChars.ToArray<char>();
            randCharsIdx = random.Next(0, randChars.Length);

            words = exercise.TaskItems[0];
            words.Shuffle();

            exercise.TaskItems = new List<List<Concept>>();
            foreach (var concept in words)
            {
                var word = concept as Word;

                var row = new List<Concept>();
                var start = random.Next(0, (Cols - word.Text.Length) + 1);
                while (row.Count < Cols)
                {
                    if (row.Count == start)
                    {
                        row.AddRange(ToLetters(word.Text));
                    }
                    else
                    {
                        row.Add(GetRandomLetter(random));
                    }
                }
                exercise.TaskItems.Add(row);
            }

            for (int i = 0; i < (Rows - words.Count); i++)
            {
                exercise.TaskItems.Insert(random.Next(0, exercise.TaskItems.Count), CreateDummyRow(random));
            }
        }

        Letter GetRandomLetter(Random random)
        {
            // return CreateLetter(false, char.ToString((char)('A' + random.Next(0, 26))));

            randCharsIdx = (randCharsIdx + 1 < randChars.Length) ? ++randCharsIdx : 0;
            return CreateLetter(false, char.ToString(randChars[randCharsIdx]));
        }

        List<Concept> CreateDummyRow(Random random)
        {
            var letters = new List<Concept>();
            for (int i = 0; i < Cols; i++)
            {
                letters.Add(GetRandomLetter(random));
            }

            return letters;
        }

        List<Concept> ToLetters(string word)
        {
            var letters = new List<Concept>();
            foreach (var letter in word.AsEnumerable())
            {
                letters.Add(CreateLetter(true, char.ToString(letter)));
            }

            return letters;
        }

        Letter CreateLetter(bool isSolution, string text)
        {
            var letter = new Letter();
            if (isSolution)
            {
                letter.IsSolution = true;
            }
            else
            {
                letter.IsOption = true;
            }
            letter.Text = text;
            letter.ShowAsPlainText = true;
            letter.AddToTextSize = -6;

            return letter;
        }

        protected override View CreateAndInitConceptView(Concept concept)
        {
            var container = base.CreateAndInitConceptView(concept) as ViewGroup;

            // Adjust padding for letter views rendered as plain text
            container.GetChildAt(0).SetPadding(ToPx(Padding), 0, ToPx(Padding), 0);    // Set spacing

            return container;
        }

        protected override bool IsTextCardCallback(BaseText concept)
        {
            return false;
        }

        protected override bool CheckUserInteracted()
        {
            bool hasUserInteracted = false;

            // Check if the user is done and for any words found
            for (int i = 0; i < llTaskItemRows.ChildCount; i++)
            {
                var view = (ViewGroup)llTaskItemRows.GetChildAt(i);
                var llTaskItemRow = view.FindViewById<LinearLayout>(Resource.Id.llTaskItemRow);
                var chars = new List<char>();
                var viewsFound = new List<View>();
                for (int k = 0; k < llTaskItemRow.ChildCount; k++)
                {
                    var containerView = llTaskItemRow.GetChildAt(k) as ViewGroup;
                    var contentView = containerView.GetChildAt(0);
                    if (GetTag<Drawable>(contentView, Resource.Id.selected_tag_key) != null)
                    {
                        var concept = GetTag<Concept>(contentView, Resource.Id.concept_tag_key) as Letter;
                        chars.Add(char.Parse(concept.Text));
                        viewsFound.Add(contentView);

                        hasUserInteracted = true;
                    }
                }

                var word = new string(chars.ToArray());
                if (words.Any(w => (w as Word).Text.Equals(word)))
                {
                    viewsFound.ForEach(v => v.SetBackgroundResource(Resource.Drawable.highlighted_background));
                }
            }

            return hasUserInteracted;
        }
    }

    public class SpeakerSCFragment : SelectConceptFragment
    {
        protected override void TransformTaskItems()
        {
            exercise = exercise.DeepCopy();

            var concepts = new List<Concept>();
            concepts.AddRange(exercise.TaskItems[0]);
            concepts.AddRange(Lesson.OptionItems.PickRandomItems(7).Select(item => { item.IsOption = true; return item; }));
            concepts.Shuffle();

            exercise.TaskItems = new List<List<Concept>>();
            exercise.TaskItems.Add(concepts.GetRange(0, 3));
            exercise.TaskItems.Add(concepts.GetRange(3, 3));
            exercise.TaskItems.Add(concepts.GetRange(6, 3));
        }
    }

    public class SelectConceptFragment : BaseConceptLayoutFragment<SelectConcept>
    {
        // The current selected view item for SelectConcept.SingleChoice=true
        View selectedItem;

        protected int SelectedBackgroundResourceId { get; set; }

        public override View OnCreateView(LayoutInflater inflater, ViewGroup container, Bundle savedInstanceState)
        {
            View view = inflater.Inflate(Resource.Layout.SelectConcept, container, false);
            InitBaseLayoutView(view);

            return view;
        }

        protected override void InitIteration()
        {
            base.InitIteration();

            // Reset selected item
            selectedItem = null;

            var currentIteration = GetCurrentIteration<SelectConceptIteration>();

            if (currentIteration.Tasks != null)
            {
                // Random select an exercise from this iteration
                exercise = currentIteration.Tasks.PickRandomItems(1).FirstOrDefault();
            }

            // Transform stored task item configuration
            TransformTaskItems();

            // Build task items
            BuildTaskItems();
        }

        protected override View CreateAndInitConceptView(Concept concept)
        {
            var conceptView = CreateConceptView(concept);
            var container = CreateConceptContainer(conceptView);
            AddSelectHandler(conceptView);

            return container;
        }

        void AddSelectHandler(View conceptView)
        {
            var concept = GetTag<Concept>(conceptView, Resource.Id.concept_tag_key);
            if (concept != null)
            {
                if (concept.IsOption || concept.IsSolution)
                {
                    if (concept is Speaker)
                    {
                        conceptView.Click -= ConceptView_Click_PlaySound;
                        conceptView.Click += ConceptView_Click_PlaySound_Click_SelectItem;
                    }
                    else
                    {
                        conceptView.Click += ConceptView_Click_SelectItem;
                    }
                }
            }
        }

        void RemoveSelectHandler(View conceptView)
        {
            var concept = GetTag<Concept>(conceptView, Resource.Id.concept_tag_key);
            if (concept != null)
            {
                if (concept is Speaker)
                {
                    conceptView.Click -= ConceptView_Click_PlaySound_Click_SelectItem;
                    conceptView.Click += ConceptView_Click_PlaySound;
                }
                else
                {
                    conceptView.Click -= ConceptView_Click_SelectItem;
                }
            }
        }

        void ConceptView_Click_SelectItem(object sender, EventArgs e)
        {
            var view = sender as View;

            var drawable = GetTag<Drawable>(view, Resource.Id.selected_tag_key);
            if (drawable == null)
            {
                if (Lesson.SingleChoice)
                {
                    if (selectedItem != null)
                    {
                        UnselectItem(selectedItem);
                    }
                    selectedItem = view;
                }
                SetTag(view, Resource.Id.selected_tag_key, view.Background != null
                    ? view.Background
                    : new ColorDrawable(Android.Graphics.Color.Transparent));
                view.SetBackgroundResource(SelectedBackgroundResourceId > 0 ? SelectedBackgroundResourceId : Resource.Drawable.selected_background);
            }
            else
            {
                if (Lesson.SingleChoice)
                {
                    selectedItem = null;
                }
                UnselectItem(view);
            }

            FireUserInteracted(CheckUserInteracted());
        }

        void UnselectItem(View view)
        {
            var drawable = GetTag<Drawable>(view, Resource.Id.selected_tag_key);
            if (drawable != null)
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
                    var concept = GetTag<Concept>(view, Resource.Id.concept_tag_key);
                    layout.AddView(CreateAndInitConceptView(concept), idx);
                }
                SetTag<Drawable>(view, Resource.Id.selected_tag_key, null);
            }
        }

        void ConceptView_Click_PlaySound_Click_SelectItem(object sender, EventArgs e)
        {
            var view = sender as View;

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
                    var conceptView = containerView.GetChildAt(0);
                    if (GetTag<Drawable>(conceptView, Resource.Id.selected_tag_key) != null)
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
                for (int k = 0; k < llTaskItemRow.ChildCount; k++)
                {
                    var conceptContainer = llTaskItemRow.GetChildAt(k) as ViewGroup;
                    var conceptView = conceptContainer.GetChildAt(0);
                    if (conceptView != null)
                    {
                        var concept = GetTag<Concept>(conceptView, Resource.Id.concept_tag_key);
                        if (concept.IsOption)
                        {
                            RemoveSelectHandler(conceptView);
                            if (GetTag<Drawable>(conceptView, Resource.Id.selected_tag_key) != null)
                            {
                                MoveConceptContainer(conceptContainer);
                                conceptContainer.SetBackgroundResource(Resource.Drawable.rectangle_red);
                                correct = false;
                            }
                        }
                        else if (concept.IsSolution)
                        {
                            RemoveSelectHandler(conceptView);
                            if (GetTag<Drawable>(conceptView, Resource.Id.selected_tag_key) == null)
                            {
                                correct = false;
                            }
                            MoveConceptContainer(conceptContainer);
                            conceptContainer.SetBackgroundResource(Resource.Drawable.rectangle_green);
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

