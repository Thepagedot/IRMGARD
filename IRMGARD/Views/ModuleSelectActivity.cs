
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
using IRMGARD.Models;
using Android.Graphics;

namespace IRMGARD
{
	[Activity (Label = "Modulauswahl", ParentActivity = typeof(LevelSelectActivity))]
    public class ModuleSelectActivity : AppCompatActivity
	{
        private ModuleAdapter moduleAdapter;

		protected override void OnCreate (Bundle bundle)
		{
			base.OnCreate (bundle);
			SetContentView (Resource.Layout.ModuleSelect);
            SetSupportActionBar(FindViewById<Toolbar>(Resource.Id.toolbar));
            SupportActionBar.SetDisplayHomeAsUpEnabled(true);
            //this.SetSystemBarBackground (Color.ParseColor (DataHolder.Current.CurrentLevel.Color));

            moduleAdapter = new ModuleAdapter(this, DataHolder.Current.CurrentLevel.Modules.ToArray());
			var moduleListView = FindViewById<ListView> (Resource.Id.lvModules);
			moduleListView.ItemClick += ModuleListView_ItemClick;
            moduleListView.Adapter = moduleAdapter;
		}

        protected override void OnResume()
        {
            base.OnResume();

            SoundPlayer.Stop();

            if (moduleAdapter != null)
            {
                moduleAdapter.NotifyDataSetChanged();
            }
		}


		void ModuleListView_ItemClick (object sender, AdapterView.ItemClickEventArgs e)
		{
			// Set selected level as current
			DataHolder.Current.CurrentModule = DataHolder.Current.CurrentLevel.Modules.ElementAt(e.Position);

            // Check if module has been implemented
            if (!DataHolder.Current.CurrentModule.Lessons.Any())
            {
                Toast.MakeText(this, "This module has not been implemented yet.", ToastLength.Short).Show();
                return;
            }

            if (Env.RestrictedModuleAccess)
            {
                // Check if module is available
                if (e.Position > 0 && !DataHolder.Current.CurrentLevel.Modules.ElementAt(e.Position - 1).IsCompleted)
                {
                    SoundPlayer.PlaySound(this, "Application/Module_Not_Available.mp3");                    
                    return;
                }
            }

			DataHolder.Current.CurrentLesson = DataHolder.Current.CurrentModule.Lessons.First();
			DataHolder.Current.CurrentIteration = DataHolder.Current.CurrentLesson.Iterations.First();

            if (String.IsNullOrEmpty(DataHolder.Current.CurrentModule.VideoPath))
            {
                // Navigate to Lesson view
                var intent = new Intent(this, typeof(LessonFameActivity));
                StartActivity(intent);
            }
            else
            {
                // Navigate to video player
                var intent = new Intent(this, typeof(VideoActivity));
                var bundle = new Bundle();
                bundle.PutString("nextView", "LessonFrameActivity");
                bundle.PutString("videoPath", DataHolder.Current.CurrentModule.VideoPath);
                intent.PutExtras(bundle);
                StartActivity(intent);
            }
		}

        public override bool OnCreateOptionsMenu(IMenu menu)
        {
            MenuInflater.Inflate(Resource.Menu.legal_notice_menu, menu);

            return base.OnCreateOptionsMenu(menu);
        }

        public override bool OnOptionsItemSelected(IMenuItem item)
        {
            switch (item.ItemId)
            {
                // Navigate to legal notice
                case Resource.Id.btnLegalNotice:
                    StartActivity(new Intent(this, typeof(LegalNoticeActivity)));
                    break;
            }

            return base.OnOptionsItemSelected(item);
        }
    }
}