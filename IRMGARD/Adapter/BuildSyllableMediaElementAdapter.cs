using System;
using Android.Widget;
using IRMGARD.Models;
using System.Collections.Generic;
using Android.Content;
using Android.Views;
using Android.Graphics;
using Android.Graphics.Drawables;

namespace IRMGARD
{
    public class BuildSyllableMediaElementAdapter : ArrayAdapter<Syllable>
    {
        private LayoutInflater layoutInflater;
        private Boolean addMultiIcon;
        private List<Syllable> items;

        public BuildSyllableMediaElementAdapter(Context context, int resourceId, List<Syllable> items, Boolean addMultiIcon) : base (context, resourceId, items)
        {
            layoutInflater = LayoutInflater.From(context);
            this.addMultiIcon = addMultiIcon;
            this.items = items;
        }

        public override View GetView(int position, Android.Views.View convertView, Android.Views.ViewGroup parent)
        {
            View view = convertView;
            if (view == null)
                view = layoutInflater.Inflate(Resource.Layout.BuildSyllableMediaElement, null);


            var bitmap = BitmapFactory.DecodeResource(Context.Resources, Resource.Drawable.ic_volume_up_black_24dp);
            var imageView = view.FindViewById<ImageView>(Resource.Id.ivMeidaElementImage);
            ((BitmapDrawable)imageView.Drawable).Bitmap.Recycle();
            imageView.SetImageBitmap (bitmap);

            if (addMultiIcon)
                view.FindViewById<TextView>(Resource.Id.tvAddMultiIcon).Text = "+";
            
            return view;
        }
    }
}

