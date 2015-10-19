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
        private static MediaPlayer player;

        static SoundPlayer()
        {
            player = new MediaPlayer();
            player.Completion += Player_Completion;
        }


		/// <summary>
		/// Plaies a sound from an Assets sound file
		/// </summary>
		/// <param name="context">Context.</param>
		/// <param name="fileName">File name.</param>
		/// <param name = "folderName">Folder Name. Takes "Sounds" if nothing else is set</param>
		public static void PlaySound(Context context, string fileName, string folderName = null)
		{
			// Set default folder "Sounds" if nothing else is set
			if (folderName == null)
				folderName = "Sounds";
			
			try 
			{
				// Describe sound file from Assets properly
				var descriptor = context.Assets.OpenFd(folderName + "/" +  fileName);
						
                // Reset player
                player.Stop();
                player.Release();
                player = new MediaPlayer();
                player.Completion += Player_Completion;

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
		}

        static void Player_Completion (object sender, EventArgs e)
        {
            player.Stop();
            player.Release();
        }
	}
}