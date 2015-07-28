
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
	public class FourPicturesFragment : LessonFragment
	{
		private FourPictures Lesson;
		private List<FourPicturesIteration> Iterations;

		public FourPicturesFragment (Lesson lesson)
		{
			this.Lesson = lesson as FourPictures;

			Iterations = new List<FourPicturesIteration> ();
			foreach (var iteration in lesson.Iterations) {
				if (iteration is FourPicturesIteration)
					Iterations.Add(iteration as FourPicturesIteration);
			}

		

			if (Lesson == null)
				throw new NotSupportedException("Wrong lesson type.");
		}

		public override void OnCreate (Bundle savedInstanceState)
		{
			base.OnCreate (savedInstanceState);
		}

		public override View OnCreateView (LayoutInflater inflater, ViewGroup container, Bundle savedInstanceState)
		{
			var view = inflater.Inflate(Resource.Layout.FourPictures, container, false);

			var medidaElementAdapter = new FourPicturesAdapter(Activity.BaseContext, 0, Iterations.First().Options);
			var gvFourPivtures = view.FindViewById<GridView>(Resource.Id.gvFourPictures);
			gvFourPivtures.Adapter = medidaElementAdapter;
			gvFourPivtures.ItemClick += GvFourPivtures_ItemClick;
					
			return view;
		}

		void GvFourPivtures_ItemClick (object sender, AdapterView.ItemClickEventArgs e)
		{
			var option = Iterations.First ().Options.ElementAt (e.Position);
			if (option.IsCorrect) {
				Toast.MakeText (Activity.BaseContext, "Rrrrichtiiig", ToastLength.Short).Show ();
				LessonFinished();
			}
			else
				Toast.MakeText (Activity.BaseContext, "Leider verloren", ToastLength.Short).Show();
		}				
	}
}