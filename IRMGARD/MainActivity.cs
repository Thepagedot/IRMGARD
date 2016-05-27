using System;
using System.Threading.Tasks;

using Android.App;
using Android.Content;
using Android.Graphics;
using Android.OS;
using Android.Widget;
using Android.Support.V7.App;
using Toolbar = Android.Support.V7.Widget.Toolbar;

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
        }

        public override void OnBackPressed() {
            // Disable back button navigation on first screen
        }

        protected override async void OnResume()
        {
            base.OnResume();

            bmpSplashscreen = BitmapFactory.DecodeResource(Resources, Resource.Drawable.splashscreen);
            ivSplashscreen.SetImageBitmap(bmpSplashscreen);

            // Show splashscreen for two seconds
            await Task.Delay(Env.Debug ? 100 : 2000);

            // Navigate to video player
            var extras = new Bundle();
            extras.PutString("nextView", "LevelSelectActivity");
            extras.PutString("videoPath", DataHolder.Current.Common.IntroVideoPath);
            var intent = new Intent(this, typeof(VideoActivity));
            intent.PutExtras(extras);
            StartActivity(intent);
        }

        protected override void OnPause()
        {
            base.OnPause();

            ivSplashscreen.SetImageBitmap(null);
            bmpSplashscreen.Dispose();
            bmpSplashscreen = null;
        }
    }
}