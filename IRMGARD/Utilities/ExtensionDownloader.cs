using System;
using System.IO.Compression.Zip;
using System.Linq;
using Android.OS;
using ExpansionDownloader;
using ExpansionDownloader.Client;
using ExpansionDownloader.Database;
using ExpansionDownloader.Service;

namespace IRMGARD
{
    public class MyExtensionDownloaderClient : IDownloaderClient
    {
        public event DownloadProgressChangedEventHandler DownloadProgressChanged;
        public delegate void DownloadProgressChangedEventHandler (object sender, DownloadProgressInfo e);
        public void RaiseDownloadProgressChanged (DownloadProgressInfo progress)
        {
            if (DownloadProgressChanged != null)
                DownloadProgressChanged (this, progress);
        }

        /// <summary>
        /// The downloader service.
        /// </summary>
        public IDownloaderService DownloaderService;

        /// <summary>
        /// The downloader service connection.
        /// </summary>
        public IDownloaderServiceConnection DownloaderServiceConnection;

        /// <summary>
        /// The downloader state.
        /// </summary>
        public DownloaderState DownloaderState;

        /// <summary>
        /// The is paused.
        /// </summary>
        public bool IsPaused;

        /// <summary>
        /// The zip file validation handler.
        /// </summary>
        public ZipFileValidationHandler ZipFileValidationHandler;

        /// <summary>
        /// Sets the state of the various controls based on the progressinfo
        /// object sent from the downloader service.
        /// </summary>
        /// <param name="progress">
        /// The progressinfo object sent from the downloader service.
        /// </param>
        public void OnDownloadProgress(DownloadProgressInfo progress)
        {
            RaiseDownloadProgressChanged (progress);
            //this.averageSpeedTextView.Text = string.Format ("{0} Kb/s", Helpers.GetSpeedString (progress.CurrentSpeed));
            //this.timeRemainingTextView.Text = string.Format (
            //    "Verbleibende Zeit: {0}", Helpers.GetTimeRemaining (progress.TimeRemaining));
            //this.progressBar.Max = (int)(progress.OverallTotal >> 8);
            //this.progressBar.Progress = (int)(progress.OverallProgress >> 8);
            //this.progressPercentTextView.Text = string.Format (
            //    "{0}%", progress.OverallProgress * 100 / progress.OverallTotal);
            //this.progressFractionTextView.Text = Helpers.GetDownloadProgressString (
            //    progress.OverallProgress, progress.OverallTotal);
        }

