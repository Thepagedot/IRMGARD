
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

namespace IRMGARD
{
	[Activity (Label = "Level Auswahl", ParentActivity = typeof(MainActivity))]
    public class LevelSelectActivity : AppCompatActivity
	{
		protected override void OnCreate (Bundle bundle)
		{
			base.OnCreate (bundle);
			SetContentView (Resource.Layout.LevelSelect);
            SetSupportActionBar(FindViewById<Toolbar>(Resource.Id.toolbar));
            SupportActionBar.SetDisplayHomeAsUpEnabled(true);
            this.CompatMode();

			var levelListView = FindViewById<ListView> (Resource.Id.lvLevels);
			levelListView.ItemClick += LevelListView_ItemClick;
			levelListView.Adapter = new LevelAdapter (this, DataHolder.Current.Levels.ToArray());
		}

		void LevelListView_ItemClick (object sender, AdapterView.ItemClickEventArgs e)
		{
            // Check if level is enabled
            if (!DataHolder.Current.Levels.ElementAt(e.Position).IsEnabled)
            {
                if (SoundPlayer.IsPlaying)
                {
                    SoundPlayer.Stop();
                }
                SoundPlayer.PlaySound(this, "Application/Level_Not_Available.mp3");
                return;
            }

			// Set selected level as current
			DataHolder.Current.CurrentLevel = DataHolder.Current.Levels.ElementAt(e.Position);

			// Navigate to Lesson view
            var intent = new Intent(this, typeof(LevelSponsorActivity));
			StartActivity(intent);
		}
	}
}