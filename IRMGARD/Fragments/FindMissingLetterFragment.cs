
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

            // Initialize iteration
            InitIteration();
            return view;
        }

        private void InitIteration()
        {
            var currentIteration = iterations.ElementAt(currentIterationIndex);

            var taskItemAdapter = new FindMissingLetterTaskItemAdapter(Activity.BaseContext, 0, currentIteration.TaskLetters);
            for (int i = 0; i < currentIteration.TaskLetters.Count; i++)
            {
                var view = taskItemAdapter.GetView(i, null, null);
                llTaskItems.AddView(view);
            }
        }
    }
}

