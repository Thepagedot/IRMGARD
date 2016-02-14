using System;
using IRMGARD.Models;
using System.Collections.Generic;
using System.Linq;
using Android.Views;
using Android.OS;
using Android.Widget;
using Android.Graphics;
using Android.Content;
using IRMGARD.Shared;

namespace IRMGARD
{
    public class BuildSyllableFragment : LessonFragment<BuildSyllable>
    {
        LinearLayout llTaskItems;
        LinearLayout llSoundItems;
        FlowLayout flLetters;

        SyllablesToLearn currentSyllablesToLearn;

        public BuildSyllableFragment(Lesson lesson) : base(lesson) {}

        public override View OnCreateView (LayoutInflater inflater, ViewGroup container, Bundle savedInstanceState)
        {
            // Prepare view
            var view = inflater.Inflate(Resource.Layout.BuildSyllable, container, false);
            llTaskItems = view.FindViewById<LinearLayout>(Resource.Id.llTaskItems);
            llSoundItems = view.FindViewById<LinearLayout>(Resource.Id.llSoundItems);
            flLetters = view.FindViewById<FlowLayout> (Resource.Id.flLetters);
			
            // Initialize iteration
            InitIteration();
            return view;
        }

        protected override void InitIteration()
        {
            base.InitIteration();

            // Pick random syllable(s) from pool
            currentSyllablesToLearn = GetCurrentIteration<BuildSyllableIteration>().SyllablePool.PickRandomItems(1).FirstOrDefault();

            // Reset drop zone
            foreach (var syllable in currentSyllablesToLearn.Syllables)
            {
                foreach (var taskItem in syllable.SyllableParts)
                {
                    taskItem.IsDirty = false;
                    taskItem.TaskLetter.Letter = String.Empty;
                }
            }

            // Generate options
            currentSyllablesToLearn.Options = GenerateOptions(currentSyllablesToLearn, 10);

            BuildOptions(currentSyllablesToLearn.Options);
            BuildTaskLetters(currentSyllablesToLearn.Syllables);
            BuildSyllableSoundElements(currentSyllablesToLearn.Syllables);
        }

        List<LetterBase> GenerateOptions(SyllablesToLearn syllablesToLearn, int numberOfOptions)
        {
            var options = new List<LetterBase>();
            var random = new Random();

            // Add correct options
            foreach (var syllable in syllablesToLearn.Syllables)
                foreach (var letter in syllable.SyllableParts)
                    options.Add(new LetterBase(letter.TaskLetter.CorrectLetter, letter.TaskLetter.IsShort, letter.TaskLetter.IsLong));

            // Add false options
            while (options.Count < numberOfOptions)
            {
                var randomLetter = Alphabet.GetRandomLetter();
                if (syllablesToLearn.HasLongAndShortLetters)
                {                    
                    options.Add(new LetterBase(randomLetter, random.Next(1) == 0, random.Next(1) == 0));
                }
                else
                {
                    options.Add(new LetterBase(randomLetter));
                }
            }

            options.Shuffle();
            return options;
        }

        void BuildOptions(List<LetterBase> options)
        {
            flLetters.RemoveAllViews();
            var adapter = new LetterAdapter(Activity.BaseContext, 0, options);
            for (int i = 0; i < options.Count; i++) 
            {
                // Add letter to view
                var view = adapter.GetView(i, null, null);
                var letter = options.ElementAt (i).Letter;

                view.Touch += (sender, e) => {
                    var data = ClipData.NewPlainText ("letter", letter);
                    (sender as View).StartDrag (data, new View.DragShadowBuilder (sender as View), null, 0);
                };

                flLetters.AddView (view);
            }
        }

