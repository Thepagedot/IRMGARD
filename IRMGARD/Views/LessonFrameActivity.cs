
using System;
using System.Collections.Generic;
using System.Linq;
using Android.App;
using Android.Graphics;
using Android.OS;
using Android.Support.V7.App;
using Android.Support.V7.Widget;
using Android.Text;
using Android.Text.Style;
using Android.Views;
using Android.Widget;
using IRMGARD.Models;
using IRMGARD.Shared;
using AlertDialog = Android.Support.V7.App.AlertDialog;
using Toolbar = Android.Support.V7.Widget.Toolbar;
using Android.Support.Design.Widget;
using Android.Views.Animations;

namespace IRMGARD
{
	[Activity (Label = "Lesson", ParentActivity = typeof(ModuleSelectActivity))]
    public class LessonFameActivity : AppCompatActivity
	{
		private const string lessonFragmentTag = "current-Lesson-fragment";

		//IMenuItem hintButton;
        FloatingActionButton btnNext;
        TextView txtCapitalAlphabet;
        TextView txtLowerAlphabet;
        FrameLayout fragmentContainer;
        RecyclerView rvProgress;
        List<Progress> progressList;
        LessonFragment currentFragment;
        ImageView ivSuccess;

		protected override void OnCreate (Bundle bundle)
		{
			base.OnCreate (bundle);
			SetContentView (Resource.Layout.LessonFrame);
            SetSupportActionBar(FindViewById<Toolbar>(Resource.Id.toolbar));
            SupportActionBar.SetDisplayHomeAsUpEnabled(true);
            progressList = new List<Progress>();
            this.CompatMode();

            btnNext = FindViewById<FloatingActionButton>(Resource.Id.btnNext);
            btnNext.Click += BtnNext_Click;
            rvProgress = FindViewById<RecyclerView>(Resource.Id.rvProgress);
            rvProgress.SetLayoutManager(new LinearLayoutManager(this, LinearLayoutManager.Horizontal, false));
            rvProgress.SetAdapter(new ProgressAdapter(progressList));
			txtCapitalAlphabet = FindViewById<TextView>(Resource.Id.txtCapitalAlphabet);
			txtLowerAlphabet = FindViewById<TextView>(Resource.Id.txtLowerAlphabet);
			fragmentContainer = FindViewById<FrameLayout> (Resource.Id.fragmentContainer);
            ivSuccess = FindViewById<ImageView>(Resource.Id.ivSuccess);

            // Initially hide success image
            ivSuccess.Visibility = ViewStates.Gone;
		}            

		protected override void OnStart()
		{
			InitLesson();
			base.OnStart();
		}

		/// <summary>
		/// Initiates the view with the current lession.
		/// </summary>
		private void InitLesson()
        {
            var module = DataHolder.Current.CurrentModule;
            var lesson = DataHolder.Current.CurrentLesson;
            var iteration = DataHolder.Current.CurrentIteration;

            // ----------------------------------------------------------------------
            // Module specifics
            // ----------------------------------------------------------------------

            // Set playground background color
            fragmentContainer.SetBackgroundColor(Color.ParseColor(module.Color));

            // ----------------------------------------------------------------------
            // Lesson specifics
            // ----------------------------------------------------------------------

            // Set the name of the current Lesson as page title
            Title = lesson.Title;

            // Hide hint button, if no hint is available
            //if (hintButton != null)
            //hintButton.SetVisible(!string.IsNullOrEmpty(Lesson.Hint));

            // Progress
            progressList.Clear();
            for (var i = 0; i < lesson.Iterations.Count; i++)
                progressList.Add(new Progress(lesson.Iterations[i].Status)); // Set iteration's status to the same as progess's staus
            progressList[0].IsCurrent = true;

            rvProgress.GetAdapter().NotifyDataSetChanged();

            // ----------------------------------------------------------------------
            // Load Lesson fragment
            // ----------------------------------------------------------------------

            // Create an instance of the fragment according to the current type of level
            var transaction = FragmentManager.BeginTransaction();
            currentFragment = CreateFragmentForLesson(DataHolder.Current.CurrentLesson);
            if (currentFragment != null)
            {
                // Handle finished events
                currentFragment.LessonFinished += LessonFragment_LessonFinished;
                currentFragment.IterationFinished += Fragment_IterationFinished;
                currentFragment.IterationChanged += Fragment_IterationChanged;
                currentFragment.UserInteracted += CurrentFragment_UserInteracted;

                // Add the fragment to the container
                transaction.Replace(Resource.Id.fragmentContainer, currentFragment, lessonFragmentTag);
                transaction.Commit();
            }
            else
            {
                // Fragment for this type of Lesson could not be loaded. Remove old fragment
                var oldFragment = FragmentManager.FindFragmentByTag(lessonFragmentTag);
                if (oldFragment != null)
                {
                    transaction.Remove(oldFragment);
                    transaction.Commit();
                }
            }
        }

