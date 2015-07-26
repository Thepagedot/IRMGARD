﻿
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Android.App;
using Android.Content;
using Android.OS;
using Android.Runtime;
using Android.Views;
using Android.Widget;
using System.Threading.Tasks;
using System.Security.Cryptography;
using IRMGARD.Models;

namespace IRMGARD
{
	[Activity (Label = "Lesson", ParentActivity = typeof(LevelSelectActivity))]			
	public class LessonFameActivity : Activity
	{
		private const string lessonFragmentTag = "current-lesson-fragment";

		IMenuItem hintButton;
		TextView ModuleNumberText;
		TextView LessonNumberText;
		TextView CapitalAlphabetText;
		TextView LowerAlphabetText;
		FrameLayout FragmentContainer;

		protected override void OnCreate (Bundle bundle)
		{
			base.OnCreate (bundle);
			SetContentView (Resource.Layout.LessonFrame);

			// Hide image on Lollypop
			if (Build.VERSION.SdkInt <= BuildVersionCodes.Kitkat)
			{
				ActionBar.SetLogo (Resource.Drawable.Icon);
				ActionBar.SetDisplayUseLogoEnabled (true);
				ActionBar.SetDisplayShowHomeEnabled(true);
			}

			ActionBar.SetDisplayHomeAsUpEnabled (true);

			ModuleNumberText = FindViewById<TextView>(Resource.Id.txtModuleNumber);
			LessonNumberText = FindViewById<TextView>(Resource.Id.txtLessonNumber);
			CapitalAlphabetText = FindViewById<TextView>(Resource.Id.txtCapitalAlphabet);
			LowerAlphabetText = FindViewById<TextView>(Resource.Id.txtLowerAlphabet);
			FragmentContainer = FindViewById<FrameLayout> (Resource.Id.fragmentContainer);
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
			FragmentContainer.SetBackgroundColor(Android.Graphics.Color.ParseColor(module.Color));

			// Prepare module progress overview
			var moduleNumber = DataHolder.Current.CurrentLevel.Modules.IndexOf(DataHolder.Current.CurrentModule) + 1;
			ModuleNumberText.Text = "Module: " + moduleNumber + "/" + DataHolder.Current.CurrentLevel.Modules.Count;

			// ----------------------------------------------------------------------
			// Lesson specifics
			// ----------------------------------------------------------------------

			// Set the name of the current lesson as page title
			Title = lesson.Title;

			// Hide hint button, if no hint is available
			if (hintButton != null)
				hintButton.SetVisible(!string.IsNullOrEmpty(lesson.Hint));

			// Mark letters in alphabet
			CapitalAlphabetText.TextFormatted = Alphabet.GetLettersMarked(iteration.LettersToLearn, true);
			LowerAlphabetText.TextFormatted = Alphabet.GetLettersMarked(iteration.LettersToLearn, false);

			// Prepare lesson progress overview
			var lessonNumber = DataHolder.Current.CurrentModule.Lessons.IndexOf(lesson) + 1;
			LessonNumberText.Text = "Lesson: " + lessonNumber + "/" + DataHolder.Current.CurrentModule.Lessons.Count;

			// ----------------------------------------------------------------------
			// Load lesson fragment
			// ----------------------------------------------------------------------

			// Create an instance of the fragment according to the current type of level
			FragmentTransaction transaction = FragmentManager.BeginTransaction();
			var fragment = CreateFragmentForLesson(DataHolder.Current.CurrentLesson);
			if (fragment != null)
			{
				// Handle finished event
				fragment.Finished += LessonFragment_Finished;

				// Add the fragment to the container
				transaction.Replace(Resource.Id.fragmentContainer, fragment, lessonFragmentTag);
				transaction.Commit();
			} 
			else 
			{
				// Fragment for this type of lesson could not be loaded. Remove old fragment
				var oldFragment = FragmentManager.FindFragmentByTag(lessonFragmentTag);
				if (oldFragment != null) 
				{
					transaction.Remove(oldFragment);
					transaction.Commit();
				}
			}
		}
			
		/// <summary>
		/// Handles the lesson's fragment's finished event
		/// </summary>
		/// <param name="sender">sender.</param>
		/// <param name="e">event args.</param>
		void LessonFragment_Finished(object sender, EventArgs e)
		{
			Toast.MakeText (this, "Lesson finished!", ToastLength.Short).Show();
			NextLesson();
		}

		/// <summary>
		/// Returns a new instance of the fragment type according to the type of lesson
		/// </summary>
		/// <returns>The fragment for the lesson.</returns>
		/// <param name="lesson">current lesson.</param>
		private LessonFragment CreateFragmentForLesson(Lesson lesson)
		{
			if (lesson is HearMe)		
				return new HearMeFragment();
			if (lesson is FourPictures)	
				return new FourPicturesFragment();
			else 						
				return null;
		}

		#region UI Operations

		public override bool OnCreateOptionsMenu (IMenu menu)
		{
			MenuInflater.Inflate(Resource.Menu.levelFrame_menu, menu);
			hintButton = menu.FindItem(Resource.Id.btnHint);
			hintButton.SetVisible(!string.IsNullOrEmpty(DataHolder.Current.CurrentLesson.Hint));

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
				case Resource.Id.btnHint:
					Toast.MakeText (this, DataHolder.Current.CurrentLesson.Hint, ToastLength.Long).Show();
					break;
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
						DataHolder.Current.CurrentLesson = DataHolder.Current.CurrentModule.Lessons.First ();
						DataHolder.Current.CurrentIteration = DataHolder.Current.CurrentLesson.Iterations.First();
						InitLesson ();
					}
					break;
				case Resource.Id.btnPreviousModule:
					var previousModule = DataHolder.Current.CurrentLevel.GetPreviousModule(DataHolder.Current.CurrentModule);
					if (previousModule != null)
					{					
						DataHolder.Current.CurrentModule = previousModule;
						DataHolder.Current.CurrentLesson = DataHolder.Current.CurrentModule.Lessons.First ();
						DataHolder.Current.CurrentIteration = DataHolder.Current.CurrentLesson.Iterations.First();
						InitLesson ();
					}
				break;
			}

			return base.OnOptionsItemSelected (item);
		}

		#endregion

		/// <summary>
		/// Switches to the next lesson if available
		/// </summary>
		private void NextLesson()
		{
			var nextLesson = DataHolder.Current.CurrentModule.GetNextLesson (DataHolder.Current.CurrentLesson);
			if (nextLesson != null) 
			{
				DataHolder.Current.CurrentLesson = nextLesson;
				DataHolder.Current.CurrentIteration = nextLesson.Iterations.First();
				InitLesson();
			}	
		}

		/// <summary>
		/// Swtiches to the previous lesson if available.
		/// </summary>
		private void PreviousLesson()
		{
			var previousLesson = DataHolder.Current.CurrentModule.GetPrevioustLesson (DataHolder.Current.CurrentLesson);
			if (previousLesson != null)
			{					
				DataHolder.Current.CurrentLesson = previousLesson;
				DataHolder.Current.CurrentIteration = previousLesson.Iterations.First();
				InitLesson ();
			}	
		}
	}
}