
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Android.App;
using Android.Content;
using Android.OS;
using Android.Runtime;
using Android.Views;
using Android.Widget;
using Android.Support.V7.App;
using Toolbar = Android.Support.V7.Widget.Toolbar;
using Android.Support.Design.Widget;

namespace IRMGARD
{
    [Activity(Label = "Danke an", ParentActivity = typeof(LevelSelectActivity))]            
    public class LevelSponsorActivity : AppCompatActivity
    {
        protected override void OnCreate(Bundle bundle)
        {
            base.OnCreate(bundle);
            SetContentView(Resource.Layout.LevelSponsor);
            SetSupportActionBar(FindViewById<Toolbar>(Resource.Id.toolbar));
            SupportActionBar.SetDisplayHomeAsUpEnabled (true);
            SupportActionBar.Subtitle = String.Format(GetString(Resource.String.sponsor_welcome), DataHolder.Current.CurrentLevel.Name);
                      
            var closeButton = FindViewById<FloatingActionButton>(Resource.Id.btnClose);
            closeButton.Click += CloseButton_Click;
        }

        void CloseButton_Click (object sender, EventArgs e)
        {
            var intent = new Intent(this, typeof(ModuleSelectActivity));
            intent.SetFlags(ActivityFlags.ClearTop);
            StartActivity(intent);
        }
    }
}

