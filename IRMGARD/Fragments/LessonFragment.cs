
using System;
using System.Linq;

using Android.App;
using IRMGARD.Models;
using Android.Provider;

namespace IRMGARD
{
    public abstract class LessonFragment : Fragment
    {
        /// <summary>
        /// Occurs when iteration finished.
        /// </summary>
        public event IterationFinishedEventHandler IterationFinished;
        public delegate void IterationFinishedEventHandler(object sender, IterationFinishedEventArgs e);

        /// <summary>
        /// Occurs when iteration changed.
        /// </summary>
        public event IterationChangedEventHandler IterationChanged;
        public delegate void IterationChangedEventHandler(object sender, IterationChangedEventArgs e);

        /// <summary>
        /// Occurs when a Lesson has finished all its iterations
        /// </summary>
        public event LessonFinishedEventHandler LessonFinished;
        public delegate void LessonFinishedEventHandler(object sender, EventArgs e);

        /// <summary>
        /// Occurs when a user interacted with the fragment
        /// </summary>
        public event UserInteractedEventHandler UserInteracted;
        public delegate void UserInteractedEventHandler(object sender, UserInteractedEventArgs e);

        protected void FireIterationFinished(Iteration iteration, bool success, bool showAnimation)
        {
            if (IterationFinished != null)
                IterationFinished(this, new IterationFinishedEventArgs(iteration, success, showAnimation));
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

        /// <summary>
        /// Fires the user interacted.
        /// </summary>
        /// <param name="isReady">Indicates if a lesson is ready to be ckeched.</param>
        protected void FireUserInteracted(bool isReady)
        {
            if (UserInteracted != null)
                UserInteracted(this, new UserInteractedEventArgs(isReady));
        }

        public abstract void CheckSolution();
    }

    public abstract class LessonFragment<T> : LessonFragment where T : Lesson
    {
        protected T Lesson;
        private int currentIterationIndex;

        protected LessonFragment(Lesson lesson)
        {
            this.currentIterationIndex = 0;
            this.Lesson = (T)lesson;
        }

        /// <summary>
        /// Gets the current iteration.
        /// </summary>
        /// <returns>The current iteration.</returns>
        /// <typeparam name="TIteration">The type of iteration you are expecting.</typeparam>
        protected TIteration GetCurrentIteration<TIteration>() where TIteration : Iteration
        {
            return (TIteration)Lesson.Iterations.ElementAt(currentIterationIndex);
        }

        /// <summary>
        /// Finishes the iteration and initiates the next one when available or finishes the Lesson.
        /// </summary>
        protected void FinishIteration(bool success)
        {
            FinishIteration(success, true);
        }

        /// <summary>
        /// Finishes the iteration and initiates the next one when available or finishes the Lesson.
        /// </summary>
        protected void FinishIteration(bool success, bool showAnimation)
        {
            if (SoundPlayer.IsPlaying)
                SoundPlayer.Stop();
            
            var iteration = Lesson.Iterations.ElementAt(currentIterationIndex);
            iteration.Status = success ? IterationStatus.Success : IterationStatus.Failed;

            FireIterationFinished(iteration, success, showAnimation);

            if (currentIterationIndex == Lesson.Iterations.Count - 1)
            {
                // All iterations done. Finish Lesson
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
            FireIterationChanged(Lesson.Iterations.ElementAt(currentIterationIndex));
        }

        /// <summary>
        /// Checks if the current iteration's entries are correct
        /// </summary>
        public override abstract void CheckSolution();

    }

    public class IterationChangedEventArgs : EventArgs
    {
        public Iteration Iteration { get; set; }

        public IterationChangedEventArgs(Iteration iteration) : base()
        {
            this.Iteration = iteration;
        }
    }

    public class IterationFinishedEventArgs : EventArgs
    {
        public Iteration Iteration { get; set; }
        public bool Success { get; set; }
        public bool ShowAnimation { get; set; }

        public IterationFinishedEventArgs(Iteration iteration, bool success, bool showAnimation) : base()
        {
            this.Iteration = iteration;
            this.Success = success;
            this.ShowAnimation = showAnimation;
        }
    }

    public class UserInteractedEventArgs : EventArgs
    {
        public bool IsReady { get; set; }

        public UserInteractedEventArgs(bool isReady) : base()
        {
            this.IsReady = isReady;
        }
    }
}