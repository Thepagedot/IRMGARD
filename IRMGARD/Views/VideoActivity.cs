
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
using Android.Content.Res;
using Android.Media;

namespace IRMGARD
{
    [Activity(Label = "IRMGARD")]            
    public class VideoActivity : Activity
    {
        VideoView videoView;
        string nextView;

        protected override void OnCreate(Bundle bundle)
        {
            base.OnCreate(bundle);

            // Read context
            nextView = Intent.Extras.GetString("nextView");

            // Set UI
            SetContentView(Resource.Layout.Video);
            videoView = FindViewById<VideoView>(Resource.Id.videoView);
            FindViewById<Button>(Resource.Id.btnNext).Click += BtnNext_Click;
            FindViewById<Button>(Resource.Id.btnRepeat).Click += BtnRepeat_Click;

            // Play Video
            Play();
        }
                              
        protected void Play()
        {            
            if (!String.IsNullOrEmpty(DataHolder.Current.CurrentModule.VideoPath))
            {           
                var uri = Android.Net.Uri.Parse("android.resource://" + PackageName + "/raw/" + Resource.Raw.Handy_Modul_1_720p);
                videoView.SetVideoURI(uri);
                videoView.Start();
            }
        }

        void BtnRepeat_Click (object sender, EventArgs e)
        {
            Play();
        }  

        void BtnNext_Click (object sender, EventArgs e)
        {
            // Navigate to next activity according to the given bundle
            Intent intent = null;
            if (nextView.Equals("LessonFrameActivity"))
                intent = new Intent(this, typeof(LessonFameActivity));
            else if (nextView.Equals("ModuleSelectActivity"))
                intent = new Intent(this, typeof(ModuleSelectActivity));

            if (intent != null)            
                StartActivity(intent);
        }
    }
}

