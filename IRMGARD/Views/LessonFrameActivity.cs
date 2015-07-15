
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

namespace IRMGARD
{
	[Activity (Label = "Lesson", ParentActivity = typeof(LevelSelectActivity))]			
	public class LessonFameActivity : Activity
	{
		protected override void OnCreate (Bundle bundle)
		{
			base.OnCreate (bundle);
			SetContentView (Resource.Layout.LessonFrame);


			ActionBar.SetLogo (Resource.Drawable.Icon);
			ActionBar.SetDisplayUseLogoEnabled (true);
			ActionBar.SetDisplayShowHomeEnabled(true);
			ActionBar.SetDisplayHomeAsUpEnabled (true);

			InitLession();
		}

		/// <summary>
		/// Initiates the view with the current lession.
		/// </summary>
		private void InitLession()
		{
			var lesson = DataHolder.Current.CurrentLesson;

			// Set the name of the current lesson as page title
			Title = lesson.Title;
		}

		#region UI Operations

		public override bool OnCreateOptionsMenu (IMenu menu)
		{
			MenuInflater.Inflate(Resource.Menu.levelFrame_menu, menu);
			return base.OnCreateOptionsMenu (menu);
		}

		public override bool OnOptionsItemSelected (IMenuItem item)
		{
			switch (item.ItemId) 
			{
				case Resource.Id.btnVoiceInstruction:
					SoundPlayer.PlaySound(this, DataHolder.Current.CurrentLesson.SoundPath);
					break;
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