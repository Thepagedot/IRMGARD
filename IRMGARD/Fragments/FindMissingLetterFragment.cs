
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Android.App;
using Android.Content;
using Android.OS;
using Android.Runtime;
using Android.Util;
using Android.Views;
using Android.Widget;
using IRMGARD.Models;
using IRMGARD.Shared;

namespace IRMGARD
{
    public class FindMissingLetterFragment : LessonFragment<FindMissingLetter>
    {
        private LinearLayout llTaskItems;
        private FlowLayout flLetters;
        private Case fontCase;
        List<Iteration> iterationsBackup;

        public override View OnCreateView(LayoutInflater inflater, ViewGroup container, Bundle savedInstanceState)
        {
            // Prepare view
            var view = inflater.Inflate(Resource.Layout.FindMissingLetter, container, false);
            llTaskItems = view.FindViewById<LinearLayout>(Resource.Id.llTaskItems);
            flLetters = view.FindViewById<FlowLayout>(Resource.Id.flLetters);

            // Backup iterations loaded
            iterationsBackup = new List<Iteration>(Lesson.Iterations);

            // Shuffle Iterations
            Lesson.Iterations.Shuffle();

            // Initialize iteration
            InitIteration();
            return view;
        }

        protected override void InitIteration()
        {
            base.InitIteration();

            var currentIteration = GetCurrentIteration<FindMissingLetterIteration>();

            // Set random font case for whole iteration
            fontCase = Case.Lower;
            if (currentIteration.RandomizeCase)
            {
                var random = new Random();
                fontCase = (Case)(random.Next(2) + 1);
            }

            // Create task letters
            ResetCorrectTaskLetter(currentIteration.TaskItems);
            BuildTaskLetters(currentIteration.TaskItems, fontCase);

            // Generate options
            currentIteration.Options = GenerateOptions(currentIteration, 10, fontCase);

            flLetters.RemoveAllViews();
            var letterAdapter = new LetterAdapter(Activity.BaseContext, 0, currentIteration.Options.Cast<LetterBase>().ToList());
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
        }

        private List<FindMissingLetterOption> GenerateOptions(FindMissingLetterIteration iteration, int numberOfOptions, Case fontCase)
        {
            // Add correct options
            var correctTasks = iteration.TaskItems.Where(l => l.IsSearched);
            var options = correctTasks.Select(task => new FindMissingLetterOption(task.TaskLetter.CorrectLetter.ToCase(fontCase), task.TaskLetter.IsLong, task.TaskLetter.IsLong, iteration.TaskItems.IndexOf(task))).ToList();

            // Add false options
            while (options.Count < numberOfOptions)
            {
                var letter = Alphabet.GetRandomLetter();

                // TODO: This will never return two of the same letters with different isLong or isShort indicators. This is probably needed for some lessons.
                while (options.FirstOrDefault(o => o.Letter.Equals(letter, StringComparison.InvariantCultureIgnoreCase)) != null)
                {
                    letter = Alphabet.GetRandomLetter();
                }

                if (iteration.HasLongAndShortLetters)
                {
                    var random = new Random();
                    options.Add(new FindMissingLetterOption(letter.ToCase(fontCase), random.Next(1) == 0, random.Next(1) == 0, -1));
                }
                else
                {
                    options.Add(new FindMissingLetterOption(letter.ToCase(fontCase), false, false, -1));
                }
            }

            options.Shuffle();
            return options;
        }

        private void ResetCorrectTaskLetter(List<TaskItem> taskItems)
        {
            foreach (var taskItem in taskItems)
            {
                if (taskItem.IsSearched)
                {
                    taskItem.TaskLetter.Letter = "";
                    taskItem.IsDirty = false;
                }
            }
        }

        private void BuildTaskLetters(List<TaskItem> taskItems, Case fontCase)
        {
            llTaskItems.RemoveAllViews();

            // Convert letters to font case
            foreach (var item in taskItems)
            {
                item.TaskLetter.Letter = item.TaskLetter.Letter.ToCase(fontCase);
                item.TaskLetter.CorrectLetter = item.TaskLetter.CorrectLetter.ToCase(fontCase);
            }

            var taskItemAdapter = new TaskItemAdapter(Activity.BaseContext, 0, taskItems);
            for (var i = 0; i < taskItems.Count; i++)
            {
                var view = taskItemAdapter.GetView(i, null, null);

                // Define searched letters as drop zone
                if (taskItems.ElementAt(i).IsSearched)
                    view.Drag += View_Drag;

                // Add letter to view
                llTaskItems.AddView(view);
            }
        }

        private void View_Drag (object sender, View.DragEventArgs e)
        {
            // React on different dragging events
            var evt = e.Event;
            switch (evt.Action)
            {
                case DragAction.Ended:
                case DragAction.Started:
                    e.Handled = true;
                    break;
                // Dragged element has been dropped at the drop zone
                case DragAction.Drop:
                    e.Handled = true;

                    // Try to get clip data
                    var data = e.Event.ClipData;
                    if (data != null)
                    {
                        var taskItems = GetCurrentIteration<FindMissingLetterIteration>().TaskItems;
                        var draggedLetter = data.GetItemAt(0).Text;
                        var position = llTaskItems.IndexOfChild(sender as View);

                        if (taskItems[position].IsSearched)
                        {
                            taskItems[position].TaskLetter.Letter = draggedLetter;
                            taskItems[position].IsDirty = true;
                        }

                        var isReady = taskItems.Count(t => t.IsSearched && !t.IsDirty) == 0;
                        FireUserInteracted(isReady);
                        BuildTaskLetters(taskItems, fontCase);
                    }
                    break;
            }
        }

        public override void CheckSolution()
        {
            var isCorrect = false;
            var currentIteration = GetCurrentIteration<FindMissingLetterIteration>();
            for(var i = 0; i < currentIteration.TaskItems.Count; i++ )
            {
                var taskItem = currentIteration.TaskItems[i];
                if (taskItem.IsSearched)
                {
                    var droppedLetter = currentIteration.Options.FirstOrDefault(o => o.Letter.Equals(taskItem.TaskLetter.Letter));
                    isCorrect = droppedLetter != null && droppedLetter.CorrectPos == i;

                    if (!isCorrect)
                        break;
                }
            }

            FinishIteration(isCorrect);
        }

        public override void OnDestroy()
        {
            base.OnDestroy();

            // Restore iterations loaded
            Lesson.Iterations = iterationsBackup;
        }
    }
}