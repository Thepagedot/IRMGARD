using System;
using System.Collections.Generic;

using Android.App;
using Android.Content;
using Android.Views;
using Android.Widget;

using IRMGARD.Models;

namespace IRMGARD
{
    public class MemoryAdapter : ArrayAdapter<MemoryOption>
    {
        public MemoryAdapter(Context context, int resourceId, IList<MemoryOption> items) : base (context, resourceId, items)
        {
        }

        public override View GetView(int position, View convertView, ViewGroup parent)
        {
            var view = convertView;
            if (view == null)
            {
                view = LayoutInflater.From(Context).Inflate(Resource.Layout.MemoryCard, null);

                var item = GetItem(position);
                if (item.Media != null)
                {
                    var ivCardPictureFront = view.FindViewById<ImageView>(Resource.Id.ivCardPictureFront);
                    ivCardPictureFront.SetImageBitmap(BitmapLoader.Instance.LoadBitmap(Count / 2, (Activity) view.Context, item.Media.ImagePath));
                }
                else
                {
                    var textView = view.FindViewById<TextView>(Resource.Id.tvCardTextFront);
                    if (item.Name.Length > 4)
                    {
                        textView.SetTextAppearance(Context, Android.Resource.Style.TextAppearanceDeviceDefaultSmall);
                        textView.SetTextColor(Android.Graphics.Color.Black);
                    }
                    textView.Text = item.Name;
                }
            }

            return view;
        }
    }
}

