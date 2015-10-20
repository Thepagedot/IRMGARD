using System;
using IRMGARD.Models;
using Android.Views;
using Android.OS;
using System.Collections.Generic;
using System.Linq;
using Android.Widget;
using Android.Content;
using Android.Graphics;
using Android.Graphics.Drawables;

namespace IRMGARD
{
    public class AbcRankSolutionElementAdapter : ArrayAdapter<AbcRankOption>
    {
        private List<AbcRankOption> items;

        public AbcRankSolutionElementAdapter(Context context, int resourceId, List<AbcRankOption> items) : base (context, resourceId, items)
        {
            this.items = items;
        }

        public override View GetView(int position, Android.Views.View convertView, Android.Views.ViewGroup parent)
        {
            View view = convertView;
            if (view == null)
                view = LayoutInflater.From(Context).Inflate(Resource.Layout.AbcRankSolutionItems, null);
            else
                ((BitmapDrawable)view.FindViewById<ImageView>(Resource.Id.ivAbcRankSolutionItem).Drawable).Bitmap.Recycle();
            

            var item = items.ElementAt(position);

            if (item.Name == null)
            {
                // if there is no item as solution show the placeholder question mark without text
                prepareUiElementsForImage(BitmapFactory.DecodeResource(Context.Resources, Resource.Drawable.ic_help_black_24dp), view.FindViewById<ImageView>(Resource.Id.ivAbcRankSolutionItem), view.FindViewById<TextView>(Resource.Id.abcRankSolutionElementName));
            }
            else
            {
                // if there is an item at this position of the solution
                if (item.Media != null)
                {
                    // if there is an item with media element
                    prepareUiElementsForImage(AssetHelper.GetBitmap(Context, item.Media.ImagePath), view.FindViewById<ImageView>(Resource.Id.ivAbcRankSolutionItem), view.FindViewById<TextView>(Resource.Id.abcRankSolutionElementName));
                }
                else
                {
                    // if there is an item with image and contains only text
                    view.FindViewById<TextView>(Resource.Id.abcRankSolutionElementName).Text = items.ElementAt(position).Name;
                    view.FindViewById<TextView>(Resource.Id.abcRankSolutionElementName).TextSize = 40.0f;
                }
            }

            return view;
        }

        private void prepareUiElementsForImage(Bitmap bitmap, ImageView imageView, TextView textView)
        {
            textView.Text = string.Empty;
            textView.TextSize = 0.0f;

            imageView.SetImageBitmap(bitmap);
        }
    }
}

