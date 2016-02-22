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

            if (item.Path != null)
            {
                getGifStream(item.Path, view.FindViewById<GifImageView>(Resource.Id.gifImageView));
            }

            return view;
        }

        private async Task getGifStream(string path, GifImageView gifImageView)
        {
            using (var stream = Context.Assets.Open(path))
            {                   
                var streamReader = new StreamReader(stream);
                var bytes = default(byte[]);
                using (var memoryStream = new MemoryStream())
                {
                    streamReader.BaseStream.CopyTo(memoryStream);
                    bytes = memoryStream.ToArray();

                    if (gifImageView != null)
                    {
                        gifImageView.SetBytes(bytes);
                        gifImageView.StartAnimation();
                        await Task.Delay(1);
                        gifImageView.StopAnimation();
                    }
                }
            }
        }
    }
}