        /// <summary>
        /// The download state should trigger changes in the UI.
        /// It may be useful to show the state as being indeterminate at times.
        /// </summary>
        /// <param name="newState">
        /// The new state.
        /// </param>
        public void OnDownloadStateChanged (DownloaderState newState)
        {
            if (this.DownloaderState != newState) {
                this.DownloaderState = newState;
                //this.statusTextView.Text = this.GetString (Helpers.GetDownloaderStringFromState (newState));
            }

            bool showDashboard = true;
            bool showCellMessage = false;
            bool paused = false;
            bool indeterminate = true;
            switch (newState) {
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
                this.UpdatePauseButton (paused);
            } else {
                this.ValidateExpansionFiles ();
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
        public void OnServiceConnected (Messenger m)
        {
            this.DownloaderService = ServiceMarshaller.CreateProxy (m);
            this.DownloaderService.OnClientUpdated (this.DownloaderServiceConnection.GetMessenger ());
        }


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
        public bool AreExpansionFilesDelivered ()
        {
            var downloads = DownloadsDatabase.GetDownloads ();

            return downloads.Any () && downloads.All (x => Helpers.DoesFileExist (this, x.FileName, x.TotalBytes, false));
        }

        /// <summary>
        /// The do validate zip files.
        /// </summary>
        /// <param name="state">
        /// The state.
        /// </param>
        public void DoValidateZipFiles (object state)
        {
            var downloads = DownloadsDatabase.GetDownloads().Select (x => Helpers.GenerateSaveFileName (this, x.FileName)).ToArray ();

            var result = downloads.Any () && downloads.All (this.IsValidZipFile);

            this.RunOnUiThread (
                delegate {
                    this.pauseButton.Click += async delegate {
                        //Finish();
                        if (this.useObbDownloader != null) {
                            this.useObbDownloader.Visibility = ViewStates.Gone;
                        }
                        await InitApp ();
                    };

                    this.dashboardView.Visibility = ViewStates.Visible;
                    this.useCellDataView.Visibility = ViewStates.Gone;

                    if (result) {
                        this.statusTextView.SetText (Resource.String.text_validation_complete);
                        this.pauseButton.SetText (Android.Resource.String.Ok);
                    } else {
                        this.statusTextView.SetText (Resource.String.text_validation_failed);
                        this.pauseButton.SetText (Android.Resource.String.Cancel);
                    }
                });
        }

        /// <summary>
        /// The get expansion files.
        /// </summary>
        /// <returns>
        /// The get expansion files.
        /// </returns>
        public bool GetExpansionFiles ()
        {
            bool result = false;

            try {
                // Build the intent that launches this activity.
                Intent launchIntent = this.Intent;
                var intent = new Intent (this, typeof (MainActivity));
                intent.SetFlags (ActivityFlags.NewTask | ActivityFlags.ClearTop);
                intent.SetAction (launchIntent.Action);

                if (launchIntent.Categories != null) {
                    foreach (string category in launchIntent.Categories) {
                        intent.AddCategory (category);
                    }
                }

                // Build PendingIntent used to open this activity when user
                // taps the notification.
                PendingIntent pendingIntent = PendingIntent.GetActivity (
                    this, 0, intent, PendingIntentFlags.UpdateCurrent);

                // Request to start the download
                DownloadServiceRequirement startResult = DownloaderService.StartDownloadServiceIfRequired (
                    this, pendingIntent, typeof (MediaDownloadService));

                // The DownloaderService has started downloading the files,
                // show progress otherwise, the download is not needed so  we
                // fall through to starting the actual app.
                if (startResult != DownloadServiceRequirement.NoDownloadRequired) {
                    this.InitializeDownloadUi ();
                    result = true;
                }
            } catch (PackageManager.NameNotFoundException e) {
                e.PrintStackTrace ();
            }

            return result;
        }

        /// <summary>
        /// If the download isn't present, we initialize the download UI. This ties
        /// all of the controls into the remote service calls.
        /// </summary>
        public void InitializeDownloadUi ()
        {
            this.InitializeControls ();
            this.useObbDownloader.Visibility = ViewStates.Visible;

            this.DownloaderServiceConnection = ClientMarshaller.CreateStub (
                this, typeof (MediaDownloadService));
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
        public bool IsValidZipFile (string filename)
        {
            this.ZipFileValidationHandler = new ZipFileValidationHandler (filename) {
                UpdateUi = this.OnUpdateValidationUi
            };

            return File.Exists (filename) && ZipFile.Validate (this.ZipFileValidationHandler);
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
        public void OnPauseButtonClick (object sender, EventArgs e)
        {
            if (this.IsPaused) {
                this.DownloaderService.RequestContinueDownload ();
            } else {
                this.DownloaderService.RequestPauseDownload ();
            }

            this.UpdatePauseButton (!this.IsPaused);
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
        public void OnCellDataResume (object sender, EventArgs args)
        {
            this.StartActivity (new Intent (Settings.ActionWifiSettings));
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
        public void OnOpenWiFiSettingsButtonOnClick (object sender, EventArgs e)
        {
            this.StartActivity (new Intent (Settings.ActionWifiSettings));
        }

        /// <summary>
        /// The on update validation ui.
        /// </summary>
        /// <param name="handler">
        /// The handler.
        /// </param>
        public void OnUpdateValidationUi (ZipFileValidationHandler handler)
        {
            var info = new DownloadProgressInfo (
                handler.TotalBytes, handler.CurrentBytes, handler.TimeRemaining, handler.AverageSpeed);

            this.RunOnUiThread (() => this.OnDownloadProgress (info));
        }

        /// <summary>
        /// Update the pause button.
        /// </summary>
        /// <param name="paused">
        /// Is the download paused.
        /// </param>
        public void UpdatePauseButton (bool paused)
        {
            this.IsPaused = paused;
            int stringResourceId = paused ? Resource.String.text_button_resume : Resource.String.text_button_pause;
            this.pauseButton.SetText (stringResourceId);
        }

        /// <summary>
        /// Perfom a check to see if the expansion files are valid zip files.
        /// </summary>
        public void ValidateExpansionFiles ()
        {
            // Pre execute
            this.dashboardView.Visibility = ViewStates.Visible;
            this.useCellDataView.Visibility = ViewStates.Gone;
            this.statusTextView.SetText (Resource.String.text_verifying_download);
            this.pauseButton.Click += delegate {
                if (this.ZipFileValidationHandler != null) {
                    this.ZipFileValidationHandler.ShouldCancel = true;
                }
            };
            this.pauseButton.SetText (Resource.String.text_button_cancel_verify);

            ThreadPool.QueueUserWorkItem (this.DoValidateZipFiles);
        }
    }
}

