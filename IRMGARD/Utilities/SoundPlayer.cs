using System;
using Android.Content;
using Android.Media;
using System.Threading.Tasks;
using Android.Widget;
using Java.IO;

namespace IRMGARD
{
	public static class SoundPlayer
	{
        static readonly MediaPlayer player;

        static bool waitForCompletionActive;

        public static bool WasStopped { get; private set; }

        public static bool IsPlaying
        {
            get
            {
                return player.IsPlaying;
            }
        }

        static SoundPlayer()
        {
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
            PlaySound(context, false, fileName, folderName);
        }

		/// <summary>
		/// Plays a sound from an Assets sound file
		/// </summary>
		/// <param name="context">Context.</param>
        /// <param name="waitForCompletion">Wait for the last audio to finish before starting to play this audio file</param>
		/// <param name="fileName">File name.</param>
		/// <param name = "folderName">Folder Name. Takes "Sounds" if nothing else is set</param>
		public static async void PlaySound(Context context, bool waitForCompletion, string fileName, string folderName = null)
		{
			try
			{
                // Reset stopped info state
                WasStopped = false;

                // Describe sound file from Assets properly
                var descriptor = AssetHelper.Instance.OpenFd(folderName ?? DataHolder.Current.Common.AssetSoundDir + "/" + fileName);

                // Mark as active
                waitForCompletionActive = waitForCompletion;

                // Still playing?
                while (waitForCompletion && IsPlaying)
                {
                    await Task.Delay(500);
                }

                // Continue to play next sound?
                if (waitForCompletion && !waitForCompletionActive)
                {
                    return;
                }

                // Prepare to play sound file
                EndPlayback();
                player.SetDataSource(descriptor.FileDescriptor, descriptor.StartOffset, descriptor.Length);
                player.Prepare();
				player.Start();
			}
			catch (FileNotFoundException)
			{
				Toast.MakeText(context, context.GetString(Resource.String.error_soundfileNotFound), ToastLength.Short);
				System.Console.WriteLine("Error: Soundfile '" + fileName + "' could not be found.");
                player.Reset();
            }
            catch (Java.Lang.IllegalStateException)
            {
                System.Console.WriteLine("Error: Player is in an invalid state - trying to play '" + fileName + "'");
                Stop();
            }
		}

        /// <summary>
        /// Stops the currently playing sound.
        /// </summary>
        public static void Stop()
        {
            waitForCompletionActive = false;
            WasStopped = true;
            EndPlayback();
        }

        static void Player_Completion (object sender, EventArgs e)
        {
            EndPlayback();
        }

        static void EndPlayback()
        {
            // Reset player after playing a soundfile
            if (IsPlaying)
            {
                player.Stop();
            }
            player.Reset();
        }
    }
}