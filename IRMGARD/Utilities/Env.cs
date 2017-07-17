using Android.OS;

namespace IRMGARD
{
    public static class Env
    {
#if DEBUG
        public const bool Debug = true;
        public const bool UseOBB = false;                   // Default: false
        public const bool RestrictedModuleAccess = false;
#else
        public const bool Debug = false;
        public const bool UseOBB = true;
        public const bool RestrictedModuleAccess = false;    // Alpha/Beta: false, Production: true
#endif
        public const bool Release = !Debug;

        public static bool MarshmallowSupport
        {
            get { return Build.VERSION.SdkInt >= BuildVersionCodes.M; }
        }

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
