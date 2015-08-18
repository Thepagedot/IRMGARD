
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Android.App;
using Android.Content;
using Android.OS;
using Android.Runtime;
using Android.Util;
using Android.Views;
using Android.Widget;
using IRMGARD.Models;

namespace IRMGARD
{
    public class HearMeFragment : LessonFragment<HearMe>
	{
        public HearMeFragment (Lesson lesson) : base(lesson) {}

        public override void OnCreate (Bundle savedInstanceState)
        {
            base.OnCreate (savedInstanceState);
        }

        protected override void InitIteration()
        {
        }

		public override View OnCreateView (LayoutInflater inflater, ViewGroup container, Bundle savedInstanceState)
		{
			var view = inflater.Inflate(Resource.Layout.HearMe, container, false);
			var finishButton = view.FindViewById<Button>(Resource.Id.btnFinish);
			finishButton.Click += FinishButton_Click;
			return view;
		}

		void FinishButton_Click (object sender, EventArgs e)
		{
            CheckSolution();
		}

        protected override void CheckSolution()
        {
            FinishIteration();
        }            
	}
}