using System;
using Android.Content;
using Android.Media;
using System.Threading.Tasks;

namespace IRMGARD
{
	public static class SoundPlayer
	{
		/// <summary>
		/// Plaies a sound from an Assets sound file
		/// </summary>
		/// <param name="context">Context.</param>
		/// <param name="fileName">File name.</param>
		public static void PlaySound(Context context, string fileName)
		{
			// Describe sound file from Assets properly
			var descriptor = context.Assets.OpenFd(fileName);

			// Play sound file
			var player = new MediaPlayer();
			player.SetDataSource(descriptor.FileDescriptor, descriptor.StartOffset, descriptor.Length);
			player.Prepare();
			player.Start();
		}

		/// <summary>
		/// Plaies the sound async.
		/// </summary>
		/// <param name="context">Context.</param>
		/// <param name="fileName">File name.</param>
		public static async Task PlaySoundAsync(Context context, string fileName)
		{
			// Describe sound file from Assets properly
			var descriptor = context.Assets.OpenFd(fileName);

			// Play sound file
			var player = new MediaPlayer();
			await player.SetDataSourceAsync(descriptor.FileDescriptor, descriptor.StartOffset, descriptor.Length);
			player.Prepare();
			player.Start();
		}
	}
}

