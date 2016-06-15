using System;

using Android.App;
using Android.Content;
using Android.Graphics;
using Android.OS;
using Android.Views;
using Android.Widget;
using Android.Support.V7.App;
using Toolbar = Android.Support.V7.Widget.Toolbar;
using Android.Support.Design.Widget;

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

            var startButton = FindViewById<FloatingActionButton>(Resource.Id.btnStart);
            startButton.Click += StartButton_Click;
        }

        void NavigateToNextScreen()
        {
            // Navigate to video player
            var extras = new Bundle();
            extras.PutString("nextView", "LevelSelectActivity");
            extras.PutString("videoPath", DataHolder.Current.Common.IntroVideoPath);
            var intent = new Intent(this, typeof(VideoActivity));
            intent.PutExtras(extras);
            StartActivity(intent);
        }

        void StartButton_Click(object sender, EventArgs e)
        {
            NavigateToNextScreen();
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

        public override bool OnCreateOptionsMenu(IMenu menu)
        {
            MenuInflater.Inflate(Resource.Menu.legal_notice_menu, menu);

            return base.OnCreateOptionsMenu(menu);
        }

        public override bool OnOptionsItemSelected(IMenuItem item)
        {
            switch (item.ItemId)
            {
                // Navigate to legal notice
                case Resource.Id.btnLegalNotice:
                    StartActivity(new Intent(this, typeof(LegalNoticeActivity)));
                    break;
            }

            return base.OnOptionsItemSelected(item);
        }
    }
}