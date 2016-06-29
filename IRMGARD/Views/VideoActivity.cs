
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
using Android.Support.V7.App;
using Toolbar = Android.Support.V7.Widget.Toolbar;
using Android.Support.Design.Widget;
using Android.Graphics;

namespace IRMGARD
{
    [Activity(Label = "IRMGARD", NoHistory = true)]
    public class VideoActivity : AppCompatActivity, MediaPlayer.IOnPreparedListener, ISurfaceHolderCallback
    {
        MediaPlayer mediaPlayer;
        VideoView videoView;
        string nextView;
        string videoPath;

        protected override void OnCreate(Bundle bundle)
        {
            base.OnCreate(bundle);
            RequestedOrientation = Android.Content.PM.ScreenOrientation.Portrait;
            SetContentView(Resource.Layout.Video);
            this.SetSystemBarBackground (Color.Black);

            // Read context
            nextView = Intent.Extras.GetString("nextView");
            videoPath = Intent.Extras.GetString("videoPath");

            FindViewById<FloatingActionButton>(Resource.Id.btnNext).Click += BtnNext_Click;
            FindViewById<FloatingActionButton>(Resource.Id.btnRepeat).Click += BtnRepeat_Click;
            videoView = FindViewById<VideoView>(Resource.Id.videoView);

            var holder = videoView.Holder;
            holder.AddCallback(this);
        }

        protected override void OnResume()
        {
            base.OnResume();
            mediaPlayer = new MediaPlayer();
        }

        public override void OnWindowFocusChanged(bool hasFocus)
        {
            base.OnWindowFocusChanged(hasFocus);

            if (hasFocus)
            {
                // Adjust video size to 9:16
                var layoutParams = videoView.LayoutParameters;
                layoutParams.Width = Convert.ToInt32(videoView.MeasuredHeight * 0.5625);
                videoView.LayoutParameters = layoutParams;

                // Play Video
                Play();
            }
        }

        protected override void OnPause()
        {
            base.OnPause();
            mediaPlayer.Stop();
            mediaPlayer.Release();
        }

        protected void Play()
        {
            if (!String.IsNullOrEmpty(videoPath) && !mediaPlayer.IsPlaying)
            {
                var descriptor = AssetHelper.Instance.OpenFd(videoPath);
                mediaPlayer.SetDataSource(descriptor.FileDescriptor, descriptor.StartOffset, descriptor.Length);
                mediaPlayer.Prepare();
                mediaPlayer.Start();
                mediaPlayer.Completion += MediaPlayer_Completion;
            }
        }

        private void MediaPlayer_Completion(object sender, EventArgs e)
        {
            NavigateNext();
        }

        void BtnNext_Click (object sender, EventArgs e)
        {
            NavigateNext();
        }

        private void NavigateNext()
        {
            // Navigate to next activity according to the given bundle
            Intent intent = null;
            if (nextView.Equals("LessonFrameActivity"))
                intent = new Intent(this, typeof(LessonFameActivity));
            else if (nextView.Equals("LevelSelectActivity"))
                intent = new Intent(this, typeof(LevelSelectActivity));
            else if (nextView.Equals("ModuleSelectActivity"))
                intent = new Intent(this, typeof(ModuleSelectActivity));

            if (intent != null)
            {
                mediaPlayer.Stop();
                StartActivity(intent);
            }
        }

        void BtnRepeat_Click (object sender, EventArgs e)
        {
            mediaPlayer.Stop();
            mediaPlayer.Prepare();
            mediaPlayer.Start();
        }

        public void SurfaceCreated(ISurfaceHolder holder)
        {
            mediaPlayer.SetDisplay(holder);
        }

        public void SurfaceDestroyed(ISurfaceHolder holder) {}
        public void SurfaceChanged(ISurfaceHolder holder, Android.Graphics.Format format, int w, int h) {}
        public void OnPrepared(MediaPlayer player) {}
    }
}