
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

namespace IRMGARD
{	
	public class FourPicturesFragment : LessonFragment
	{
		public override void OnCreate (Bundle savedInstanceState)
		{
			base.OnCreate (savedInstanceState);
		}

		public override View OnCreateView (LayoutInflater inflater, ViewGroup container, Bundle savedInstanceState)
		{
			var view = inflater.Inflate(Resource.Layout.FourPictures, container, false);
			var finishButton = view.FindViewById<Button>(Resource.Id.btnFinish);
			finishButton.Click += FinishButton_Click;
			return view;
		}

		void FinishButton_Click (object sender, EventArgs e)
		{
			LessonFinished();
		}
	}
}