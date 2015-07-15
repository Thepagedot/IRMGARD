
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

//			ActionBar.SetLogo (Resource.Drawable.Icon);
//			ActionBar.SetDisplayUseLogoEnabled (true);
//			ActionBar.SetDisplayShowHomeEnabled(true);
			ActionBar.SetDisplayHomeAsUpEnabled (true);

			// Set the name of the current lesson as page title
			Title = DataHolder.Current.CurrentLesson.Title;
		}

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
					NextLesson();
					break;
				case Resource.Id.btnPreviousLesson:
					PreviousLesson();
					break;
			}

			return base.OnOptionsItemSelected (item);
		}

		private void NextLesson()
		{
			var index = DataHolder.Current.CurrentModule.LessonsList.IndexOf(DataHolder.Current.CurrentLesson);
			if (DataHolder.Current.CurrentModule.LessonsList.Count - 1 < index)
			{
				// Next lesson is available
				DataHolder.Current.CurrentLesson = DataHolder.Current.CurrentModule.LessonsList.ElementAt(index + 1);
			}
		}
	}
}