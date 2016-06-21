using System;
using System.Collections.Generic;

using Android.Graphics;
using Android.Content;
using Android.Widget;
using Android.OS;
using Android.Util;

namespace IRMGARD
{
    /// <summary>
    /// Represents a bitmap image loader facility to reduce the memory usage and the amount of GC events
    /// by loading new bitmaps into the space of bitmaps used before.
    ///
    /// Re-using the allocated memory of an old image is only applicable for equally-sized or smaller images.
    ///
    /// To reuse the allocated memory of the bitmaps used before you have to limit maxPoolSize to the maximum
    /// amount of images used in the current lesson. When max pool size limit is reached the next new image
    /// will reuse the allocated memory of the oldest image in the pool.
    ///
    /// To finally release the allocated memory use <see cref="ReleaseCache"/>.
    /// The best time to release allocated bitmap memory is at the end of each Lesson if the subsequent Lesson
    /// is using a smaller amount of bitmaps per iteration than the previous one.
    /// </summary>
    public sealed class BitmapLoader
    {
        const string TAG = "BitmapCache";

        static readonly BitmapLoader instance = new BitmapLoader();

        readonly BitmapFactoryOptionsPool bitmapPool;

        private BitmapLoader() {
            bitmapPool = new BitmapFactoryOptionsPool();
        }

        public static BitmapLoader Instance
        {
            get { return instance; }
        }

        /// <summary>
        /// Loads a non-scaled bitmap from the Assets folder into memory.
        /// </summary>
        /// <returns>The bitmap.</returns>
        /// <param name="maxPoolSize">The maximum limit of images to pool.</param>
        /// <param name="context">A context to get an AssetManager instance.</param>
        /// <param name="fileName">The image file path.</param>
        /// <param name="assetImageDir">The parent directory for <paramref name="fileName"/> path parameter.</param>
        public Bitmap LoadBitmap(int maxPoolSize, Context context, string fileName,
            string assetImageDir = null)
        {
            Bitmap bitmap = DecodeBitmap(maxPoolSize, context, fileName, assetImageDir);
            if (Env.Debug)
            {
                Log.Debug(TAG, "Sync. Decoding ({0}) done. Bytes:{1}", bitmap.ToString(), bitmap.ByteCount.ToString());
                TimeProfiler.LogMemInfo();
                Log.Debug(TAG, "========================================");
            }
            return bitmap;
        }

        /// <summary>
        /// Loads a non-scaled bitmap from the Assets folder on a background thread into an ImageView.
        /// </summary>
        /// <param name="maxPoolSize">The maximum limit of images to pool.</param>
        /// <param name="imageView">The ImageView for the bitmap to load into.</param>
        /// <param name="context">A context to get an AssetManager instance.</param>
        /// <param name="fileName">The image file path.</param>
        /// <param name="assetImageDir">The parent directory for <paramref name="fileName"/> path parameter.</param>
        public void LoadBitmapInImageViewAsync(int maxPoolSize, ImageView imageView, Context context, string fileName,
            string assetImageDir = null)
        {
            BitmapWorkerTask task = new BitmapWorkerTask(this, imageView);
            task.Execute(maxPoolSize, context, fileName, assetImageDir);
        }

        public void ReleaseCache()
        {
            bitmapPool.ReleaseCache();
        }

