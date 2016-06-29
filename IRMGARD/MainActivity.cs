using System;
using System.IO;
using System.IO.Compression.Zip;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

using Android.App;
using Android.Content;
using Android.Content.PM;
using Android.Graphics;
using Android.OS;
using Android.Views;
using Android.Widget;
using Android.Support.V7.App;
using Toolbar = Android.Support.V7.Widget.Toolbar;
using Android.Provider;

using ExpansionDownloader;
using ExpansionDownloader.Client;
using ExpansionDownloader.Database;
using ExpansionDownloader.Service;
using Android.Support.Design.Widget;
using HockeyApp.Android;
using HockeyApp.Android.Metrics;

namespace IRMGARD
{
    [Activity (Label = "IRMGARD", MainLauncher = true, Icon = "@drawable/icon")]
    public partial class MainActivity : AppCompatActivity
    {
        private ImageView ivSplashscreen;
        private Bitmap bmpSplashscreen;
        private TextView tvLoadingStatus;
        private ProgressBar pbLoadingStatus;
        private FloatingActionButton startButton;
        private MyExtensionDownloaderClient ExtensionDownloaderClient;

        #region Activity Lifecycle Methods

        /// <summary>
        /// Called when the activity is first created; we wouldn't create a
        /// layout in the case where we have the file and are moving to another
        /// activity without downloading.
        /// </summary>
        /// <param name="savedInstanceState">
        /// The saved instance state.
        /// </param>
        protected override async void OnCreate(Bundle savedInstanceState)
        {
            base.OnCreate(savedInstanceState);

            if (Env.Release)
            {
                // Register HockeyApp
                CrashManager.Register(this, "089a6ee65f4242b89c51eab36a4e0ed2");
                MetricsManager.Register(this, Application, "089a6ee65f4242b89c51eab36a4e0ed2");
            }

            // Init start screen
            SetContentView(Resource.Layout.Main);
            RequestedOrientation = Android.Content.PM.ScreenOrientation.Portrait;
            Title = "";
            SetSupportActionBar (this.FindViewById<Toolbar> (Resource.Id.toolbar));
            ivSplashscreen = this.FindViewById<ImageView>(Resource.Id.ivSplashscreen);
            tvLoadingStatus = this.FindViewById<TextView> (Resource.Id.tvLoadingStatus);
            pbLoadingStatus = this.FindViewById<ProgressBar>(Resource.Id.pbLoadingStatus);
            startButton = this.FindViewById<FloatingActionButton>(Resource.Id.btnStart);
            startButton.Click += StartButton_Click;

            // Init extension downloader
            ExtensionDownloaderClient = new MyExtensionDownloaderClient();
            ExtensionDownloaderClient.DownloadProgressChanged += ExtensionDownloaderClient_DownloadProgressChanged;

            if (Env.Release)
            {
                // Before we do anything, are the files we expect already here and
                // delivered (presumably by Market)
                // For free titles, this is probably worth doing. (so no Market
                // request is necessary)
                initText.SetText(Resource.String.text_app_check_obb);
                initText.Visibility = ViewStates.Visible;
                if (!ExtensionDownloaderClient.AreExpansionFilesDelivered() && !ExtensionDownloaderClient.GetExpansionFiles())
                {
                    initText.SetText(Resource.String.text_app_download);
                    initText.Visibility = ViewStates.Visible;
                    ExtensionDownloaderClient.InitializeDownloadUi();
                }
            }
            // download simulation
            //else
            //{
            //    initText.SetText(Resource.String.text_app_check_obb);
            //    initText.Visibility = ViewStates.Visible;
            //    await Task.Delay(4000);
            //    this.InitializeControls();
            //    this.useObbDownloader.Visibility = ViewStates.Visible;

            //    initText.SetText(Resource.String.text_app_download);
            //    initText.Visibility = ViewStates.Visible;

            //    for (int i = 1; i < 10; i++)
            //    {
            //        this.RunOnUiThread(() => ExtensionDownloaderClient.OnDownloadProgress(new DownloadProgressInfo(10000000, i*1000000, i*100, i*500)));
            //        await Task.Delay(1000);
            //    }
            //    this.useObbDownloader.Visibility = ViewStates.Gone;
            //}


            //var extStore = Android.OS.Environment.ExternalStorageDirectory.AbsolutePath;

            await InitApp();
        }

