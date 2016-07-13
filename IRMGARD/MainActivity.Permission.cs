using System.Threading.Tasks;

using Android.App;
using Android.Content.PM;
using Android.OS;
using Android.Support.Design.Widget;
using Android.Support.V4.App;
using Android.Util;
using Android.Views;
using Android.Content;

namespace IRMGARD
{
    public partial class MainActivity
    {
        // Id to identify a storage permission request.
        static readonly int REQUEST_STORAGE = 0;

        // Permissions required to read and write to external storage.
        static string[] PERMISSIONS_STORAGE = {
            Android.Manifest.Permission.ReadExternalStorage,
            Android.Manifest.Permission.WriteExternalStorage
        };

        public static bool CheckStoragePermission(Context ctx)
        {
            return (ActivityCompat.CheckSelfPermission(ctx, Android.Manifest.Permission.ReadExternalStorage) != (int)Permission.Granted
                    || ActivityCompat.CheckSelfPermission(ctx, Android.Manifest.Permission.WriteExternalStorage) != (int)Permission.Granted);
        }

        async Task TryCreateAppAsync()
        {
            if (Env.MarshmallowSupport)
            {
                initText.SetText(Resource.String.text_app_check_permissions);
                initText.Visibility = ViewStates.Visible;

                // Check if the Storage permissions are already available.
                Log.Info(TAG, "Check Storage permissions...");
                if (CheckStoragePermission(this))
                {
                    // Storage permissions have not been granted
                    await Task.Delay(3000);
                    RequestStoragePermissions();
                }
                else
                {
                    // Storage permissions are already available
                    Log.Info(TAG, "Storage permissions has already been granted.");
                    await CreateApp();
                }
            }
            else
            {
                Log.Info(TAG, "Pre 23-SDK");
                await CreateApp();
            }
        }

        private void RequestStoragePermissions()
        {
            Log.Info(TAG, "Storage permissions have NOT been granted. Requesting permissions.");

            if (ActivityCompat.ShouldShowRequestPermissionRationale(this, Android.Manifest.Permission.ReadExternalStorage)
                || ActivityCompat.ShouldShowRequestPermissionRationale(this, Android.Manifest.Permission.WriteExternalStorage))
            {
                // Provide an additional rationale to the user if the permission was not granted
                // and the user would benefit from additional context for the use of the permission.
                // For example if the user has previously denied the permission.
                Log.Info(TAG, "Displaying Storage permissions rationale to provide additional context.");
                new PermissionRationaleDialogFragment().Show(FragmentManager, "PermissionRationaleDialogFragmentTag");
            }
            else
            {
                // Storage permissions have not been granted yet. Request it directly.
                ActivityCompat.RequestPermissions(this, PERMISSIONS_STORAGE, REQUEST_STORAGE);
            }
        }

        bool VerifyPermissions(Permission[] grantResults)
        {
            // At least one result must be checked.
            if (grantResults.Length < 1)
                return false;

            // Verify that each required permission has been granted, otherwise return false.
            foreach (Permission result in grantResults)
            {
                if (result != Permission.Granted)
                {
                    return false;
                }
            }
            return true;
        }

        // Callback received when a permissions request has been completed.
        public override async void OnRequestPermissionsResult(int requestCode, string[] permissions, Permission[] grantResults)
        {
            if (requestCode == REQUEST_STORAGE)
            {
                // Received permission result for Storage permission.
                Log.Info(TAG, "Received response for Storage permission request.");

                // Check if all required permissions has been granted
                if (VerifyPermissions(grantResults))
                {
                    // Storage permissions has been granted
                    Log.Info(TAG, "Storage permissions have now been granted.");
                    await CreateApp();
                }
                else
                {
                    Log.Info(TAG, "Storage permissions were NOT granted.");
                    new PermissionRationaleDialogFragment().Show(FragmentManager, "PermissionRationaleDialogFragmentTag");
                }
            }
            else
            {
                base.OnRequestPermissionsResult(requestCode, permissions, grantResults);
            }
        }

        #region ExternalStoragePermissionRationaleDialog

        class PermissionRationaleDialogFragment : Android.App.DialogFragment
        {
            public override Dialog OnCreateDialog(Bundle savedInstanceState)
            {
                AlertDialog.Builder builder = new AlertDialog.Builder(Activity);
                builder.SetMessage(Resource.String.permission_storage_rationale);
                builder.SetPositiveButton(Resource.String.permission_storage_rationale_continue,
                    (dialog, id) => ActivityCompat.RequestPermissions(Activity, PERMISSIONS_STORAGE, REQUEST_STORAGE));
                builder.SetNegativeButton(Resource.String.permission_storage_rationale_quit,
                    (dialog, id) => Activity.Finish());
                return builder.Create();
            }
        }

        #endregion
    }
}