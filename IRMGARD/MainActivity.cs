using System;

using Android.App;
using Android.Content;
using Android.Runtime;
using Android.Views;
using Android.Widget;
using Android.OS;
using IRMGARD.Models;
using System.Collections.Generic;
using Newtonsoft.Json;

namespace IRMGARD
{
	[Activity (Label = "IRMGARD", MainLauncher = true, Icon = "@drawable/icon")]
	public class MainActivity : Activity
	{
		protected override async void OnCreate (Bundle bundle)
		{			
			base.OnCreate (bundle);
			SetContentView (Resource.Layout.Main);

			// Initialize DataHolder if needed
			if (DataHolder.Current == null) {
				DataHolder.Current = new DataHolder ();

				// Load first level from JSON
				await DataHolder.Current.LoadLevelAsync (1);
			}

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