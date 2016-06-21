using Android.App;

using ExpansionDownloader.Service;

namespace IRMGARD
{
    [Service]
    public class MediaDownloadService : DownloaderService
    {
        /// <summary>
        /// This public key comes from your Android Market publisher account, and it
        /// used by the LVL to validate responses from Market on your behalf.
        /// Note: MODIFY FOR YOUR APPLICATION!
        /// </summary>
        protected override string PublicKey
        {
            get
            {
                return "MIIBIjANBgkqhkiG9w0BAQEFAAOCAQ8AMIIBCgKCAQEAieZl7lCwRPcjbixc0pNl7EVCS6lj/JpFm8RHSb7G3ER95R7TQREPBrFFiCBcgF6zozCCNbKUOtum9J++DNS78k9g1TrpIYLRQU1YolFA65uAENcSqMmxazZQReXkMtQmXw3hidX5J3db+psE9k0IiKZ2B7ROHWBkBlc7Cgrj9nabmMZOFXoOMOPg6xL8hCvbd/4Af5zIBZP2FYeYWaoLb+fKt8dn2OqiZEnKMCjtQWOYv12bqmjuyd4WPD7AjbH9I3bSzrkZvIrh26qXDMmGCBLLIknKMbyocESuac9diew/yMkCnVtxnQHY8aAiO937vkP4D3u5MbQLi6go73pIyQIDAQAB";
            }
        }

        /// <summary>
        /// This is used by the preference obfuscater to make sure that your
        /// obfuscated preferences are different than the ones used by other
        /// applications.
        /// </summary>
        protected override byte[] Salt
        {
            get
            {
                return new byte[] { 7, 23, 6, 5, 24, 9, 10, 100, 83, 24, 83, 42, 39, 23, 102, 101, 36, 5, 4, 64 };
            }
        }

        /// <summary>
        /// Fill this in with the class name for your alarm receiver. We do this
        /// because receivers must be unique across all of Android (it's a good idea
        /// to make sure that your receiver is in your unique package)
        /// </summary>
        protected override string AlarmReceiverClassName
        {
            get
            {
                return "irmgard.MediaDownloadAlarmReceiver";
            }
        }
    }
}