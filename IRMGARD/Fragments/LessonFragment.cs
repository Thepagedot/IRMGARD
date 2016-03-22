
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
        public delegate void LessonFinishedEventHandler(object sender, LessonFinishedEventArgs e);

        /// <summary>
        /// Occurs when a user interacted with the fragment
        /// </summary>
        public event UserInteractedEventHandler UserInteracted;
        public delegate void UserInteractedEventHandler(object sender, UserInteractedEventArgs e);

        /// <summary>
        /// Occurs when a refresh of progress list items is required
        /// </summary>
        public event ProgressListRefreshRequestedEventHandler ProgressListRefreshRequested;
        public delegate void ProgressListRefreshRequestedEventHandler(object sender, ProgressListRefreshRequestedEventArgs e);

        protected void FireIterationFinished(Iteration iteration, bool success, bool provideFeedback)
        {
            if (IterationFinished != null)
                IterationFinished(this, new IterationFinishedEventArgs(iteration, success, provideFeedback));
        }

        protected void FireIterationChanged(Iteration iteration)
        {
            if (IterationChanged != null)
                IterationChanged(this, new IterationChangedEventArgs(iteration));
        }

        protected void FireLessonFinished(bool provideFeedback)
        {
            if (LessonFinished != null)
                LessonFinished(this, new LessonFinishedEventArgs(provideFeedback));
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

        protected void FireProgressListRefreshRequested(Lesson lesson)
        {
            if (ProgressListRefreshRequested != null)
                ProgressListRefreshRequested(this, new ProgressListRefreshRequestedEventArgs(lesson));
        }

        public abstract void CheckSolution();
    }

    public abstract class LessonFragment<T> : LessonFragment where T : Lesson
    {
        protected T Lesson;
        private int currentIterationIndex;

        protected LessonFragment()
        {
            this.currentIterationIndex = 0;
            this.Lesson = (T)(DataHolder.Current.CurrentLesson);
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
        protected void FinishIteration(bool success, bool provideFeedback)
        {
            if (SoundPlayer.IsPlaying)
                SoundPlayer.Stop();

            var iteration = Lesson.Iterations.ElementAt(currentIterationIndex);
            iteration.Status = success ? IterationStatus.Success : IterationStatus.Failed;

            FireIterationFinished(iteration, success, provideFeedback);

            if (currentIterationIndex == Lesson.Iterations.Count - 1)
            {
                // All iterations done. Finish Lesson
                FireLessonFinished(provideFeedback);
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

    public class LessonFinishedEventArgs : EventArgs
    {
        public bool ProvideFeedback { get; set; }

        public LessonFinishedEventArgs(bool provideFeedback) : base()
        {
            this.ProvideFeedback = provideFeedback;
        }
    }

    public class IterationFinishedEventArgs : EventArgs
    {
        public Iteration Iteration { get; set; }
        public bool Success { get; set; }
        public bool ProvideFeedback { get; set; }

        public IterationFinishedEventArgs(Iteration iteration, bool success, bool provideFeedback) : base()
        {
            this.Iteration = iteration;
            this.Success = success;
            this.ProvideFeedback = provideFeedback;
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

    public class ProgressListRefreshRequestedEventArgs : EventArgs
    {
        public Lesson Lesson { get; set; }

        public ProgressListRefreshRequestedEventArgs(Lesson lesson) : base()
        {
            this.Lesson = lesson;
        }
    }
}