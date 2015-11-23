
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

            // Set title and image
            Title = DataHolder.Current.CurrentLevel.Name;
            var imageView = FindViewById<ImageView>(Resource.Id.ivSponsors);
            Picasso.With(this).Load(Resource.Drawable.irmgard_danke_01).Into(imageView);

            var closeButton = FindViewById<FloatingActionButton>(Resource.Id.btnClose);
            closeButton.Click += CloseButton_Click;
        }

        void CloseButton_Click (object sender, EventArgs e)
        {
            var intent = new Intent(this, typeof(ModuleSelectActivity));
            StartActivity(intent);
        }
    }
}

