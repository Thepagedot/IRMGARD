using System;
using IRMGARD.Models;
using Android.Widget;
using Android.Views;
using Android.Content;
using System.Collections.Generic;
using Android.Graphics;
using Android.Runtime;
using Android.Graphics.Drawables;

namespace IRMGARD
{
    public class PickSyllableAdapter : ArrayAdapter<PickSyllableOption>
    {
        private LayoutInflater layoutInflater;

        public PickSyllableAdapter(Context context, int resourceId, List<PickSyllableOption> items) : base (context, resourceId, items)
        {
            layoutInflater = LayoutInflater.From(context);
        }

        public override View GetView(int position, Android.Views.View convertView, Android.Views.ViewGroup parent)
        {
            View view = convertView;
            if (view == null)
                view = layoutInflater.Inflate(Resource.Layout.MediaElement, null);
            else
                ((BitmapDrawable)view.FindViewById<ImageView>(Resource.Id.ivMeidaElementImage).Drawable).Bitmap.Recycle();

            var bitmap = BitmapFactory.DecodeResource(Context.Resources, Resource.Drawable.ic_volume_up_black_24dp);
            var imageView = view.FindViewById<ImageView>(Resource.Id.ivMeidaElementImage);
            imageView.SetImageBitmap(bitmap);

            return view;
        }
    }
}

