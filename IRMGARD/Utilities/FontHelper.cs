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
            Sen,
            SenBold
        };

        static FontHelper()
        {
            fontFiles.Add(Font.Sen, "Fonts/sen-regular.otf");
            fontFiles.Add(Font.SenBold, "Fonts/sen-bold.otf");
        }

        public static Typeface Get(Context context, Font font)
        {
            Typeface tf = null;
            if (!fontCache.TryGetValue(font, out tf))
            {
                try
                {
                    tf = Typeface.CreateFromAsset(context.Assets, fontFiles[font]);
                }
                catch (Exception e)
                {
                    if (Env.Debug)
                    {
                        Log.Debug(TAG, "Cannot load font {0} ({1})!", fontFiles[font], e.Message);
                    }
                    return Typeface.Default;
                }
                fontCache[font] = tf;
            }

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

