using System;
using IRMGARD.Models;
using Android.Widget;
using Android.Views;
using Android.Content;
using System.Collections.Generic;
using Felipecsl.GifImageViewLibrary;
using System.IO;
using System.Threading.Tasks;

namespace IRMGARD
{
    public class GifAdapter : ArrayAdapter<GifTask>
    {
        public GifAdapter(Context context, int resourceId, IList<GifTask> items) : base (context, resourceId, items)
        {
        }


        public override View GetView(int position, View convertView, ViewGroup parent)
        {
            var item = GetItem(position);
            var view = convertView;
            if (view == null)
                view = LayoutInflater.From(Context).Inflate(Resource.Layout.GifTask, null);
            /*else
                ((BitmapDrawable)view.FindViewById<ImageView>(Resource.Id.image).Drawable).Bitmap.Recycle();*/

            var gifImageView = view.FindViewById<GifImageView>(Resource.Id.gifImageView);

            if (item.Path != null)
            {
                using (var stream = Context.Assets.Open(item.Path))
                {                   
                    var streamReader = new StreamReader(stream);
                    var bytes = default(byte[]);
                    using (var memoryStream = new MemoryStream())
                    {
                        streamReader.BaseStream.CopyTo(memoryStream);
                        bytes = memoryStream.ToArray();

                        if (gifImageView != null)
                            gifImageView.SetBytes(bytes);
                    }
                }
            }

            return view;
        }
    }
}