        private void CurrentFragment_UserInteracted (object sender, UserInteractedEventArgs e)
        {
            if (e.IsReady)
            {
                btnNext.Enabled = true;
                btnNext.StartAnimation(AnimationUtils.LoadAnimation(this, Resource.Animation.ShowNextButton));
            }
		}

        void Fragment_IterationChanged(object sender, IterationChangedEventArgs e)
        {
            var iterationIndex = DataHolder.Current.CurrentLesson.Iterations.IndexOf(e.Iteration);

            // Update progress
            progressList.ElementAt(iterationIndex).IsCurrent = true;
            rvProgress.GetAdapter().NotifyItemChanged(iterationIndex);

            // Disable check button
            btnNext.Enabled = false;
            btnNext.StartAnimation(AnimationUtils.LoadAnimation(this, Resource.Animation.HideNextButton));

            // Update iteration number
            SupportActionBar.Subtitle = "Iteration " + (DataHolder.Current.CurrentLesson.Iterations.IndexOf(e.Iteration) + 1) + "/" + DataHolder.Current.CurrentLesson.Iterations.Count;

            // Mark letters in alphabet
            txtCapitalAlphabet.TextFormatted = GetLettersMarked(e.Iteration.LettersToLearn, true);
            txtLowerAlphabet.TextFormatted = GetLettersMarked(e.Iteration.LettersToLearn, false);
        }

        /// <summary>
        /// Handles the Lesson fragement's iteration finished event
        /// </summary>
        /// <param name="sender">Sender.</param>
        /// <param name="e">E.</param>
        void Fragment_IterationFinished(object sender, IterationFinishedEventArgs e)
        {
            // Update status
            var iterationIndex = DataHolder.Current.CurrentLesson.Iterations.IndexOf(e.Iteration);
            progressList.ElementAt(iterationIndex).Status = e.Success ? IterationStatus.Success : IterationStatus.Failed;
            progressList.ElementAt(iterationIndex).IsCurrent = false;
            rvProgress.GetAdapter().NotifyItemChanged(iterationIndex);

            // Show success animation
            if (e.Success)
                ivSuccess.SetImageResource(Resource.Drawable.irmgard_icon_spiel_supergemacht);
            else
                ivSuccess.SetImageResource(Resource.Drawable.irmgard_icon_spiel_nochmal);

            var animation = AnimationUtils.LoadAnimation(this, Resource.Animation.ShowFeedbackIcon);
            animation.AnimationEnd += (s, args) => ivSuccess.Visibility = ViewStates.Gone;
            ivSuccess.Visibility = ViewStates.Visible;
            ivSuccess.StartAnimation(animation);

            // Stop Player
            SoundPlayer.Stop();
        }

		/// <summary>
		/// Handles the Lesson fragment's Lesson finished event
		/// </summary>
		/// <param name="sender">sender.</param>
		/// <param name="e">event args.</param>
        void LessonFragment_LessonFinished(object sender, EventArgs e)
		{
            var builder = new AlertDialog.Builder(this);
            builder.SetTitle(Resource.String.lesson_finished);
            builder.SetMessage(Resource.String.lesson_finished_message);
            builder.SetCancelable(false);
            builder.SetPositiveButton(Android.Resource.String.Ok, (s, args) => NextLesson());
            builder.Show();
		}

		/// <summary>
		/// Returns a new instance of the fragment type according to the type of Lesson
		/// </summary>
		/// <returns>The fragment for the Lesson.</returns>
		/// <param name="lesson">current Lesson.</param>
		private LessonFragment CreateFragmentForLesson(Lesson lesson)
		{
			if (lesson is HearMe)
				return new HearMeFragment(lesson);
			if (lesson is FourPictures)
				return new FourPicturesFragment(lesson);
            if (lesson is PickSyllable)
                return new PickSyllableFragment(lesson);
            if (lesson is BuildSyllable)
                return new BuildSyllableFragment(lesson);
            if (lesson is FindMissingLetter)
                return new FindMissingLetterFragment(lesson);
            if (lesson is AbcRank)
                return new AbcRankFragment(lesson);
            if (lesson is LetterDrop)
                return new LetterDropFragment(lesson);
            if (lesson is Memory)
                return new MemoryFragment(lesson);

            return null;
		}

