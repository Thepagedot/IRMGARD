
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
					Toast.MakeText (this, "Voice button clicked!", ToastLength.Short).Show();
					break;
				case Resource.Id.btnHint:
					Toast.MakeText (this, "Hint button clicked!", ToastLength.Short).Show();
					break;
			}

			return base.OnOptionsItemSelected (item);
		}
	}
}