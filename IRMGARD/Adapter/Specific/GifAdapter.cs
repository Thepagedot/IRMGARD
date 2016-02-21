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
                byte[] buffer;

                using (var stream = Context.Assets.Open(item.Path))
                {
                    StreamReader bodyReader = new StreamReader(stream);
                    string bodyString = bodyReader.ReadToEnd();
                    int length = bodyString.Length;
                    buffer = new byte[length];

                    stream.Read(buffer, 0, length);
                    var gifImageView = view.FindViewById<GifImageView>(Resource.Id.gifImageView);

                    /*byte[] bytes = new byte[bodyString.Length * sizeof(char)];
                    System.Buffer.BlockCopy(bodyString.ToCharArray(), 0, bytes, 0, bytes.Length);
                    */
                        
                    if (gifImageView != null)
                        gifImageView.SetBytes(buffer);
                }
            }

            return view;
        }
    }
}

