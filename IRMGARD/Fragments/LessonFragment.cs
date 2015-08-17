
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
    public abstract class LessonFragment<T> : Fragment
    {
        public delegate void FinishedEventHandler(object sender, EventArgs e);
        public event FinishedEventHandler Finished;

        protected T lesson;
        protected int currentIterationIndex;

        protected LessonFragment(Lesson lesson)
        {
            this.currentIterationIndex = 0;

            // Convert lesson to according sub type
            this.lesson = lesson as T;
            if (lesson == null)
                throw new NotSupportedException("Wrong lesson type.");
        }

        protected T GetCurrentIteration<U>()
        {
            return (U)((lesson as Lesson).Iterations.ElementAt(currentIterationIndex));
        }

        protected void LessonFinished()
        {
            if (Finished != null)
                Finished (this, null);
        }
    }
}	