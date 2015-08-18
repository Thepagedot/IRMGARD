
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
    public abstract class LessonFragment : Fragment
    {
        /// <summary>
        /// Occurs when a lesson has finished all its iterations
        /// </summary>
        public event FinishedEventHandler Finished;
        public delegate void FinishedEventHandler(object sender, EventArgs e);

        protected void LessonFinished()
        {
            if (Finished != null)
                Finished (this, null);
        }
    }

    public abstract class LessonFragment<T> : LessonFragment
    {
        protected T lesson;
        private int currentIterationIndex;

        protected LessonFragment(Lesson lesson)
        {
            this.currentIterationIndex = 0;

            // Convert lesson to the according sub type
            var obj = (object)lesson;
            this.lesson = (T)obj;
            if (lesson == null)
                throw new NotSupportedException("Wrong lesson type.");
        }

        /// <summary>
        /// Gets the current iteration.
        /// </summary>
        /// <returns>The current iteration.</returns>
        /// <typeparam name="U">The type of iteration you are expecting.</typeparam>
        protected U GetCurrentIteration<U>()
        {
            var obj = (object)((lesson as Lesson).Iterations.ElementAt(currentIterationIndex));
            return (U)obj;
        }

        /// <summary>
        /// Finishes the iteration and initiates the next one when available or finishes the lesson.
        /// </summary>
        protected void FinishIteration()
        {
            if (currentIterationIndex == (lesson as Lesson).Iterations.Count - 1)
            {
                // All iterations done. Finish lesson
                LessonFinished();
            }
            else
            {
                currentIterationIndex++;
                InitIteration();
            }
        }                        

        /// <summary>
        /// Initiates the current iteration
        /// </summary>
        protected abstract void InitIteration();

        /// <summary>
        /// Checks if the current iteration's entries are correct
        /// </summary>
        protected abstract void CheckSolution();
    }
}	