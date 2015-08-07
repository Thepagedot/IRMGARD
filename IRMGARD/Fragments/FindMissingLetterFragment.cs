
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

namespace IRMGARD
{
    public class FindMissingLetterFragment : LessonFragment
    {
        private FindMissingLetter lesson;
        private List<FindMissingLetterIteration> iterations;
        private List<FindMissingLetterOption> currentOptions;
        private int currentIterationIndex;

        private LinearLayout llTaskItems;
        private ListView lvLetters;

        public FindMissingLetterFragment(Lesson lesson)
        {
            // Convert lesson to according sub type
            this.lesson = lesson as FindMissingLetter;
            if (lesson == null)
                throw new NotSupportedException("Wrong lesson type.");

            // Convert iterations
            iterations = new List<FindMissingLetterIteration>();
            foreach (var iteration in lesson.Iterations) 
            {
                if (iteration is FindMissingLetterIteration)
                    iterations.Add(iteration as FindMissingLetterIteration);
            }

            this.currentOptions = new List<FindMissingLetterOption>();
            this.currentIterationIndex = 0;
        }       

        public override View OnCreateView(LayoutInflater inflater, ViewGroup container, Bundle savedInstanceState)
        {
            // Prepare view
            var view = inflater.Inflate(Resource.Layout.FindMissingLetter, container, false);
            llTaskItems = view.FindViewById<LinearLayout>(Resource.Id.llTaskItems);
            lvLetters = view.FindViewById<ListView>(Resource.Id.lvLetters);
            lvLetters.ItemClick += LvLetters_ItemClick;
            lvLetters.ItemLongClick += LvLetters_ItemLongClick;
            var btnCheck = view.FindViewById<ImageButton>(Resource.Id.btnCheck);
            btnCheck.Click += BtnCheck_Click;

            // Initialize iteration
            InitIteration();
            return view;
        }            

        private void InitIteration()
        {
            var currentIteration = iterations.ElementAt(currentIterationIndex);

            // Create lesson
            BuildTaskLetters(currentIteration.TaskLetters);

            var letterAdapter = new FindMissingLetterAdapter(Activity.BaseContext, 0, currentIteration.Options);
            lvLetters.Adapter = letterAdapter;
        }

        void BuildTaskLetters(List<string> letters)
        {
            llTaskItems.RemoveAllViews();
            var taskItemAdapter = new FindMissingLetterTaskItemAdapter(Activity.BaseContext, 0, letters);
            for (int i = 0; i < letters.Count; i++)
            {
                var view = taskItemAdapter.GetView(i, null, null);
                if (letters.ElementAt(i).Equals(""))
                {
                    // Define as drop zone
                    view.Drag += View_Drag;
                }

                // Add letter to view
                llTaskItems.AddView(view);
            }
        }

        void LvLetters_ItemLongClick (object sender, AdapterView.ItemLongClickEventArgs e)
        {
            // Generate clip data package to attach it to the drag
            var data = ClipData.NewPlainText("letter", iterations.ElementAt(currentIterationIndex).Options.ElementAt(e.Position).Letter);

            // Start dragging and pass data
            (e.View).StartDrag(data, new View.DragShadowBuilder(e.View), null, 0);
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

                // Dragged element enters the drop zone
                case DragAction.Entered:                                       
                    break;

                // Dragged element exits the drop zone
                case DragAction.Exited:                                       
                    break;

                // Dragged element has been dropped at the drop zone
                case DragAction.Drop:
                    e.Handled = true;

                    // Try to get clip data
                    var data = e.Event.ClipData;
                    if (data != null)
                    {
                        var letter = data.GetItemAt(0).Text;
                        var list = iterations.ElementAt(currentIterationIndex).TaskLetters;
                        var empty = list.FirstOrDefault(t => t.Equals(""));
                        empty = letter;                           
                        
                                BuildTaskLetters(list);
                        
                        
                    }
                    break;
            }
        }

        void LvLetters_ItemClick (object sender, AdapterView.ItemClickEventArgs e)
        {
            var currentIteration = iterations.ElementAt(currentIterationIndex);
            var selectedOption = currentIteration.Options.ElementAt(e.Position);

            if (selectedOption.CorrectPos >= 0 && currentIteration.TaskLetters.ElementAt(selectedOption.CorrectPos).Equals(""))
            {
                Toast.MakeText (Activity.BaseContext, "Rrrrichtiiig", ToastLength.Short).Show();
                if (currentIterationIndex == iterations.Count - 1) {
                    // All iterations done. Finish lesson
                    LessonFinished ();
                } else {
                    currentIterationIndex++;
                    InitIteration();
                }
            }
        }

        void BtnCheck_Click (object sender, EventArgs e)
        {

        }
    }
}