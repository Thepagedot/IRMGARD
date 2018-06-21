using System;
using System.IO;
using System.Threading.Tasks;

using Android.App;
using Android.Content;
using Android.Content.PM;
using Android.OS;
using Android.Views;
using Android.Widget;
using Android.Support.Design.Widget;
using Android.Support.V7.App;
using Toolbar = Android.Support.V7.Widget.Toolbar;

using HockeyApp.Android;
using HockeyApp.Android.Metrics;

namespace IRMGARD
{
    [Activity (Label = "IRMGARD", MainLauncher = true, Icon = "@drawable/icon")]
    public partial class MainActivity : AppCompatActivity
    {
        const string TAG = "MainActivity";

        private TextView initText;
        private FloatingActionButton startButton;

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

        private async Task InitApp()
        {
            if (useObbDownloader != null)
            {
                useObbDownloader.Visibility = ViewStates.Gone;
            }
            initText.SetText(Resource.String.text_app_startup);
            initText.Visibility = ViewStates.Visible;

            // Create and initialize asset manager instance for accessing asset files from obbs or apks
            ApplicationInfo ai = ApplicationContext.PackageManager.GetApplicationInfo(ApplicationContext.PackageName, PackageInfoFlags.MetaData);
            AssetHelper.CreateInstance(BaseContext, ai.MetaData.GetInt("obbMainVersionCode"), ai.MetaData.GetInt("obbPatchVersionCode"));
            if (!AssetHelper.Instance.IsValid())
            {
                if (Env.UseOBB)
                {
                    if (this.GetExpansionFiles()) {
                        return;
                    }
                }
            }

            // Initialize DataHolder if needed
            if (DataHolder.Current == null)
            {
                DataHolder.Current = new DataHolder();
                DataHolder.Current.LoadCommon();

                // Load levels from JSON
                DataHolder.Current.LoadLevel(1);
                DataHolder.Current.LoadLevel(2);
                DataHolder.Current.LoadLevel(3);
                DataHolder.Current.LoadLevel(4);
                DataHolder.Current.LoadLevel(5);
                DataHolder.Current.LoadLevel(6);
                DataHolder.Current.LoadLevel(7);
                DataHolder.Current.LoadLevel(8);
                DataHolder.Current.LoadLevel(9);

                //await DataHolder.Current.LoadLevelAsync(-1);

                // Load progress
                await DataHolder.Current.LoadProgressAsync();
            }

            // Set apps global font
            FontHelper.ReplaceDefaultFont(this, "MONOSPACE", FontHelper.Font.StandardRegular);

            initText.Visibility = ViewStates.Gone;
            startButton.Visibility = ViewStates.Visible;
        }

        private async Task CreateApp()
        {
            if (Env.UseOBB)
            {
                // Before we do anything, are the files we expect already here and
                // delivered (presumably by Market)
                // For free titles, this is probably worth doing. (so no Market
                // request is necessary)
                initText.SetText(Resource.String.text_app_check_obb);
                initText.Visibility = ViewStates.Visible;

                if (AreExpansionFilesDelivered())
                {
                    await InitApp();
                }
                else
                {
                    initText.Visibility = ViewStates.Gone;
                    this.GetExpansionFiles();
                }
            }
            else
            {
                //await SimulateDownload();
                await InitApp();
            }
        }

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
                MetricsManager.Register(Application, "089a6ee65f4242b89c51eab36a4e0ed2");
            }

            RequestedOrientation = Android.Content.PM.ScreenOrientation.Portrait;

            // Init start screen
            SetContentView(Resource.Layout.Main);
            Title = "";
            SetSupportActionBar(this.FindViewById<Toolbar>(Resource.Id.toolbar));

            initText = this.FindViewById<TextView>(Resource.Id.initText);
            startButton = this.FindViewById<FloatingActionButton>(Resource.Id.btnStart);
            startButton.Click += StartButton_Click;

            await TryCreateAppAsync();
        }

        protected override void OnResume()
        {
            base.OnResume();

            DownloaderOnResume();
        }

        protected override void OnStop()
        {
            base.OnStop();

            DownloaderOnStop();
        }

        protected override void OnDestroy()
        {
            base.OnDestroy();

            DownloaderOnDestroy();
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