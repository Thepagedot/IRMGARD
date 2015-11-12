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
        public AbcRankAdapter(Context context, int resourceId, List<AbcRankOption> items) : base (context, resourceId, items)
        {
        }


        public override View GetView(int position, Android.Views.View convertView, Android.Views.ViewGroup parent)
        {
            var item = GetItem(position);
            var view = convertView;
            if (view == null)
                view = LayoutInflater.From(Context).Inflate(Resource.Layout.AbcRankTaskItems, null);
            else
                ((BitmapDrawable)view.FindViewById<ImageView>(Resource.Id.abcRankMeidaElementImage).Drawable).Bitmap.Recycle();


            if (item.Media != null)
            {
                var bitmap = AssetHelper.GetBitmap(Context, item.Media.ImagePath);
                if (bitmap != null)
                    view.FindViewById<ImageView>(Resource.Id.abcRankMeidaElementImage).SetImageBitmap(bitmap);
            }
            else
            {
                view.FindViewById<TextView>(Resource.Id.abcRankElementName).Text = item.Name;
            }

            return view;
        }
    }
}

