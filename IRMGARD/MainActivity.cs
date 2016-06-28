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
    public partial class MainActivity : AppCompatActivity, IDownloaderClient
    {
        #region Constants and Fields

        /// <summary>
        /// The background image.
        /// </summary>
        private ImageView ivSplashscreen;
        private Bitmap bmpSplashscreen;

        private TextView initText;
        private FloatingActionButton startButton;

        /// <summary>
        /// The downloader service.
        /// </summary>
        private IDownloaderService downloaderService;

        /// <summary>
        /// The downloader service connection.
        /// </summary>
        private IDownloaderServiceConnection downloaderServiceConnection;

        /// <summary>
        /// The downloader state.
        /// </summary>
        private DownloaderState downloaderState;

        /// <summary>
        /// The is paused.
        /// </summary>
        private bool isPaused;

        /// <summary>
        /// The zip file validation handler.
        /// </summary>
        private ZipFileValidationHandler zipFileValidationHandler;

        #endregion

        #region Activity Lifecycle Methods

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
            initText.SetText(Resource.String.text_app_startup);
            initText.Visibility = ViewStates.Visible;

            // Create and initialize asset manager instance for accessing asset files from obbs or apks
            ApplicationInfo ai = ApplicationContext.PackageManager.GetApplicationInfo(ApplicationContext.PackageName, PackageInfoFlags.MetaData);
            AssetHelper.CreateInstance(BaseContext, ai.MetaData.GetInt("obbMainVersionCode"), ai.MetaData.GetInt("obbPatchVersionCode"));

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

            // Set apps global font
            FontHelper.ReplaceDefaultFont(this, "MONOSPACE", FontHelper.Font.Sen);

            initText.Visibility = ViewStates.Gone;
            startButton.Visibility = ViewStates.Visible;
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
                MetricsManager.Register(this, Application, "089a6ee65f4242b89c51eab36a4e0ed2");
            }

            RequestedOrientation = Android.Content.PM.ScreenOrientation.Portrait;

            // Init start screen
            SetContentView(Resource.Layout.Main);
            Title = "";
            SetSupportActionBar(this.FindViewById<Toolbar>(Resource.Id.toolbar));

            ivSplashscreen = this.FindViewById<ImageView>(Resource.Id.ivSplashscreen);
            initText = this.FindViewById<TextView>(Resource.Id.initText);
            startButton = this.FindViewById<FloatingActionButton>(Resource.Id.btnStart);
            startButton.Click += StartButton_Click;

            if (Env.Release)
            {
                // Before we do anything, are the files we expect already here and
                // delivered (presumably by Market)
                // For free titles, this is probably worth doing. (so no Market
                // request is necessary)
                initText.SetText(Resource.String.text_app_check_obb);
                initText.Visibility = ViewStates.Visible;
                if (!this.AreExpansionFilesDelivered() && !this.GetExpansionFiles())
                {
                    initText.SetText(Resource.String.text_app_download);
                    initText.Visibility = ViewStates.Visible;
                    this.InitializeDownloadUi();
                }
            }
            /* download simulation
            else
            {
                initText.SetText(Resource.String.text_app_check_obb);
                initText.Visibility = ViewStates.Visible;
                await Task.Delay(4000);
                this.InitializeControls();
                this.useObbDownloader.Visibility = ViewStates.Visible;

                initText.SetText(Resource.String.text_app_download);
                initText.Visibility = ViewStates.Visible;

                for (int i = 1; i < 10; i++)
                {
                    this.RunOnUiThread(() => this.OnDownloadProgress(new DownloadProgressInfo(10000000, i*1000000, i*100, i*500)));
                    await Task.Delay(1000);
                }
                this.useObbDownloader.Visibility = ViewStates.Gone;
            }
            */

            //var extStore = Android.OS.Environment.ExternalStorageDirectory.AbsolutePath;

            await InitApp();
        }

        protected override void OnResume()
        {
            base.OnResume();

            /// Re-connect the stub to our service on resume.
            if (Env.Release && this.downloaderServiceConnection != null)
            {
                this.downloaderServiceConnection.Connect(this);
            }
        }

        protected override void OnPause()
        {
            base.OnPause();

            ivSplashscreen.SetImageBitmap(null);
            bmpSplashscreen.Dispose();
            bmpSplashscreen = null;
        }

        protected override void OnStop()
        {
            base.OnStop();

            /// Disconnect the stub from our service on stop.
            if (Env.Release && this.downloaderServiceConnection != null)
            {
                this.downloaderServiceConnection.Disconnect(this);
            }
        }

        protected override void OnDestroy()
        {
            base.OnDestroy();

            if (Env.Release && this.zipFileValidationHandler != null)
            {
                this.zipFileValidationHandler.ShouldCancel = true;
            }
        }

        #endregion

        #region IDownloaderClient Methods

        /// <summary>
        /// Sets the state of the various controls based on the progressinfo
        /// object sent from the downloader service.
        /// </summary>
        /// <param name="progress">
        /// The progressinfo object sent from the downloader service.
        /// </param>
        public void OnDownloadProgress(DownloadProgressInfo progress)
        {
            this.averageSpeedTextView.Text = string.Format("{0} Kb/s", Helpers.GetSpeedString(progress.CurrentSpeed));
            this.timeRemainingTextView.Text = string.Format(
                "Verbleibende Zeit: {0}", Helpers.GetTimeRemaining(progress.TimeRemaining));
            this.progressBar.Max = (int)(progress.OverallTotal >> 8);
            this.progressBar.Progress = (int)(progress.OverallProgress >> 8);
            this.progressPercentTextView.Text = string.Format(
                "{0}%", progress.OverallProgress * 100 / progress.OverallTotal);
            this.progressFractionTextView.Text = Helpers.GetDownloadProgressString(
                progress.OverallProgress, progress.OverallTotal);
        }

        /// <summary>
        /// The download state should trigger changes in the UI.
        /// It may be useful to show the state as being indeterminate at times.
        /// </summary>
        /// <param name="newState">
        /// The new state.
        /// </param>
        public void OnDownloadStateChanged(DownloaderState newState)
        {
            if (this.downloaderState != newState)
            {
                this.downloaderState = newState;
                this.statusTextView.Text = this.GetString(Helpers.GetDownloaderStringFromState(newState));
            }

            bool showDashboard = true;
            bool showCellMessage = false;
            bool paused = false;
            bool indeterminate = true;
            switch (newState)
            {
                case DownloaderState.Idle:
                case DownloaderState.Connecting:
                case DownloaderState.FetchingUrl:
                    break;
                case DownloaderState.Downloading:
                    indeterminate = false;
                    break;
                case DownloaderState.Failed:
                case DownloaderState.FailedCanceled:
                case DownloaderState.FailedFetchingUrl:
                case DownloaderState.FailedUnlicensed:
                    paused = true;
                    showDashboard = false;
                    indeterminate = false;
                    break;
                case DownloaderState.PausedNeedCellularPermission:
                case DownloaderState.PausedWifiDisabledNeedCellularPermission:
                    showDashboard = false;
                    paused = true;
                    indeterminate = false;
                    showCellMessage = true;
                    break;
                case DownloaderState.PausedByRequest:
                    paused = true;
                    indeterminate = false;
                    break;
                case DownloaderState.PausedRoaming:
                case DownloaderState.PausedSdCardUnavailable:
                    paused = true;
                    indeterminate = false;
                    break;
                default:
                    paused = true;
                    break;
            }

            if (newState != DownloaderState.Completed)
            {
                this.dashboardView.Visibility = showDashboard ? ViewStates.Visible : ViewStates.Gone;
                this.useCellDataView.Visibility = showCellMessage ? ViewStates.Visible : ViewStates.Gone;
                this.progressBar.Indeterminate = indeterminate;
                this.UpdatePauseButton(paused);
            }
            else
            {
                this.ValidateExpansionFiles();
            }
        }

        /// <summary>
        /// Create the remote service and marshaler.
        /// </summary>
        /// <remarks>
        /// This is how we pass the client information back to the service so
        /// the client can be properly notified of changes.
        /// Do this every time we reconnect to the service.
        /// </remarks>
        /// <param name="m">
        /// The messenger to use.
        /// </param>
        public void OnServiceConnected(Messenger m)
        {
            this.downloaderService = ServiceMarshaller.CreateProxy(m);
            this.downloaderService.OnClientUpdated(this.downloaderServiceConnection.GetMessenger());
        }

        #endregion

        #region Expansion Files Download Methods

        /// <summary>
        /// Go through each of the Expansion APK files defined in the project
        /// and determine if the files are present and match the required size.
        /// </summary>
        /// <remarks>
        /// Free applications should definitely consider doing this, as this
        /// allows the application to be launched for the first time without
        /// having a network connection present.
        /// Paid applications that use LVL should probably do at least one LVL
        /// check that requires the network to be present, so this is not as
        /// necessary.
        /// </remarks>
        /// <returns>
        /// True if they are present, otherwise False;
        /// </returns>
        private bool AreExpansionFilesDelivered()
        {
            var downloads = DownloadsDatabase.GetDownloads();

            return downloads.Any() && downloads.All(x => Helpers.DoesFileExist(this, x.FileName, x.TotalBytes, false));
        }

        /// <summary>
        /// The do validate zip files.
        /// </summary>
        /// <param name="state">
        /// The state.
        /// </param>
        private void DoValidateZipFiles(object state)
        {
            var downloads = DownloadsDatabase.GetDownloads().Select(x => Helpers.GenerateSaveFileName(this, x.FileName)).ToArray();

            var result = downloads.Any() && downloads.All(this.IsValidZipFile);

            this.RunOnUiThread(
                delegate
                {
                    this.pauseButton.Click += async delegate
                    {
                        //Finish();
                        if (this.useObbDownloader != null)
                        {
                            this.useObbDownloader.Visibility = ViewStates.Gone;
                        }
                        await InitApp();
                    };

                    this.dashboardView.Visibility = ViewStates.Visible;
                    this.useCellDataView.Visibility = ViewStates.Gone;

                    if (result)
                    {
                        this.statusTextView.SetText(Resource.String.text_validation_complete);
                        this.pauseButton.SetText(Android.Resource.String.Ok);
                    }
                    else
                    {
                        this.statusTextView.SetText(Resource.String.text_validation_failed);
                        this.pauseButton.SetText(Android.Resource.String.Cancel);
                    }
                });
        }

        /// <summary>
        /// The get expansion files.
        /// </summary>
        /// <returns>
        /// The get expansion files.
        /// </returns>
        private bool GetExpansionFiles()
        {
            bool result = false;

            try
            {
                // Build the intent that launches this activity.
                Intent launchIntent = this.Intent;
                var intent = new Intent(this, typeof(MainActivity));
                intent.SetFlags(ActivityFlags.NewTask | ActivityFlags.ClearTop);
                intent.SetAction(launchIntent.Action);

                if (launchIntent.Categories != null)
                {
                    foreach (string category in launchIntent.Categories)
                    {
                        intent.AddCategory(category);
                    }
                }

                // Build PendingIntent used to open this activity when user
                // taps the notification.
                PendingIntent pendingIntent = PendingIntent.GetActivity(
                    this, 0, intent, PendingIntentFlags.UpdateCurrent);

                // Request to start the download
                DownloadServiceRequirement startResult = DownloaderService.StartDownloadServiceIfRequired(
                    this, pendingIntent, typeof(MediaDownloadService));

                // The DownloaderService has started downloading the files,
                // show progress otherwise, the download is not needed so  we
                // fall through to starting the actual app.
                if (startResult != DownloadServiceRequirement.NoDownloadRequired)
                {
                    this.InitializeDownloadUi();
                    result = true;
                }
            }
            catch (PackageManager.NameNotFoundException e)
            {
                e.PrintStackTrace();
            }

            return result;
        }

        /// <summary>
        /// If the download isn't present, we initialize the download UI. This ties
        /// all of the controls into the remote service calls.
        /// </summary>
        private void InitializeDownloadUi()
        {
            this.InitializeControls();
            this.useObbDownloader.Visibility = ViewStates.Visible;

            this.downloaderServiceConnection = ClientMarshaller.CreateStub(
                this, typeof(MediaDownloadService));
        }

        /// <summary>
        /// The is valid zip file.
        /// </summary>
        /// <param name="filename">
        /// The filename.
        /// </param>
        /// <returns>
        /// The is valid zip file.
        /// </returns>
        private bool IsValidZipFile(string filename)
        {
            this.zipFileValidationHandler = new ZipFileValidationHandler(filename)
            {
                UpdateUi = this.OnUpdateValidationUi
            };

            return File.Exists(filename) && ZipFile.Validate(this.zipFileValidationHandler);
        }

        /// <summary>
        /// The on pause button click.
        /// </summary>
        /// <param name="sender">
        /// The sender.
        /// </param>
        /// <param name="e">
        /// The e.
        /// </param>
        private void OnPauseButtonClick(object sender, EventArgs e)
        {
            if (this.isPaused)
            {
                this.downloaderService.RequestContinueDownload();
            }
            else
            {
                this.downloaderService.RequestPauseDownload();
            }

            this.UpdatePauseButton(!this.isPaused);
        }

        /// <summary>
        /// The on cell data resume event handler.
        /// </summary>
        /// <param name="sender">
        /// The sender.
        /// </param>
        /// <param name="args">
        /// The args.
        /// </param>
        private void OnCellDataResume(object sender, EventArgs args)
        {
            this.StartActivity(new Intent(Settings.ActionWifiSettings));
            //this.downloaderService.SetDownloadFlags(ServiceFlags.FlagsDownloadOverCellular);
            //this.downloaderService.RequestContinueDownload();
            //this.useCellDataView.Visibility = ViewStates.Gone;
        }

        /// <summary>
        /// The on open wi fi settings button on click.
        /// </summary>
        /// <param name="sender">
        /// The sender.
        /// </param>
        /// <param name="e">
        /// The e.
        /// </param>
        private void OnOpenWiFiSettingsButtonOnClick(object sender, EventArgs e)
        {
            this.StartActivity(new Intent(Settings.ActionWifiSettings));
        }

        /// <summary>
        /// The on update validation ui.
        /// </summary>
        /// <param name="handler">
        /// The handler.
        /// </param>
        private void OnUpdateValidationUi(ZipFileValidationHandler handler)
        {
            var info = new DownloadProgressInfo(
                handler.TotalBytes, handler.CurrentBytes, handler.TimeRemaining, handler.AverageSpeed);

            this.RunOnUiThread(() => this.OnDownloadProgress(info));
        }

        /// <summary>
        /// Update the pause button.
        /// </summary>
        /// <param name="paused">
        /// Is the download paused.
        /// </param>
        private void UpdatePauseButton(bool paused)
        {
            this.isPaused = paused;
            int stringResourceId = paused ? Resource.String.text_button_resume : Resource.String.text_button_pause;
            this.pauseButton.SetText(stringResourceId);
        }

        /// <summary>
        /// Perfom a check to see if the expansion files are valid zip files.
        /// </summary>
        private void ValidateExpansionFiles()
        {
            // Pre execute
            this.dashboardView.Visibility = ViewStates.Visible;
            this.useCellDataView.Visibility = ViewStates.Gone;
            this.statusTextView.SetText(Resource.String.text_verifying_download);
            this.pauseButton.Click += delegate
            {
                if (this.zipFileValidationHandler != null)
                {
                    this.zipFileValidationHandler.ShouldCancel = true;
                }
            };
            this.pauseButton.SetText(Resource.String.text_button_cancel_verify);

            ThreadPool.QueueUserWorkItem(this.DoValidateZipFiles);
        }

        #endregion

        #region OptionsMenu

        public override bool OnCreateOptionsMenu(IMenu menu)
        {
            MenuInflater.Inflate(Resource.Menu.legal_notice_contact_menu, menu);

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