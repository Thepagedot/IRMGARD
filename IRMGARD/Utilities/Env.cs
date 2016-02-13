using Android.OS;

namespace IRMGARD
{
    public static class Env
    {
        public const bool Debug = true;
        public const bool Release = !Debug;

        public const string AssetImageDir = "Images";
        public const string AssetSoundDir = "Sounds";

        public static bool LollipopSupport
        {
            get { return Build.VERSION.SdkInt >= BuildVersionCodes.Lollipop; }
        }
    }
}

