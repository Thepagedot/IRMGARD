
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
    public class VideoActivity : Activity, MediaPlayer.IOnPreparedListener, ISurfaceHolderCallback
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
            FindViewById<Button>(Resource.Id.btnNext).Click += BtnNext_Click;
            FindViewById<Button>(Resource.Id.btnRepeat).Click += BtnRepeat_Click;
            var videoView = (VideoView)FindViewById<VideoView>(Resource.Id.videoView);

            ISurfaceHolder holder = videoView.Holder;
            holder.SetType (SurfaceType.PushBuffers);
            holder.AddCallback(this);

            var descriptor = Assets.OpenFd("Videos/Handy_Modul_1_720p.mp4");
            mediaPlayer = new MediaPlayer();
            mediaPlayer.SetDataSource(descriptor.FileDescriptor, descriptor.StartOffset, descriptor.Length);
            mediaPlayer.Prepare();
            mediaPlayer.Looping = false;
            //mediaPlayer.Start();

            // Play Video
            Play();
        }
                              
        protected void Play()
        {            
            if (!String.IsNullOrEmpty(DataHolder.Current.CurrentModule.VideoPath))
            {           
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
            Console.WriteLine("SurfaceCreated");
            mediaPlayer.SetDisplay(holder);
        }
        public void SurfaceDestroyed(ISurfaceHolder holder)
        {
            Console.WriteLine("SurfaceDestroyed");
        }
        public void SurfaceChanged(ISurfaceHolder holder, Android.Graphics.Format format, int w, int h)
        {
            Console.WriteLine("SurfaceChanged");
        }
        public void OnPrepared(MediaPlayer player)
        {

        }
    }
}

