﻿
using System;

using Android.App;
using Android.Content;
using Android.OS;
using Android.Views;
using Android.Widget;
using Android.Media;
using Android.Support.V7.App;
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
            ActivityHelper.ApplyLevelColors(Theme);
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
            Play();
        }

        protected override void OnPause()
        {
            base.OnPause();

            ReleasePlayer();
        }

        protected override void OnStop()
        {
            base.OnStop();

            // ReleasePlayer(); already done in OnPause()
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

        void ReleasePlayer()
        {
            try
            {
                if (mediaPlayer != null)
                {
                    mediaPlayer.Stop();
                }
            }
            catch (Java.Lang.IllegalStateException)
            {
                // Yes - this might happen...
            }
            finally
            {
                if (mediaPlayer != null)
                {
                    mediaPlayer.Reset();
                    mediaPlayer.Release();
                    mediaPlayer = null;
                }
            }
        }

        protected void Play()
        {
            try
            {
                if (mediaPlayer != null && !String.IsNullOrEmpty(videoPath) && !mediaPlayer.IsPlaying)
                {
                    var descriptor = AssetHelper.Instance.OpenFd(videoPath);
                    mediaPlayer.SetDataSource(descriptor.FileDescriptor, descriptor.StartOffset, descriptor.Length);
                    mediaPlayer.Prepare();
                    mediaPlayer.Start();
                    mediaPlayer.Completion += MediaPlayer_Completion;
                }
            }
            catch (Java.Lang.IllegalStateException)
            {
                // Yes - this might happen...
            }
        }

        private void MediaPlayer_Completion(object sender, EventArgs e)
        {
            if (mediaPlayer != null)
            {
                mediaPlayer.Completion -= MediaPlayer_Completion;
            }
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
                StartActivity(intent);
            }
        }

        void BtnRepeat_Click (object sender, EventArgs e)
        {
            RenavigateToVideoPlayer();
        }

        private void RenavigateToVideoPlayer()
        {
            // Re-navigate to video player
            var bundle = new Bundle();
            bundle.PutString("nextView", nextView);
            bundle.PutString("videoPath", videoPath);

            var intent = new Intent(this, typeof(VideoActivity));
            intent.PutExtras(bundle);

            Finish();
            StartActivity(intent);
        }

        public void SurfaceCreated(ISurfaceHolder holder)
        {
            if (mediaPlayer != null)
            {
                try
                {
                    mediaPlayer.SetDisplay(holder);
                }
                catch (Java.Lang.IllegalStateException)
                {
                    ReleasePlayer();
                    mediaPlayer = new MediaPlayer();
                    mediaPlayer.SetDisplay(holder);
                }
            }
        }

        public void SurfaceDestroyed(ISurfaceHolder holder) {}
        public void SurfaceChanged(ISurfaceHolder holder, Android.Graphics.Format format, int w, int h) {}
        public void OnPrepared(MediaPlayer player) {}
    }
}