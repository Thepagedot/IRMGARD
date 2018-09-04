using Android.App;
using Android.OS;
using Android.Support.V7.App;
using Android.Text;
using Android.Views;
using Android.Widget;
using Toolbar = Android.Support.V7.Widget.Toolbar;

namespace IRMGARD
{
    [Activity(Label = "Kontakt", NoHistory = true)]
    public class LegalNoticeActivity : AppCompatActivity
    {
        protected override void OnCreate(Bundle bundle)
        {
            base.OnCreate(bundle);
            SetContentView(Resource.Layout.LegalNotice);
            SetSupportActionBar(FindViewById<Toolbar>(Resource.Id.toolbar));
            SupportActionBar.SetDisplayHomeAsUpEnabled(true);
            //RequestedOrientation = Android.Content.PM.ScreenOrientation.Portrait;

            var version = PackageManager.GetPackageInfo(PackageName, 0).VersionName;
            FindViewById<TextView>(Resource.Id.tvVersionInfo).TextFormatted = FromHtml(string.Format(Resources.GetString(Resource.String.version_info), version));
            FindViewById<TextView>(Resource.Id.tvLegalNotice).TextFormatted = FromHtml(Resources.GetString(Resource.String.legalnotice_content));
        }

        public override bool OnOptionsItemSelected(IMenuItem item)
        {
            switch (item.ItemId)
            {
                // Respond to the action bar's Up/Home button
                case Android.Resource.Id.Home:
                    Finish();
                    return true;
            }
            return base.OnOptionsItemSelected(item);
        }

        private ISpanned FromHtml(string html)
        {
            if (Env.NougatSupport)
            {
                return Html.FromHtml(html, FromHtmlOptions.ModeLegacy);
            }
            else
            {
                return Html.FromHtml(html);
            }
        }
    }
}