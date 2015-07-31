
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

namespace IRMGARD
{
	[Activity (Label = "Levels", ParentActivity = typeof(MainActivity))]			
	public class LevelSelectActivity : Activity
	{
		protected override void OnCreate (Bundle bundle)
		{
			base.OnCreate (bundle);
			SetContentView (Resource.Layout.LevelSelect);

			// Hide image on Lollypop
			if (Build.VERSION.SdkInt <= BuildVersionCodes.Kitkat)
			{
				ActionBar.SetLogo (Resource.Drawable.Icon);
				ActionBar.SetDisplayUseLogoEnabled (true);
				ActionBar.SetDisplayShowHomeEnabled(true);
			}

			ActionBar.SetDisplayHomeAsUpEnabled (true);

			var levelListView = FindViewById<ListView> (Resource.Id.lvLevels);
			levelListView.ItemClick += LevelListView_ItemClick;
			levelListView.Adapter = new LevelAdapter (this, DataHolder.Current.Levels.ToArray());
		}

		void LevelListView_ItemClick (object sender, AdapterView.ItemClickEventArgs e)
		{
			// Set selected level as current
			DataHolder.Current.CurrentLevel = DataHolder.Current.Levels.ElementAt(e.Position);

			// Navigate to lesson view
            var intent = new Intent(this, typeof(LevelSponsorActivity));
			StartActivity (intent);
		}
	}
}