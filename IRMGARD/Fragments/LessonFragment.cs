
using System;
using System.Linq;

using Android.App;
using IRMGARD.Models;

namespace IRMGARD
{	
    public abstract class LessonFragment : Fragment
    {
        /// <summary>
        /// Occurs when iteration finished.
        /// </summary>
        public event IterationFinishedEventHandler IterationFinished;
        public delegate void IterationFinishedEventHandler(object sender, EventArgs e);

        /// <summary>
        /// Occurs when iteration changed.
        /// </summary>
        public event IterationChangedEventHandler IterationChanged;
        public delegate void IterationChangedEventHandler(object sender, IterationChangedEventArgs e);

        /// <summary>
        /// Occurs when a lesson has finished all its iterations
        /// </summary>
        public event LessonFinishedEventHandler LessonFinished;
        public delegate void LessonFinishedEventHandler(object sender, EventArgs e);

        protected void FireIterationFinished()
        {
            if (IterationFinished != null)
                IterationFinished(this, null);
        }

        protected void FireIterationChanged(Iteration iteration)
        {
            if (IterationChanged != null)
                IterationChanged(this, new IterationChangedEventArgs(iteration));
        }

        protected void FireLessonFinished()
        {
            if (LessonFinished != null)
                LessonFinished(this, null);
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
            FireIterationFinished();

            if (currentIterationIndex == (lesson as Lesson).Iterations.Count - 1)
            {
                // All iterations done. Finish lesson
                FireLessonFinished();
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
        protected virtual void InitIteration()
        {
            // Fire iteration changed event
            FireIterationChanged((lesson as Lesson).Iterations.ElementAt(currentIterationIndex));
        }

        /// <summary>
        /// Checks if the current iteration's entries are correct
        /// </summary>
        protected abstract void CheckSolution();
    }

    public class IterationChangedEventArgs : EventArgs
    {
        public Iteration Iteration { get; set; }

        public IterationChangedEventArgs(Iteration iteration) : base()
        {
            this.Iteration = iteration;
        }
    }
}	