﻿using System;
using IRMGARD.Models;
using Android.Widget;
using System.Collections.Generic;
using System.Linq;
using Android.Content;
using Android.Views;
using IRMGARD.Shared;

namespace IRMGARD
{
    public class LetterDropFragment : LessonFragment<LetterDrop>
    {
        private LinearLayout llTaskItems;
        private FlowLayout flTaskItems;
        private FlowLayout flLetters;

        private bool useAlternateView;
        private float letterTextSize;

        public override View OnCreateView(LayoutInflater inflater, ViewGroup container, Android.OS.Bundle savedInstanceState)
        {
            // Prepare view
            var view = inflater.Inflate(Resource.Layout.LetterDrop, container, false);
            llTaskItems = view.FindViewById<LinearLayout>(Resource.Id.llTaskItems);
            llTaskItems.ViewTreeObserver.GlobalLayout += Layout_ViewChange;
            flTaskItems = view.FindViewById<FlowLayout>(Resource.Id.flTaskItems);
            flLetters = view.FindViewById<FlowLayout>(Resource.Id.flLetters);

            // Initialize iteration
            InitIteration();

            return view;
        }

        protected override void InitIteration()
        {
            base.InitIteration();

            var currentIteration = GetCurrentIteration<LetterDropIteration>();

            // Randomize font case
            var random = new Random();
            var fontCase = (Case)(random.Next(2) + 1);

            // Generate Options
            currentIteration.Options = GenerateOptions(currentIteration, 10, fontCase);

            // Generate Task letters
            currentIteration.TaskItems = new List<TaskItem>();
            foreach (var letter in currentIteration.LettersToLearn)
            {
                var taskLetter = new TaskLetter(letter.ToCase(fontCase));
                var taskItem = new TaskItem(taskLetter, null, true);
                currentIteration.TaskItems.Add(taskItem);
            }

            currentIteration.TaskItems.Shuffle();
            flLetters.RemoveAllViews();

            // Add options to view
            var letterAdapter = new LetterAdapter(Activity.BaseContext, 0, currentIteration.Options);
            for (var i = 0; i < currentIteration.Options.Count; i++)
            {
                // Add letter to view
                var view = letterAdapter.GetView(i, null, null);
                var letter = currentIteration.Options.ElementAt(i).Letter;

                // Add drag
                view.Touch += (sender, e) => {
                    var data = ClipData.NewPlainText("letter", letter);
                    (sender as View).StartDrag(data, new View.DragShadowBuilder(sender as View), null, 0);
                };

                flLetters.AddView(view);
            }

            // Add task letters to view
            useAlternateView = currentIteration.TaskItems.Count > 5;
            llTaskItems.Visibility = (useAlternateView) ? ViewStates.Gone : ViewStates.Visible;
            flTaskItems.Visibility = (useAlternateView) ? ViewStates.Visible : ViewStates.Gone;
            BuildTaskLetters(currentIteration.TaskItems);
        }

        private List<LetterBase> GenerateOptions(LetterDropIteration iteration, int numberOfOptions, Case fontCase)
        {
            var random = new Random();

            // Add correct options
            var options = iteration.LettersToLearn.Select(letter => new LetterBase(letter.ToNegativeCase(fontCase))).ToList();

            // Do not use upper case i and lower case L as options (similarity issue)
            var UCI = "I";
            var LCL = "l";

            // Add false options with random cases
            while (options.Count < numberOfOptions)
            {
                var letter = Alphabet.GetRandomLetter().ToCase((Case)(random.Next(2) + 1));
                while (options.FirstOrDefault(o => o.Letter.Equals(letter)) != null || letter.Equals(UCI) || letter.Equals(LCL))
                {
                    letter = Alphabet.GetRandomLetter().ToCase((Case)(random.Next(2) + 1));
                }

                options.Add(new LetterBase(letter));
            }

            options.Shuffle();
            return options;
        }

        private void BuildTaskLetters(List<TaskItem> taskItems)
        {
            if (useAlternateView)
            {
                flTaskItems.RemoveAllViews();
                flTaskItems.HorizontalSpacing = (int) (8 * Resources.DisplayMetrics.Density);
            }
            else
            {
                llTaskItems.RemoveAllViews();

            }
            var taskItemAdapter = new TaskItemAdapter(Activity.BaseContext, 0, taskItems);
            for (var i = 0; i < taskItems.Count; i++)
            {
                var view = taskItemAdapter.GetView(i, null, null);

                // Use different text size
                var letter = view.FindViewById<TextView>(Resource.Id.letter);
                letterTextSize = letter.TextSize / Resources.DisplayMetrics.ScaledDensity;

                // Define searched letters as drop zone
                if (taskItems.ElementAt(i).IsSearched)
                    view.Drag += View_Drag;

                // Add letter to view
                if (useAlternateView)
                {
                    var llLayout = view.FindViewById<RelativeLayout>(Resource.Id.llLayout);
                    llLayout.SetMinimumWidth((int) (80 * Resources.DisplayMetrics.Density));
                    flTaskItems.AddView(view);
                }
                else
                {
                    llTaskItems.AddView(view);
                }
            }
        }

        void Layout_ViewChange(object sender, EventArgs e)
        {
            if (!useAlternateView)
            {
                var firstChild = llTaskItems.GetChildAt(0);
                var lastChild = llTaskItems.GetChildAt(llTaskItems.ChildCount - 1);
                if (lastChild.Height > firstChild.Height || lastChild.Width < firstChild.Width / 2)
                {
                    letterTextSize -= 2f;
                    for (int i = 0; i < llTaskItems.ChildCount; i++)
                    {
                        llTaskItems.GetChildAt(i).FindViewById<TextView>(Resource.Id.letter).TextSize = letterTextSize;
                    }
                }
            }
        }

        private void View_Drag(object sender, View.DragEventArgs e)
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
                        var taskLetters = GetCurrentIteration<LetterDropIteration>().TaskItems;
                        var draggedLetter = data.GetItemAt(0).Text;
                        var position = (useAlternateView) ? flTaskItems.IndexOfChild(sender as View) : llTaskItems.IndexOfChild(sender as View);

                        // Get case of dragged letter
                        var fontCase = draggedLetter.GetCase(); 

                        // Check if selection is correct
                        if (taskLetters[position].IsSearched && taskLetters[position].TaskLetter.CorrectLetter.Equals(draggedLetter.ToNegativeCase(fontCase)))
                            taskLetters[position].TaskLetter.IsCorrect = true;

                        // Rebuild task letters
                        if (taskLetters[position].TaskLetter.Letter.Length > 1)
                            taskLetters[position].TaskLetter.Letter = taskLetters[position].TaskLetter.Letter.Remove(1);

                        taskLetters[position].TaskLetter.Letter += draggedLetter;
                        taskLetters[position].IsDirty = true;

                        var isReady = taskLetters.Count(t => t.IsSearched && !t.IsDirty) == 0;
                        FireUserInteracted(isReady);
                        BuildTaskLetters(taskLetters);
                    }

                    break;
            }
        }

        public override void CheckSolution()
        {
            var success = GetCurrentIteration<LetterDropIteration>().TaskItems.All(taskItem => taskItem.TaskLetter.IsCorrect);
            FinishIteration(success);
        }
    }
}

