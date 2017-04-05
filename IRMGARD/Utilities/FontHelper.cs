using System;
using System.Collections.Generic;

using Android.Content;
using Android.Graphics;
using Android.Util;

using Java.Lang.Reflect;

namespace IRMGARD
{
    public class FontHelper
    {
        const string TAG = "FontHelper";

        static Dictionary<Font, string> fontFiles = new Dictionary<Font, string>();
        static Dictionary<Font, Typeface> fontCache = new Dictionary<Font, Typeface>();

        public enum Font
        {
            StandardRegular,
            StandardBold
        };

        static FontHelper()
        {
            fontFiles.Add(Font.StandardRegular, "Fonts/GlacialIndifference-Regular.otf");
            fontFiles.Add(Font.StandardBold, "Fonts/GlacialIndifference-Bold.otf");
        }

        public static Typeface Get(Context context, Font font)
        {
            Typeface tf = null;
            if (!fontCache.TryGetValue(font, out tf))
            {
                try
                {
                    tf = GetTypeface(context, fontFiles[font]);
                }
                catch (Exception e)
                {
                    if (Env.Debug)
                    {
                        Log.Debug(TAG, "Cannot load font {0} ({1})!", fontFiles[font], e.Message);
                    }
                    tf = Typeface.Default;
                }
                fontCache[font] = tf;
            }

            return tf;
        }

        private static Typeface GetTypeface(Context context, string fontPath)
        {
            string tempFilePath = AssetHelper.Instance.GetExtractedTempFilePath(fontPath);
            var tf = Typeface.CreateFromFile(tempFilePath);
            AssetHelper.Instance.DeleteExtractedTempFile(fontPath);

            return tf;
        }

        public static void ReplaceDefaultFont(Context context, string staticTypefaceFieldName, Font font)
        {
            Typeface tf = Get(context, font);
            try
            {
                Field staticField = ((Java.Lang.Object) tf).Class.GetDeclaredField(staticTypefaceFieldName);
                staticField.Accessible = true;
                staticField.Set(null, tf);
            }
            catch (Exception e)
            {
                if (Env.Debug)
                {
                    Log.Debug(TAG, "Cannot replace default font {0} ({1})!", fontFiles[font], e.Message);
                }
            }
        }
    }
}

