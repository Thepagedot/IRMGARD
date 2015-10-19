using System;
using Android.Graphics;

namespace IRMGARD
{
    public static class MemoryHelper
    {
        public static void Recycle(Bitmap bitmap)
        {
            if (bitmap != null && !bitmap.IsRecycled)
            {
                bitmap.Recycle();
                System.GC.Collect();
            }
        }
    }
}

