using Android.OS;

namespace IRMGARD
{
    public static class Env
    {
        public const bool Debug = true;
        public const bool Release = !Debug;

        public static bool LollipopSupport
        {
            get { return Build.VERSION.SdkInt >= BuildVersionCodes.Lollipop; }
        }

        public static bool KitkatSupport
        {
            get { return Build.VERSION.SdkInt >= BuildVersionCodes.Kitkat; }
        }
    }
}
