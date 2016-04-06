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
    [Activity(Label = "Danke", ParentActivity = typeof(LevelSelectActivity), NoHistory = true)]
    public class LevelSponsorActivity : AppCompatActivity
    {
        ImageView ivSplashscreen;
        Bitmap bmpSplashscreen;

        protected override void OnCreate(Bundle bundle)
        {
            base.OnCreate(bundle);
            SetContentView(Resource.Layout.LevelSponsor);
            ivSplashscreen = FindViewById<ImageView>(Resource.Id.ivSplashscreen);
            SetSupportActionBar(FindViewById<Toolbar>(Resource.Id.toolbar));
            SupportActionBar.SetDisplayHomeAsUpEnabled(true);
            RequestedOrientation = Android.Content.PM.ScreenOrientation.Portrait;

            // Set title
            Title = DataHolder.Current.CurrentLevel.Name;
        }

        protected override async void OnResume()
        {
            base.OnResume();

            bmpSplashscreen = BitmapFactory.DecodeResource(Resources, Resource.Drawable.irmgard_danke_01);
            ivSplashscreen.SetImageBitmap(bmpSplashscreen);

            // Show splashscreen for a second
            await Task.Delay(1000);

            if (String.IsNullOrEmpty(DataHolder.Current.CurrentLevel.VideoPath))
            {
                StartActivity(new Intent(this, typeof(ModuleSelectActivity)));
            }
            else
            {
                // Navigate to video player
                var extras = new Bundle();
                extras.PutString("nextView", "ModuleSelectActivity");
                extras.PutString("videoPath", DataHolder.Current.CurrentLevel.VideoPath);
                var intent = new Intent(this, typeof(VideoActivity));
                intent.PutExtras(extras);
                StartActivity(intent);
            }
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

