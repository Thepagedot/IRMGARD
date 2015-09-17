		
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
	[Activity (Label = "Modules", ParentActivity = typeof(LevelSelectActivity))]			
	public class ModuleSelectActivity : Activity
	{
		protected override void OnCreate (Bundle bundle)
		{
			base.OnCreate (bundle);
			SetContentView (Resource.Layout.ModuleSelect);

			// Hide image on Lollypop
			if (Build.VERSION.SdkInt <= BuildVersionCodes.Kitkat)
			{
				ActionBar.SetLogo (Resource.Drawable.Icon);
				ActionBar.SetDisplayUseLogoEnabled (true);
				ActionBar.SetDisplayShowHomeEnabled(true);
			}

			ActionBar.SetDisplayHomeAsUpEnabled (true);

			var moduleListView = FindViewById<ListView> (Resource.Id.lvModules);
			moduleListView.ItemClick += ModuleListView_ItemClick;
			moduleListView.Adapter = new ModuleAdapter (this, DataHolder.Current.CurrentLevel.Modules.ToArray());
		}

		void ModuleListView_ItemClick (object sender, AdapterView.ItemClickEventArgs e)
		{
			// Set selected level as current
			DataHolder.Current.CurrentModule = DataHolder.Current.CurrentLevel.Modules.ElementAt(e.Position);
			DataHolder.Current.CurrentLesson = DataHolder.Current.CurrentModule.Lessons.First();
			DataHolder.Current.CurrentIteration = DataHolder.Current.CurrentLesson.Iterations.First();

            if (String.IsNullOrEmpty(DataHolder.Current.CurrentModule.VideoPath))
            {
                // Navigate to lesson view
                var intent = new Intent(this, typeof(LessonFameActivity));
                StartActivity(intent);
            }
            else
            {
                // Navigate to video player
                var intent = new Intent(this, typeof(VideoActivity));
                var bundle = new Bundle();
                bundle.PutString("nextView", "LessonFrameActivity");
                intent.PutExtras(bundle);
                StartActivity(intent);
            }
		}
	}
}