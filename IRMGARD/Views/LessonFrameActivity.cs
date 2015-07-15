
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

namespace IRMGARD
{
	[Activity (Label = "Lesson", ParentActivity = typeof(LevelSelectActivity))]			
	public class LessonFameActivity : Activity
	{
		IMenuItem hintButton;

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
		}

		protected override void OnStart()
		{
			InitLession();
			base.OnStart();
		}

		/// <summary>
		/// Initiates the view with the current lession.
		/// </summary>
		private void InitLession()
		{
			var lesson = DataHolder.Current.CurrentLesson;

			// Set the name of the current lesson as page title
			Title = lesson.Title;

			// Hide hint button, if no hint is available
			if (hintButton != null)
				hintButton.SetVisible(!string.IsNullOrEmpty(lesson.Hint));

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
					var nextLesson = DataHolder.Current.CurrentModule.GetNextLesson(DataHolder.Current.CurrentLesson);
					if (nextLesson != null)
						DataHolder.Current.CurrentLesson = nextLesson;
					InitLession();
					break;
				case Resource.Id.btnPreviousLesson:
					var previousLesson = DataHolder.Current.CurrentModule.GetPrevioustLesson(DataHolder.Current.CurrentLesson);
					if (previousLesson != null)
						DataHolder.Current.CurrentLesson = previousLesson;
					InitLession();
					break;
			}

			return base.OnOptionsItemSelected (item);
		}

		#endregion
	}
}