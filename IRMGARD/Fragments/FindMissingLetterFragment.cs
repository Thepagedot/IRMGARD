
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
            var btnCheck = view.FindViewById<ImageButton>(Resource.Id.btnCheck);
            btnCheck.Click += BtnCheck_Click;;

            // Initialize iteration
            InitIteration();
            return view;
        }            

        private void InitIteration()
        {
            var currentIteration = iterations.ElementAt(currentIterationIndex);

            // Create lesson
            var taskItemAdapter = new FindMissingLetterTaskItemAdapter(Activity.BaseContext, 0, currentIteration.TaskLetters);
            for (int i = 0; i < currentIteration.TaskLetters.Count; i++)
            {
                var view = taskItemAdapter.GetView(i, null, null);
                llTaskItems.AddView(view);
            }

            var letterAdapter = new FindMissingLetterAdapter(Activity.BaseContext, 0, currentIteration.Options);
            lvLetters.Adapter = letterAdapter;
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