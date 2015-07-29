using System;
using IRMGARD.Models;
using Android.Widget;
using Android.Views;
using Android.Content;
using System.Collections.Generic;

namespace IRMGARD
{
	public class FourPicturesAdapter : ArrayAdapter<FourPicturesOption>
	{
		private LayoutInflater layoutInflater;

		public FourPicturesAdapter (Context context, int resourceId, List<FourPicturesOption> items) : base (context, resourceId, items)
		{
			layoutInflater = LayoutInflater.From (context);
		}

		public override View GetView(int position, Android.Views.View convertView, Android.Views.ViewGroup parent)
		{
			View view = convertView;
			if (view == null)
				view = layoutInflater.Inflate(Resource.Layout.MediaElement, null);

			var bitmap = AssetHelper.GetBitmap(Context, GetItem(position).Media.ImagePath);

			view.FindViewById<ImageView> (Resource.Id.ivMeidaElementImage).SetImageBitmap (bitmap);
			return view;
		}
	}
}

