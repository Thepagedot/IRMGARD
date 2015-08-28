using System;
using IRMGARD.Models;
using Android.Views;
using Android.OS;
using System.Collections.Generic;
using System.Linq;
using Android.Widget;
using Android.Content;
using Android.Graphics;

namespace IRMGARD
{
    public class AbcRankSolutionElementAdapter : ArrayAdapter<AbcRankOption>
    {
        private List<AbcRankOption> items;
        private bool checkImage = false;

        public AbcRankSolutionElementAdapter(Context context, int resourceId, List<AbcRankOption> items, bool checkImage) : base (context, resourceId, items)
        {
            this.items = items;
            this.checkImage = checkImage;
        }

        public override View GetView(int position, Android.Views.View convertView, Android.Views.ViewGroup parent)
        {
            View view = convertView;
            if (view == null)
                view = LayoutInflater.From(Context).Inflate(Resource.Layout.AbcRankSolutionItems, null);

            if (checkImage)
            {
                Bitmap bitmap = null;
                var item = items.ElementAt(position);

                if (item.IsWithImage)
                {
                    bitmap = AssetHelper.GetBitmap(Context, item.Media.ImagePath);
                }
                else
                {
                    bitmap = BitmapFactory.DecodeResource(Context.Resources, Resource.Drawable.ic_help_black_24dp);
                }

                view.FindViewById<ImageView>(Resource.Id.ivAbcRankSolutionItem).SetImageBitmap(bitmap);
            }
            else
            {
                view.FindViewById<TextView>(Resource.Id.abcRankSolutionElementName).Text = items.ElementAt(position).Name;
            }


            return view;
        }
    }
}

