
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
using Square.Picasso;

namespace IRMGARD
{
    [Activity(Label = "Danke", ParentActivity = typeof(LevelSelectActivity), NoHistory = true)]
    public class LevelSponsorActivity : AppCompatActivity
    {
        protected override void OnCreate(Bundle bundle)
        {
            base.OnCreate(bundle);
            SetContentView(Resource.Layout.LevelSponsor);
            SetSupportActionBar(FindViewById<Toolbar>(Resource.Id.toolbar));
            SupportActionBar.SetDisplayHomeAsUpEnabled(true);
            this.CompatMode();

            // Set title
            Title = DataHolder.Current.CurrentLevel.Name;

            var closeButton = FindViewById<FloatingActionButton>(Resource.Id.btnClose);
            closeButton.Click += CloseButton_Click;
        }

        void CloseButton_Click (object sender, EventArgs e)
        {
            if (String.IsNullOrEmpty(DataHolder.Current.CurrentLevel.VideoPath))
            {
                StartActivity(new Intent(this, typeof(ModuleSelectActivity)));
            }
            else
            {
                // Navigate to video player
                var intent = new Intent(this, typeof(VideoActivity));
                var bundle = new Bundle();
                bundle.PutString("nextView", "ModuleSelectActivity");
                bundle.PutString("videoPath", DataHolder.Current.CurrentLevel.VideoPath);
                intent.PutExtras(bundle);
                StartActivity(intent);
            }
        }
    }
}

