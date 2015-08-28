using System;
using IRMGARD.Models;
using Android.Content;
using Android.Widget;
using System.Collections.Generic;
using Android.Views;
using Android.Graphics;

namespace IRMGARD
{
    public class AbcRankAdapter : ArrayAdapter<AbcRankOption>
    {
        private bool checkImage = false;
        public AbcRankAdapter(Context context, int resourceId, List<AbcRankOption> items, bool checkImage) : base (context, resourceId, items)
        {
            this.checkImage = checkImage;
        }

        public override View GetView(int position, Android.Views.View convertView, Android.Views.ViewGroup parent)
        {
            View view = convertView;
            if (view == null)
                view = LayoutInflater.From(Context).Inflate(Resource.Layout.AbcRankTaskItems, null);

            if (checkImage)
            {
                var item = GetItem(position);
                if (item.IsWithImage)
                {
                    var bitmap = AssetHelper.GetBitmap(Context, item.Media.ImagePath);
                    if (bitmap != null)
                        view.FindViewById<ImageView>(Resource.Id.abcRankMeidaElementImage).SetImageBitmap(bitmap);
                }
                else
                {
                    view.FindViewById<TextView>(Resource.Id.abcRankElementName).Text = item.Name;
                }
            }
            else
            {
                view.FindViewById<TextView>(Resource.Id.abcRankElementName).Text =  GetItem(position).Name;
            }

            return view;
        }
    }
}

