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
                // TODO Release: Switch between "IRMGARD" and "IRMGARD Prerelease" app configuration

                // IRMGARD
                return "MIIBIjANBgkqhkiG9w0BAQEFAAOCAQ8AMIIBCgKCAQEAiizOn/iHw6H3AuNg0TQG1qQv0lT2SsCiY1GdfDoYXKzIrLRoxYw1ozehxR2cb3VrglChUK+/4mV0I2lAkh3yAedT6pyRdPcWk4Nev5Tak3w8aXN6UjkZj/BgxgVVQZtYrFbx5vikx9nT1TNoUlkylXAXAKRmbGP1Kxc7FXakZJvflsd1rlJSx9bsFtHyeUBeuZGmEbB8k6qNdgsr4xy4BNrfKSnn/mJ64GmJR+YiH5SK0SHWpITATtzWRLBaMD/LPDRYFf6jvAClkFHJdm3tCjGIAS0uhIbCotfmEFNCzzhs0EN11iEv40uIMUMAQP1jlduPoxNb/WVulq21QQ0D8QIDAQAB";

                // IRMGARD Prerelease
                // return "MIIBIjANBgkqhkiG9w0BAQEFAAOCAQ8AMIIBCgKCAQEAieZl7lCwRPcjbixc0pNl7EVCS6lj/JpFm8RHSb7G3ER95R7TQREPBrFFiCBcgF6zozCCNbKUOtum9J++DNS78k9g1TrpIYLRQU1YolFA65uAENcSqMmxazZQReXkMtQmXw3hidX5J3db+psE9k0IiKZ2B7ROHWBkBlc7Cgrj9nabmMZOFXoOMOPg6xL8hCvbd/4Af5zIBZP2FYeYWaoLb+fKt8dn2OqiZEnKMCjtQWOYv12bqmjuyd4WPD7AjbH9I3bSzrkZvIrh26qXDMmGCBLLIknKMbyocESuac9diew/yMkCnVtxnQHY8aAiO937vkP4D3u5MbQLi6go73pIyQIDAQAB";
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