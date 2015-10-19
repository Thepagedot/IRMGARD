
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

namespace IRMGARD
{
    [Activity(Label = "IRMGARD")]            
    public class VideoActivity : AppCompatActivity, MediaPlayer.IOnPreparedListener, ISurfaceHolderCallback
    {
        MediaPlayer mediaPlayer;
        string nextView;

        protected override void OnCreate(Bundle bundle)
        {
            base.OnCreate(bundle);

            // Read context
            nextView = Intent.Extras.GetString("nextView");           

            // Setup UI
            SetContentView(Resource.Layout.Video);
            SetSupportActionBar(FindViewById<Toolbar>(Resource.Id.toolbar));
            SupportActionBar.SetDisplayHomeAsUpEnabled (true);

            FindViewById<Button>(Resource.Id.btnNext).Click += BtnNext_Click;
            FindViewById<Button>(Resource.Id.btnRepeat).Click += BtnRepeat_Click;
            var videoView = (VideoView)FindViewById<VideoView>(Resource.Id.videoView);

            ISurfaceHolder holder = videoView.Holder;
            holder.SetType (SurfaceType.PushBuffers);
            holder.AddCallback(this);

            mediaPlayer = new MediaPlayer();
           
            //mediaPlayer.Start();

            // Play Video
            Play();
        }

        protected override void OnPause()
        {
            base.OnPause();
            mediaPlayer.Stop();
            mediaPlayer.Release();
        }

        public override bool OnOptionsItemSelected(IMenuItem item)
        {
            switch (item.ItemId) 
            {
                // Respond to the action bar's Up/Home button
                case Android.Resource.Id.Home:
                    Finish();
                    return true;
            }

            return base.OnOptionsItemSelected(item);
        }
                           
        protected void Play()
        {            
            if (!String.IsNullOrEmpty(DataHolder.Current.CurrentModule.VideoPath))
            {           
                var descriptor = Assets.OpenFd(DataHolder.Current.CurrentModule.VideoPath);
                mediaPlayer.SetDataSource(descriptor.FileDescriptor, descriptor.StartOffset, descriptor.Length);
                mediaPlayer.Prepare();
                mediaPlayer.Start();
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

        public void SurfaceCreated(ISurfaceHolder holder)
        {
            mediaPlayer.SetDisplay(holder);
        }

        public void SurfaceDestroyed(ISurfaceHolder holder) {}
        public void SurfaceChanged(ISurfaceHolder holder, Android.Graphics.Format format, int w, int h) {}
        public void OnPrepared(MediaPlayer player) {}
    }
}

