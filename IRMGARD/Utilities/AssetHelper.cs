using System;
using Android.Graphics;
using Android.Content;
using System.Threading.Tasks;

namespace IRMGARD
{
	public static class AssetHelper
	{
		public static Bitmap GetBitmap(Context context, string fileName)
		{
			try {
				var stream = context.Assets.Open("Images/" + fileName);
				var bitmap = BitmapFactory.DecodeStream(stream);
				return bitmap;
			}
			catch (Java.IO.FileNotFoundException)
			{
				return null;
			}
		}

		public static async Task<Bitmap> GetBitmapAsync(Context context, string fileName)
		{
			var stream = context.Assets.Open(fileName);
			var bitmap = await BitmapFactory.DecodeStreamAsync(stream);
			return bitmap;
		}
	}
}

