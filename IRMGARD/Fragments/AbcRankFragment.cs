using System;
using IRMGARD.Models;
using Android.Views;
using Android.OS;
using System.Collections.Generic;
using System.Linq;
using Android.Widget;
using Android.Content;
using IRMGARD.Shared;

namespace IRMGARD
{
    public class AbcRankFragment : LessonFragment<AbcRank>
    {
        private FlowLayout flOptions;
        private LinearLayout llTaskItems;

        public override View OnCreateView (LayoutInflater inflater, ViewGroup container, Bundle savedInstanceState)
        {
            View view = inflater.Inflate(Resource.Layout.AbcRank, container, false);
            flOptions = view.FindViewById<FlowLayout> (Resource.Id.flOptions);
            llTaskItems = view.FindViewById<LinearLayout>(Resource.Id.llTaskItems);

            InitIteration();
            return view;
        }

        #region implemented abstract members of LessonFragment

        protected override void InitIteration()
        {
            base.InitIteration();

            var currentIteration = GetCurrentIteration<AbcRankIteration>();

            // Genrate Task items
            currentIteration.TaskItems.Clear();
            foreach (var option in currentIteration.Options)
            {
                currentIteration.TaskItems.Add(null);
            }

            BuildTaskElements(currentIteration.TaskItems);
            BuildOptions(currentIteration.Options);
        }

        private void BuildTaskElements(List<TaskItem> taskItems)
        {
            // Add task items to view and attach Drag and Drop handler
            llTaskItems.RemoveAllViews();
            var adapter = new TaskItemAdapter(Activity.BaseContext, 0, taskItems);
            for (var i = 0; i < taskItems.Count; i++)
            {
                var view = adapter.GetView(i, null, null);
                if (GetCurrentIteration<AbcRankIteration>().Options.FirstOrDefault().Media != null)
                {
                    // Workaround to resize drop zone space for picture cards
                    view.FindViewById<TextView>(Resource.Id.letter).SetTextSize(Android.Util.ComplexUnitType.Dip, 30);
                }
                view.Drag += View_Drag;
                llTaskItems.AddView(view);
            }
        }

        private void BuildOptions(List<AbcRankOption> currentOptions)
        {
            // Shuffle options
            currentOptions.Shuffle();

            // Add options to view
            flOptions.RemoveAllViews();
            var adapter = new AbcRankAdapter(Activity.BaseContext, 0, currentOptions);
            for (var i = 0; i < currentOptions.Count; i++)
            {
                var view = adapter.GetView(i, null, null);
                var item = currentOptions.ElementAt(i);

                // Play sound if item has a media element that gets touched
                if (item.Media != null)
                {
                    view.Touch += (sender, e) => {
                        SoundPlayer.PlaySound(Activity.BaseContext, item.Media.SoundPath);
                    };
                }

                // Add drag
                view.Touch += (sender, e) => {
                    var data = ClipData.NewPlainText("letter", item.Name);
                    (sender as View).StartDrag(data, new View.DragShadowBuilder(sender as View), null, 0);
                };

                flOptions.AddView(view);
            }
        }

        void View_Drag (object sender, View.DragEventArgs e)
        {
            // React on different dragging events
            var evt = e.Event;
            switch (evt.Action)
            {
                case DragAction.Started:
                    e.Handled = true;
                    break;
                case DragAction.Drop:
                    e.Handled = true;

                    // Try to get clip data
                    var data = e.Event.ClipData;
                    if (data != null)
                    {
                        var taskLetters = GetCurrentIteration<AbcRankIteration>().TaskItems;
                        var position = llTaskItems.IndexOfChild(sender as View);
                        var draggedLetter = data.GetItemAt(0).Text;
                        var draggedItem = GetCurrentIteration<AbcRankIteration>().Options.FirstOrDefault(o => o.Name.Equals(draggedLetter));

                        taskLetters[position] = new TaskItem(new TaskLetter(draggedItem.Name), draggedItem.Media, true);
                        taskLetters[position].IsDirty = true;

                        var isReady = taskLetters.All(l => l != null ? l.IsDirty : false);
                        FireUserInteracted(isReady);
                        BuildTaskElements(taskLetters);
                    }

                    break;
            }
        }            

        public override void CheckSolution()
        {            
            var isCorrect = true;
            var taskLetters = GetCurrentIteration<AbcRankIteration>().TaskItems;

            for (var i = 1; i < taskLetters.Count; i++)
            {
                if (String.IsNullOrEmpty(taskLetters[i].TaskLetter.Letter))
                {
                    isCorrect = false;
                    break;
                }

                var previousPos = Alphabet.Letters.IndexOf(taskLetters[i - 1].TaskLetter.Letter.ToUpper()[0].ToString());
                var thisPos = Alphabet.Letters.IndexOf(taskLetters[i].TaskLetter.Letter.ToUpper()[0].ToString());

                if (previousPos >= thisPos)
                {
                    isCorrect = false;
                    break;
                }
            }
                
            FinishIteration(isCorrect);
        }

        #endregion
    }
}

