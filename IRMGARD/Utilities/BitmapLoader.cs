using System;
using System.Collections.Generic;
using System.Linq;

using Android.Graphics;
using Android.Content;
using Android.Widget;
using Android.OS;
using Android.Util;

namespace IRMGARD
{
    public sealed class BitmapLoader
    {
        const string TAG = "BitmapCache";
        const string AssetImageDir = "Images";

        static readonly BitmapLoader instance = new BitmapLoader();

        private BitmapLoader() {}

        public static BitmapLoader Instance
        {
            get { return instance; }
        }

        public Bitmap LoadBitmap(int imgIdx, Context context, string fileName,
            string assetImageDir = AssetImageDir)
        {
            Bitmap bitmap = DecodeBitmap(imgIdx, context, fileName, assetImageDir);
            if (Env.Debug)
            {
                Log.Debug(TAG, "Sync. Decoding ({0}) done. Bytes:{1}", bitmap.ToString(), bitmap.ByteCount.ToString());
                TimeProfiler.LogMemInfo();
            }
            return bitmap;
        }

        public void LoadBitmapInImageViewAsync(int imgIdx, ImageView imageView, Context context, string fileName,
            string assetImageDir = AssetImageDir)
        {
            BitmapWorkerTask task = new BitmapWorkerTask(imageView);
            task.Execute(imgIdx, context, fileName, assetImageDir);
        }

        public void ReleaseCache()
        {
            BitmapFactoryOptionsPool.Instance.ReleaseCache();
        }

        static Bitmap DecodeBitmap(int imgIdx, Context context, string fileName, string assetImageDir)
        {
            long started = TimeProfiler.Start();
            BitmapFactory.Options options = BitmapFactoryOptionsPool.Instance.Get(imgIdx);
            if (options.InBitmap == null)
            {
                // First decode with inJustDecodeBounds=true to check dimensions
                options.InJustDecodeBounds = true;
                using (var stream = context.Assets.Open(System.IO.Path.Combine(assetImageDir, fileName)))
                {
                    BitmapFactory.DecodeStream(stream, null, options);
                }
                Bitmap bitmap = Bitmap.CreateBitmap(options.OutWidth, options.OutHeight, options.InPreferredConfig);
                options.InJustDecodeBounds = false;
                options.InBitmap = bitmap;
                options.InSampleSize = 1;
                BitmapFactoryOptionsPool.Instance.Put(imgIdx, options);
            }
            // Decode bitmap with inSampleSize set
            using (var stream = context.Assets.Open(System.IO.Path.Combine(assetImageDir, fileName)))
            {
                var bitmap = BitmapFactory.DecodeStream(stream, null, options);
                TimeProfiler.StopAndLog(TAG, "Decode Stream", started);
                return bitmap;
            }
        }

        class BitmapFactoryOptionsPool
        {
            static readonly BitmapFactoryOptionsPool instance = new BitmapFactoryOptionsPool();

            readonly List<BitmapFactory.Options> items = new List<BitmapFactory.Options>();

            private BitmapFactoryOptionsPool() {}

            public static BitmapFactoryOptionsPool Instance
            {
                get { return instance; }
            }

            void CheckAddItems(int index)
            {
                if (index > items.Count - 1)
                {
                    items.AddRange(Enumerable.Repeat(new BitmapFactory.Options(), index - (items.Count - 1)).Cast<BitmapFactory.Options>());
                }
            }

            public BitmapFactory.Options Get(int index)
            {
                CheckAddItems(index);
                return items[index];
            }

            public void Put(int index, BitmapFactory.Options options)
            {
                CheckAddItems(index);
                items[index] = options;
            }

            public void ReleaseCache()
            {
                foreach (var item in items)
                {
                    if (item != null && item.InBitmap != null)
                    {
                        item.InBitmap.Dispose();
                        item.InBitmap = null;
                    }
                }
                items.Clear();

                System.GC.Collect();
            }
        }

        class BitmapWorkerTask : AsyncTask<object, object, Bitmap>
        {
            const string TAG = "BitmapWorkerTask";

            readonly WeakReference<ImageView> imageViewReference;

            public BitmapWorkerTask(ImageView imageView)
            {
                // Use a WeakReference to ensure the ImageView can be garbage collected
                imageViewReference = new WeakReference<ImageView>(imageView);
            }

            // Decode image in background.
            protected override Bitmap RunInBackground(params object[] objArr)
            {
                int imgIdx = ((Java.Lang.Integer)objArr[0]).IntValue();
                Context context = (Context)objArr[1];
                string fileName = ((Java.Lang.String)objArr[2]).ToString();
                string assetImageDir = ((Java.Lang.String)objArr[3]).ToString();

                return DecodeBitmap(imgIdx, context, fileName, assetImageDir);
            }

            // Once complete, see if ImageView is still around and set bitmap.
            protected override void OnPostExecute(Bitmap bitmap)
            {
                if (imageViewReference != null && bitmap != null)
                {
                    ImageView imageView;
                    if (imageViewReference.TryGetTarget(out imageView))
                    {
                        if (Env.Debug)
                        {
                            Log.Debug(TAG, "Async. Decoding ({0}) done. Bytes:{1}", bitmap.ToString(), bitmap.ByteCount.ToString());
                            TimeProfiler.LogMemInfo();
                        }
                        imageView.SetImageBitmap(bitmap);
                    }
                }
            }
        }
    }
}

