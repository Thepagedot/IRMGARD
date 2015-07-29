
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
		private List<FourPicturesOption> Options;
		private List<FourPicturesOption> CurrentOptions;
		private List<FourPicturesIteration> Iterations;
		private int CurrentIterationIndex;

		private GridView gvFourPictures;
		private TextView tvLetter;
		private ImageButton btnCheck;

		public FourPicturesFragment (Lesson lesson)
		{
			this.Lesson = lesson as FourPictures;
			if (Lesson == null)
				throw new NotSupportedException("Wrong lesson type.");
			
			this.Options = Lesson.Options;
			this.CurrentOptions = new List<FourPicturesOption>();

			Iterations = new List<FourPicturesIteration>();
			foreach (var iteration in lesson.Iterations) 
			{
				if (iteration is FourPicturesIteration)
					Iterations.Add(iteration as FourPicturesIteration);
			}				
		}

		public override void OnCreate (Bundle savedInstanceState)
		{
			base.OnCreate (savedInstanceState);
			CurrentIterationIndex = 0;
		}

		public override View OnCreateView (LayoutInflater inflater, ViewGroup container, Bundle savedInstanceState)
		{
			var view = inflater.Inflate(Resource.Layout.FourPictures, container, false);

			gvFourPictures = view.FindViewById<GridView>(Resource.Id.gvFourPictures);
			gvFourPictures.ItemClick += GvFourPictures_ItemClick;

			tvLetter = view.FindViewById<TextView>(Resource.Id.tvLetter);

			btnCheck = view.FindViewById<ImageButton>(Resource.Id.btnCheck);
			btnCheck.Click += BtnCheck_Click;		

			InitIteration ();

			return view;
		}

		private void InitIteration()
		{
			var currentIteration = Iterations.ElementAt (CurrentIterationIndex);
			CurrentOptions.Clear();

			// Choose a random correct option
			var correctOptions = Options.Where(o => o.Letter.Equals(currentIteration.LettersToLearn.First(), StringComparison.InvariantCultureIgnoreCase));
			var correctOption = correctOptions.ElementAt(new Random().Next(0, correctOptions.Count() - 1));
			correctOption.IsCorrect = true;
			CurrentOptions.Add(correctOption);

			// Choose three other false Options
			var random = new Random();
			var falseOptions = Options.Where(o => !o.Letter.Equals(currentIteration.LettersToLearn.First(), StringComparison.InvariantCultureIgnoreCase));
			CurrentOptions.Add(falseOptions.ElementAt(random.Next(0, falseOptions.Count() - 1)));
			CurrentOptions.Add(falseOptions.ElementAt(random.Next(0, falseOptions.Count() - 1)));
			CurrentOptions.Add(falseOptions.ElementAt(random.Next(0, falseOptions.Count() - 1)));

			// Randomize list
			CurrentOptions.Shuffle();

			var medidaElementAdapter = new FourPicturesAdapter(Activity.BaseContext, 0, CurrentOptions);
			gvFourPictures.Adapter = medidaElementAdapter;
			tvLetter.Text = currentIteration.LettersToLearn.First();
			btnCheck.Enabled = false;
		}

		void GvFourPictures_ItemClick (object sender, AdapterView.ItemClickEventArgs e)
		{
			// Play Sound
			SoundPlayer.PlaySound(Activity.BaseContext, CurrentOptions.ElementAt(e.Position).Media.SoundPath);

			// Enable check button
			btnCheck.Enabled = e.Position >= 0;
		}

		void BtnCheck_Click (object sender, EventArgs e)
		{
			if (gvFourPictures.CheckedItemPosition >= 0)
			{
				var selectedItem = CurrentOptions.ElementAt(gvFourPictures.CheckedItemPosition);								
				if (selectedItem.IsCorrect) 
				{
					Toast.MakeText (Activity.BaseContext, "Rrrrichtiiig", ToastLength.Short).Show ();
					if (CurrentIterationIndex == Iterations.Count - 1) {
						// All iterations done. Finish lesson
						LessonFinished ();
					} else {
						CurrentIterationIndex++;
						InitIteration ();
					}
				} 
				else
				{
					Toast.MakeText (Activity.BaseContext, "Leider verloren", ToastLength.Short).Show ();
				}
			}
		}
	}
}