using System;
using System.Collections.Generic;

using Android.Content;
using Android.Media;
using Android.Widget;

using Java.IO;

namespace IRMGARD
{
	public static class SoundPlayer
	{
        static MediaPlayer player;

        public static bool IsPlaying
        {
            get
            {
                try
                {
                    return (player != null) ? player.IsPlaying : false;
                }
                catch (Java.Lang.IllegalStateException)
                {
                    HockeyApp.MetricsManager.TrackEvent("Error: SoundPlayer is in an invalid state checking playing status.");
                    InitPlayer();
                    return false;
                }
            }
        }

        static SoundPlayer()
        {
            InitPlayer();
        }

        static void InitPlayer()
        {
            if (player != null)
            {
                player.Release();
            }
            player = new MediaPlayer();
            player.Completion += Player_Completion;
        }

        /// <summary>
        /// Plays a sound from an Assets sound file
        /// </summary>
        /// <param name="context">Context.</param>
        /// <param name="fileName">File name.</param>
        /// <param name = "folderName">Folder Name. Takes "Sounds" if nothing else is set</param>
        public static void PlaySound(Context context, string fileName, string folderName = null)
		{
            if (player == null || String.IsNullOrEmpty(fileName)) {
                return;
            }

			try
			{
                // Describe sound file from Assets properly
                var descriptor = AssetHelper.Instance.OpenFd(folderName ?? DataHolder.Current.Common.AssetSoundDir + "/" + fileName);

                // Prepare to play sound file
                FinalizePlayback();
                player.SetDataSource(descriptor.FileDescriptor, descriptor.StartOffset, descriptor.Length);
                player.Prepare();
				player.Start();
			}
			catch (FileNotFoundException)
			{
				Toast.MakeText(context, context.GetString(Resource.String.error_soundfileNotFound), ToastLength.Short);
                HockeyApp.MetricsManager.TrackEvent("Error: Soundfile '" + fileName + "' could not be found.");
                player.Reset();
            }
            catch (Java.Lang.IllegalStateException)
            {
                HockeyApp.MetricsManager.TrackEvent("Error: SoundPlayer is in an invalid state - trying to play '" + fileName + "'");
                InitPlayer();
            }
        }

        /// <summary>
        /// Stops the currently playing sound.
        /// </summary>
        public static void Stop()
        {
            FinalizePlayback();
        }

        static void Player_Completion (object sender, EventArgs e)
        {
            FinalizePlayback();
        }

        static void FinalizePlayback()
        {
            if (player != null)
            {
                try
                {
                    // Reset player after playing a soundfile
                    if (IsPlaying)
                    {
                        player.Stop();
                    }
                    player.Reset();
                    RemoveAllEvents();
                }
                catch (Java.Lang.IllegalStateException)
                {
                    HockeyApp.MetricsManager.TrackEvent("Error: SoundPlayer is in an invalid state on finalizing playback.");
                    InitPlayer();
                }
            }
        }

        #region Completion Event

        static List<EventHandler> delegates = new List<EventHandler>();

        public static event EventHandler Completion
        {
            add
            {
                player.Completion += value;
                delegates.Add(value);
            }

            remove
            {
                player.Completion -= value;
                delegates.Remove(value);
            }
        }

        public static void RemoveAllEvents()
        {
            if (player != null)
            {
                foreach (EventHandler eh in delegates)
                {
                    player.Completion -= eh;
                }
                delegates.Clear();
            }
        }

        #endregion
    }
}