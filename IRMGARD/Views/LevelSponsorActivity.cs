using System;
using System.Threading.Tasks;

using Android.App;
using Android.Content;
using Android.Graphics;
using Android.OS;
using Android.Widget;
using Android.Support.V7.App;
using Toolbar = Android.Support.V7.Widget.Toolbar;
using Android.Support.Design.Widget;

namespace IRMGARD
{
    [Activity(Label = "Danke", ParentActivity = typeof(LevelSelectActivity), NoHistory = true)]
    public class LevelSponsorActivity : AppCompatActivity
    {
        ImageView ivSplashscreen;
        Bitmap bmpSplashscreen;
        bool navButtonClicked;

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

            var closeButton = FindViewById<FloatingActionButton>(Resource.Id.btnClose);
            closeButton.Click += CloseButton_Click;
        }

        void NavigateToNextScreen()
        {
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

        void CloseButton_Click(object sender, EventArgs e)
        {
            navButtonClicked = true;
            NavigateToNextScreen();
        }

        protected override async void OnResume()
        {
            base.OnResume();

            bmpSplashscreen = BitmapFactory.DecodeResource(Resources, Resource.Drawable.irmgard_danke_01);
            ivSplashscreen.SetImageBitmap(bmpSplashscreen);

            // Show splashscreen for five seconds (three seconds is too short)
            navButtonClicked = false;
            await Task.Delay(5000);

            if (!navButtonClicked)
            {
                NavigateToNextScreen();
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

