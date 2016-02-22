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
				// Describe sound file from Assets properly
                var descriptor = context.Assets.OpenFd(folderName ?? DataHolder.Current.Common.AssetSoundDir + "/" +  fileName);

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

                // Reset player if still playing
                Stop();

				// Play sound file
				player.SetDataSource(descriptor.FileDescriptor, descriptor.StartOffset, descriptor.Length);
				player.Prepare();
				player.Start();
			}
			catch (FileNotFoundException)
			{
				Toast.MakeText(context, context.GetString(Resource.String.error_soundfileNotFound), ToastLength.Short);
				System.Console.WriteLine("Error: Soundfile '" + fileName + "' could not be found.");
			}
            catch (Java.Lang.IllegalStateException)
            {
                System.Console.WriteLine("Error: Player is in an invalid state - trying to play '" + fileName + "'");
                player.Stop();
                player.Reset();
            }
		}

        /// <summary>
        /// Stops the currently playing sound.
        /// </summary>
        public static void Stop()
        {
            if (player.IsPlaying)
            {
                player.Stop();
                player.Reset();
            }
        }

        static void Player_Completion (object sender, EventArgs e)
        {
            // Reset player after playing a soundfile
            player.Stop();
            player.Reset();
        }
	}
}