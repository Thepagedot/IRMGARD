﻿using System;
using Android.App;
using Android.Graphics;
using Android.Graphics.Drawables;
using Android.OS;
using Android.Views;

namespace IRMGARD
{
    public static class ActivityHelper
    {
        /// <summary>
        /// Sets the system bar background to a sepcific color when > Lollypop.
        /// </summary>
        /// <returns>The system bar background.</returns>
        /// <param name="activity">Activity.</param>
        /// <param name="colorResourceId">Color resource identifier.</param>
        public static void SetSystemBarBackground (this Activity activity, int colorResourceId)
        {
            if (Build.VERSION.SdkInt >= BuildVersionCodes.Lollipop) {
                // Set status bar color
                activity.Window.AddFlags (WindowManagerFlags.DrawsSystemBarBackgrounds);
                activity.Window.SetStatusBarColor (activity.Resources.GetColor (colorResourceId));
            }
        }

        public static void SetSystemBarBackground (this Activity activity, Color color)
        {
            if (Build.VERSION.SdkInt >= BuildVersionCodes.Lollipop) {
                // Set status bar color
                activity.Window.AddFlags (WindowManagerFlags.DrawsSystemBarBackgrounds);
                activity.Window.SetStatusBarColor (color);
            }
        }

        public static void SetActionBarBackground (this Activity activity, ActionBar actionBar, int colorResourceId)
        {
            actionBar.SetBackgroundDrawable(new ColorDrawable(activity.Resources.GetColor(colorResourceId)));
        }
    }
}