        private async Task InitApp ()
        {
            initText.SetText (Resource.String.text_app_startup);
            initText.Visibility = ViewStates.Visible;

            // Create and initialize asset manager instance for accessing asset files from obbs or apks
            ApplicationInfo ai = ApplicationContext.PackageManager.GetApplicationInfo (ApplicationContext.PackageName, PackageInfoFlags.MetaData);
            AssetHelper.CreateInstance (BaseContext, ai.MetaData.GetInt ("obbMainVersionCode"), ai.MetaData.GetInt ("obbPatchVersionCode"));

            // Initialize DataHolder if needed
            if (DataHolder.Current == null)
            {
                DataHolder.Current = new DataHolder ();
                await DataHolder.Current.LoadCommonAsync ();

                // Load levels from JSON
                await DataHolder.Current.LoadLevelAsync (1);
                await DataHolder.Current.LoadLevelAsync (2);
                await DataHolder.Current.LoadLevelAsync (3);
                await DataHolder.Current.LoadLevelAsync (4);
                await DataHolder.Current.LoadLevelAsync (5);
                await DataHolder.Current.LoadLevelAsync (6);
                await DataHolder.Current.LoadLevelAsync (7);
                await DataHolder.Current.LoadLevelAsync (8);
                await DataHolder.Current.LoadLevelAsync (9);

                //await DataHolder.Current.LoadLevelAsync(-1);

                // Load progress
                await DataHolder.Current.LoadProgressAsync ();
            }

            // Set apps global font
            FontHelper.ReplaceDefaultFont (this, "MONOSPACE", FontHelper.Font.Sen);

            initText.Visibility = ViewStates.Gone;
            startButton.Visibility = ViewStates.Visible;
        }

        protected override void OnResume()
        {
            base.OnResume();

            /// Re-connect the stub to our service on resume.
            if (Env.Release && ExtensionDownloaderClient.DownloaderServiceConnection != null)
            {
                ExtensionDownloaderClient.DownloaderServiceConnection.Connect(this);
            }
        }

        protected override void OnPause()
        {
            base.OnPause();

            //ivSplashscreen.SetImageBitmap(null);
            //bmpSplashscreen.Dispose();
            //bmpSplashscreen = null;
        }

        protected override void OnStop()
        {
            base.OnStop();

            /// Disconnect the stub from our service on stop.
            if (Env.Release && ExtensionDownloaderClient.DownloaderServiceConnection != null)
            {
                ExtensionDownloaderClient.DownloaderServiceConnection.Disconnect(this);
            }
        }

        protected override void OnDestroy()
        {
            base.OnDestroy();

            if (Env.Release && ExtensionDownloaderClient.ZipFileValidationHandler != null)
            {
                ExtensionDownloaderClient.ZipFileValidationHandler.ShouldCancel = true;
            }
        }

        #endregion

        void StartButton_Click (object sender, EventArgs e)
        {
            // Navigate to video player
            var extras = new Bundle ();
            extras.PutString ("nextView", "LevelSelectActivity");
            extras.PutString ("videoPath", DataHolder.Current.Common.IntroVideoPath);
            var intent = new Intent (this, typeof (VideoActivity));
            intent.PutExtras (extras);
            StartActivity (intent);
        }

        void ExtensionDownloaderClient_DownloadProgressChanged (object sender, DownloadProgressInfo e)
        {
            tvLoadingStatus.Text = "Loading";
            pbLoadingStatus.Max = (int)(e.OverallTotal >> 8);
            pbLoadingStatus.Progress = (int)(e.OverallProgress >> 8);
        }

        #region OptionsMenu

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

        #endregion
    }
}