        Bitmap DecodeBitmap(int maxPoolSize, Context context, string fileName, string assetImageDir)
        {
            string filePath = System.IO.Path.Combine(assetImageDir ?? DataHolder.Current.Common.AssetImageDir, fileName);

            BitmapFactory.Options options = null;
            if (bitmapPool.TryGetOptions(maxPoolSize, filePath, out options))
            {
                return options.InBitmap;
            }

            long started = TimeProfiler.Start();
            if (options.InBitmap == null)
            {
                // First decode with inJustDecodeBounds=true to check dimensions
                options.InJustDecodeBounds = true;
                using (var stream = AssetHelper.Instance.Open(filePath))
                {
                    BitmapFactory.DecodeStream(stream, null, options);
                }

                // Due to a bug in SDKs below Kitkat a scaled down version of a bitmap cannot be decoded into an existing bitmap
                var ratio = (Env.KitkatSupport) ? 2 : 1;

                options.OutWidth = options.OutWidth / ratio;
                options.OutHeight = options.OutHeight / ratio;
                Bitmap bitmap = Bitmap.CreateBitmap(options.OutWidth, options.OutHeight, options.InPreferredConfig);
                options.InJustDecodeBounds = false;
                options.InBitmap = bitmap;
                options.InSampleSize = ratio;
            }

            // Decode bitmap with inSampleSize set
            using (var stream = AssetHelper.Instance.Open(filePath))
            {
                var bitmap = BitmapFactory.DecodeStream(stream, null, options);
                TimeProfiler.StopAndLog(TAG, "Decode Stream", started);
                return bitmap;
            }
        }

        class BitmapFactoryOptionsPool
        {
            readonly List<OptionsData> pool = new List<OptionsData>();

            public BitmapFactoryOptionsPool() {}

            public bool TryGetOptions(int maxPoolSize, string filePath, out BitmapFactory.Options options)
            {
                foreach (var item in pool)
                {
                    if (item.FilePath.Equals(filePath))
                    {
                        // Image already in the pool - just return it
                        item.LastAccess = DateTime.Now;
                        options = item.Options;
                        return true;
                    }
                }

                if (maxPoolSize > pool.Count)
                {
                    // Add new instance to pool
                    var optionsData = new OptionsData(filePath, DateTime.Now, new BitmapFactory.Options());
                    options = optionsData.Options;

                    pool.Add(optionsData);
                }
                else
                {
                    // Find the oldest pool item
                    OptionsData optionsData = null;
                    var lastAccess = DateTime.Now;
                    foreach (var item in pool)
                    {
                        if (lastAccess.CompareTo(item.LastAccess) == 1)
                        {
                            lastAccess = item.LastAccess;
                            optionsData = item;
                        }
                    }

                    // ...and substitute with the new item 
                    optionsData.FilePath = filePath;
                    optionsData.LastAccess = DateTime.Now;
                    options = optionsData.Options;
                }

                return false;
            }

            public void ReleaseCache()
            {
                foreach (var item in pool)
                {
                    if (item.Options != null && item.Options.InBitmap != null)
                    {
                        item.Options.InBitmap.Dispose();
                        item.Options.InBitmap = null;
                    }
                }
                pool.Clear();
                System.GC.Collect();
            }

            class OptionsData
            {
                public string FilePath { get; set; }
                public DateTime LastAccess { get; set; }
                public BitmapFactory.Options Options { get; set; }

                public OptionsData(string filePath, DateTime lastAccess, BitmapFactory.Options options)
                {
                    FilePath = filePath;
                    LastAccess = lastAccess;
                    Options = options;
                }
            }
        }

        class BitmapWorkerTask : AsyncTask<object, object, Bitmap>
        {
            const string TAG = "BitmapWorkerTask";

            readonly BitmapLoader parent;
            readonly WeakReference<ImageView> imageViewReference;

            public BitmapWorkerTask(BitmapLoader parent, ImageView imageView)
            {
                this.parent = parent;
                // Use a WeakReference to ensure the ImageView can be garbage collected
                imageViewReference = new WeakReference<ImageView>(imageView);
            }

            // Decode image in background.
            protected override Bitmap RunInBackground(params object[] objArr)
            {
                int maxPoolSize = ((Java.Lang.Integer)objArr[0]).IntValue();
                Context context = (Context)objArr[1];
                string fileName = ((Java.Lang.String)objArr[2]).ToString();
                string assetImageDir = ((Java.Lang.String)objArr[3]).ToString();

                return parent.DecodeBitmap(maxPoolSize, context, fileName, assetImageDir);
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