		#region UI Operations

        private void BtnNext_Click (object sender, EventArgs e)
        {
            currentFragment.CheckSolution();
        }

		public override bool OnCreateOptionsMenu (IMenu menu)
		{
			MenuInflater.Inflate(Resource.Menu.levelFrame_menu, menu);
			//hintButton = menu.FindItem(Resource.Id.btnHint);
			//hintButton.SetVisible(!string.IsNullOrEmpty(DataHolder.Current.CurrentLesson.Hint));

			return base.OnCreateOptionsMenu (menu);
		}

		public override bool OnOptionsItemSelected (IMenuItem item)
		{
			switch (item.ItemId)
			{
				// Play voice instruction
				case Resource.Id.btnVoiceInstruction:
					SoundPlayer.PlaySound(this, DataHolder.Current.CurrentLesson.SoundPath);
					break;
				// Show hint
//				case Resource.Id.btnHint:
//					Toast.MakeText (this, DataHolder.Current.CurrentLesson.Hint, ToastLength.Long).Show();
//					break;
				case Resource.Id.btnNextLesson:
					NextLesson();
					break;
				case Resource.Id.btnPreviousLesson:
					PreviousLesson();
					break;
				case Resource.Id.btnNextModule:
					var nextModule = DataHolder.Current.CurrentLevel.GetNextModule (DataHolder.Current.CurrentModule);
					if (nextModule != null)
					{
						DataHolder.Current.CurrentModule = nextModule;
						DataHolder.Current.CurrentLesson = DataHolder.Current.CurrentModule.Lessons.First();
						DataHolder.Current.CurrentIteration = DataHolder.Current.CurrentLesson.Iterations.First();
						InitLesson ();
					}
					break;
				case Resource.Id.btnPreviousModule:
					var previousModule = DataHolder.Current.CurrentLevel.GetPreviousModule(DataHolder.Current.CurrentModule);
					if (previousModule != null)
					{
						DataHolder.Current.CurrentModule = previousModule;
						DataHolder.Current.CurrentLesson = DataHolder.Current.CurrentModule.Lessons.First();
						DataHolder.Current.CurrentIteration = DataHolder.Current.CurrentLesson.Iterations.First();
						InitLesson ();
					}
				break;
			}

			return base.OnOptionsItemSelected (item);
		}

		#endregion

		/// <summary>
		/// Switches to the next Lesson if available
		/// </summary>
		private void NextLesson()
		{
			var nextLesson = DataHolder.Current.CurrentModule.GetNextLesson(DataHolder.Current.CurrentLesson);
            if (nextLesson != null)
            {
                DataHolder.Current.CurrentLesson = nextLesson;
                DataHolder.Current.CurrentIteration = nextLesson.Iterations.First();
                InitLesson();
            }
            else
            {
                var builder = new AlertDialog.Builder(this);
                builder.SetTitle(Resource.String.module_finished);
                builder.SetMessage(Resource.String.module_finished_message);
                builder.SetCancelable(false);
                builder.SetPositiveButton(Android.Resource.String.Ok, (s, args) => Finish());
                builder.Show();
            }
		}

		/// <summary>
		/// Swtiches to the previous Lesson if available.
		/// </summary>
		private void PreviousLesson()
		{
			var previousLesson = DataHolder.Current.CurrentModule.GetPrevioustLesson (DataHolder.Current.CurrentLesson);
			if (previousLesson != null)
			{
				DataHolder.Current.CurrentLesson = previousLesson;
				DataHolder.Current.CurrentIteration = previousLesson.Iterations.First();
				InitLesson();
			}
		}

        public static SpannableString GetLettersMarked(List<string> markedLetters, bool capitalize)
        {
            var alphabet = "ABCDEFGHIJKLMNOPQRSTUVWXYZ";
            if (capitalize == false)
                alphabet = alphabet.ToLower();

            if (markedLetters == null)
                return new SpannableString(alphabet);

            var spannable = new SpannableString(alphabet);
            foreach (var letter in markedLetters)
            {
                var index = Alphabet.Letters.IndexOf(letter.ToUpper());
                spannable.SetSpan(new ForegroundColorSpan(Android.Graphics.Color.Red), index, index + 1, SpanTypes.ExclusiveExclusive);
            }

            return spannable;
        }
    }
}
