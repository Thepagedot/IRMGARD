using System;

using Android.App;
using Android.Content;
using Android.Runtime;
using Android.Views;
using Android.Widget;
using Android.OS;

namespace IRMGARD
{
	[Activity (Label = "IRMGARD", MainLauncher = true, Icon = "@drawable/icon")]
	public class MainActivity : Activity
	{
		protected override void OnCreate (Bundle bundle)
		{
			// Initialize DataHolder if needed
			if (DataHolder.Current == null)
				DataHolder.Current = new DataHolder ();
			
			base.OnCreate (bundle);
			SetContentView (Resource.Layout.Main);

			ActionBar.SetLogo (Resource.Drawable.Icon);
			ActionBar.SetDisplayUseLogoEnabled (true);
			ActionBar.SetDisplayShowHomeEnabled(true);

			var startButton = FindViewById<ImageButton> (Resource.Id.btnStart);
			startButton.Click += StartButton_Click;
		}

		void StartButton_Click (object sender, EventArgs e)
		{
			var intent = new Intent (this, typeof(LevelSelectActivity));
			StartActivity (intent);
		}
	}
}