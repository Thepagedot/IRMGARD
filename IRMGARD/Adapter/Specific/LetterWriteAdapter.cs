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
    public class LetterWriteAdapter : ArrayAdapter<LetterWriteTask>
    {
        public LetterWriteAdapter(Context context, int resourceId, IList<LetterWriteTask> items) : base (context, resourceId, items)
        {
        }


        public override View GetView(int position, View convertView, ViewGroup parent)
        {
            var item = GetItem(position);
            var view = convertView;
            if (view == null)
            {
                view = LayoutInflater.From(Context).Inflate(Resource.Layout.GifTask, null);
            }

            var gifImageView = view.FindViewById<GifImageView>(Resource.Id.gifImageView);
            if (item.Path != null && gifImageView != null)
            {
                using (var stream = AssetHelper.Instance.Open(item.Path))
                {
                    try
                    {
                        using (var reader = new StreamReader(stream))
                        {
                            var bytes = default(byte[]);
                            using (var memoryStream = new MemoryStream())
                            {
                                reader.BaseStream.CopyTo(memoryStream);
                                bytes = memoryStream.ToArray();
                                gifImageView.SetBytes(bytes);
                            }
                        }
                    }
                    catch (Exception ex)
                    {
                        Console.WriteLine("JSON reader Exception on reading {0}!", item.Path);
                        Console.WriteLine("Message: {0}", ex.Message);
                    }
                }
            }

            return view;
        }
    }
}

