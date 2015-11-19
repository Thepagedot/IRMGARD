using System;
using IRMGARD.Models;
using Android.Content;
using Android.Widget;
using System.Collections.Generic;
using Android.Views;
using Android.Graphics;
using Android.Graphics.Drawables;

namespace IRMGARD
{
    public class AbcRankAdapter : ArrayAdapter<AbcRankOption>
    {
        public AbcRankAdapter(Context context, int resourceId, IList<AbcRankOption> items) : base (context, resourceId, items)
        {
        }


        public override View GetView(int position, View convertView, ViewGroup parent)
        {
            var item = GetItem(position);
            var view = convertView;
            if (view == null)
                view = LayoutInflater.From(Context).Inflate(Resource.Layout.AbcRankOptionItem, null);
            else
                ((BitmapDrawable)view.FindViewById<ImageView>(Resource.Id.image).Drawable).Bitmap.Recycle();


            if (item.Media != null)
            {
                view.FindViewById<TextView>(Resource.Id.letter).Visibility = ViewStates.Gone;
                var bitmap = AssetHelper.GetBitmap(Context, item.Media.ImagePath);
                if (bitmap != null)
                    view.FindViewById<ImageView>(Resource.Id.image).SetImageBitmap(bitmap);
            }
            else
            {
                view.FindViewById<TextView>(Resource.Id.letter).Text = item.Name;
            }

            return view;
        }
    }
}

