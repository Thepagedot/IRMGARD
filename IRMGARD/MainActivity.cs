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
using Android.Support.V7.App;
using Toolbar = Android.Support.V7.Widget.Toolbar;
using Android.Support.Design.Widget;
using Android.Graphics;

namespace IRMGARD
{
	[Activity (Label = "IRMGARD", MainLauncher = true, Icon = "@drawable/icon")]
	public class MainActivity : AppCompatActivity
	{
        ImageView ivSplashscreen;
        Bitmap bmpSplashscreen;

		protected override async void OnCreate (Bundle bundle)
		{
			base.OnCreate (bundle);
            FontHelper.ReplaceDefaultFont(this, "SERIF", FontHelper.Font.Sen);
            FontHelper.ReplaceDefaultFont(this, "MONOSPACE", FontHelper.Font.Sen);
			SetContentView (Resource.Layout.Main);
            ivSplashscreen = FindViewById<ImageView>(Resource.Id.ivSplashscreen);
            SetSupportActionBar(FindViewById<Toolbar>(Resource.Id.toolbar));
            RequestedOrientation = Android.Content.PM.ScreenOrientation.Portrait;

			// Initialize DataHolder if needed
			if (DataHolder.Current == null)
            {
				DataHolder.Current = new DataHolder();
                await DataHolder.Current.LoadCommonAsync();

                // Load levels from JSON
                await DataHolder.Current.LoadLevelAsync(1);
                await DataHolder.Current.LoadLevelAsync(2);
                await DataHolder.Current.LoadLevelAsync(3);
                await DataHolder.Current.LoadLevelAsync(4);
                await DataHolder.Current.LoadLevelAsync(5);
                await DataHolder.Current.LoadLevelAsync(6);
                await DataHolder.Current.LoadLevelAsync(7);
                await DataHolder.Current.LoadLevelAsync(8);
                await DataHolder.Current.LoadLevelAsync(9);

                //await DataHolder.Current.LoadLevelAsync(-1);

                // Load progress
                await DataHolder.Current.LoadProgressAsync();
			}

			var startButton = FindViewById<FloatingActionButton> (Resource.Id.btnStart);
			startButton.Click += StartButton_Click;
		}

        protected override void OnResume()
        {
            base.OnResume();

            bmpSplashscreen = BitmapFactory.DecodeResource(Resources, Resource.Drawable.splashscreen);
            ivSplashscreen.SetImageBitmap(bmpSplashscreen);
        }

        protected override void OnPause()
        {
            base.OnPause();

            ivSplashscreen.SetImageBitmap(null);
            bmpSplashscreen.Dispose();
            bmpSplashscreen = null;
        }

        void StartButton_Click (object sender, EventArgs e)
		{
            var intent = new Intent (this, typeof(LevelSelectActivity));
			StartActivity(intent);
		}
	}
}