        private void BuildTaskLetters(List<Syllable> syllables)
        {
            llTaskItems.RemoveAllViews();
            for (var i = 0; i < syllables.Count(); i++)
            {
                var syllable = syllables.ElementAt(i);
                var adapter = new TaskItemAdapter(Activity.BaseContext, 0, syllable.SyllableParts);

                for (int j = 0; j < syllable.SyllableParts.Count; j++)
                {
                    var view = adapter.GetView(j, null, null);
                    view.Drag += View_Drag;    

                    // Add letter to view
                    llTaskItems.AddView(view);
                }

                // Add + icon
                if (i != syllables.Count - 1)
                {
                    var divider = new TextView(Activity.BaseContext);
                    divider.LayoutParameters = new ViewGroup.LayoutParams(ViewGroup.LayoutParams.WrapContent, ViewGroup.LayoutParams.WrapContent);
                    divider.Gravity = GravityFlags.Center;
                    divider.TextSize = 36f;
                    divider.SetTextColor(Color.Black);
                    divider.Text = "+";
                    llTaskItems.AddView(divider);
                }
            }
        }

        void PlaySoundOnImageClick (object sender, EventArgs e)
        {
            LinearLayout layout = (sender as LinearLayout);

            if (layout != null)
            {
                int index = ((ViewGroup)layout.Parent).IndexOfChild(layout);
                if (index >= 0)
                {
                    SoundPlayer.PlaySound(Activity.BaseContext, currentSyllablesToLearn.Syllables.ElementAt(index).SoundPath);
                }
            }

        }

        private void BuildSyllableSoundElements(List<Syllable> syllables)
        {
            llSoundItems.RemoveAllViews();
            foreach (var syllable in syllables)
            {
                var addMultiIcon = false;
                var index = syllables.IndexOf(syllable);
                if (index > -1 && syllables.Count > 1 && (index + 1 < syllables.Count))
                {
                    addMultiIcon = true;
                } 

                var mediaElementAdapter = new BuildSyllableMediaElementAdapter(Activity.BaseContext, 0, syllables, addMultiIcon);
                var view = mediaElementAdapter.GetView(0, null, null);  
                view.Click += PlaySoundOnImageClick;
                llSoundItems.AddView(view);
            }

        }            

        void View_Drag (object sender, View.DragEventArgs e)
        {
            // React on different dragging events
            var evt = e.Event;
            switch (evt.Action) 
            {
                case DragAction.Ended:  
                case DragAction.Started:
                    e.Handled = true;
                    break;           
                case DragAction.Drop:
                    e.Handled = true;

                    // Try to get clip data
                    var data = e.Event.ClipData;
                    if (data != null)
                    {
                        var taskItems = currentSyllablesToLearn.Syllables;
                        var draggedLetter = data.GetItemAt(0).Text;
                        var position = llTaskItems.IndexOfChild(sender as View);

                        // Adjust position by consiering the "+" icons
                        var count = 0;
                        for (var i = 0; i < taskItems.Count; i++)
                        {
                            count += taskItems[i].SyllableParts.Count;
                            if (count <= position - 1)
                            {
                                position -= 1;
                            }
                        }

                        // Check if selection is correct
                        var index = position;
                        foreach (var taskLetter in taskItems)
                        {
                            if (taskLetter.SyllableParts.Count > index)
                            {
                                taskLetter.SyllableParts.ElementAt(index).TaskLetter.Letter = draggedLetter;
                                taskLetter.SyllableParts.ElementAt(index).IsDirty = true;
                                taskLetter.SyllableParts.ElementAt(index).TaskLetter.IsCorrect = taskLetter.SyllableParts.ElementAt(index).TaskLetter.CorrectLetter == draggedLetter;
                                break;
                            }
                            else
                            {
                                index -= taskLetter.SyllableParts.Count;
                            }
                        }

                        var isReady = true;
                        foreach (var taskLetter in taskItems)
                            foreach (var syllable in taskLetter.SyllableParts)
                                if (!syllable.IsDirty)
                                {
                                    isReady = false;
                                    break;
                                }                               

                        FireUserInteracted(isReady);
                        BuildTaskLetters(taskItems);
                    }

                    break;
            }
        }

        public override void CheckSolution()
        {
            var success = true;
            foreach (var syllable in currentSyllablesToLearn.Syllables)
            {
                foreach (var taskLetter in syllable.SyllableParts)
                {
                    if (!taskLetter.TaskLetter.IsCorrect)
                    {
                        success = false;                  
                        break;
                    }
                }
            }
                
            FinishIteration(success);
        }
    }
}    