using Android.OS;

namespace IRMGARD
{
    public static class Env
    {
#if DEBUG
        public const bool Debug = true;
#else
        public const bool Debug = false;
#endif
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
