
using System;
using System.Collections.Generic;
using System.Linq;

using Android.OS;
using Android.Views;
using Android.Widget;
using IRMGARD.Models;

namespace IRMGARD
{	
	public class FourPicturesFragment : LessonFragment
	{
        private FourPictures lesson;
        private List<FourPicturesOption> options;
        private List<FourPicturesOption> currentOptions;
        private List<FourPicturesIteration> iterations;
        private int currentIterationIndex;

		private GridView gvFourPictures;
		private TextView tvLetter;
		private ImageButton btnCheck;

		public FourPicturesFragment (Lesson lesson)
		{
            // Convert lesson to according sub type
			this.lesson = lesson as FourPictures;
			if (lesson == null)
				throw new NotSupportedException("Wrong lesson type.");
			
            // Convert iterations
			iterations = new List<FourPicturesIteration>();
			foreach (var iteration in lesson.Iterations) 
			{
				if (iteration is FourPicturesIteration)
					iterations.Add(iteration as FourPicturesIteration);
			}				

            // Set rest of properties
            this.options = this.lesson.Options;
            this.currentOptions = new List<FourPicturesOption>();
            currentIterationIndex = 0;
		}

		public override View OnCreateView (LayoutInflater inflater, ViewGroup container, Bundle savedInstanceState)
		{
            // Prepare view
			var view = inflater.Inflate(Resource.Layout.FourPictures, container, false);
			gvFourPictures = view.FindViewById<GridView>(Resource.Id.gvFourPictures);
			gvFourPictures.ItemClick += GvFourPictures_ItemClick;
			tvLetter = view.FindViewById<TextView>(Resource.Id.tvLetter);
			btnCheck = view.FindViewById<ImageButton>(Resource.Id.btnCheck);
			btnCheck.Click += BtnCheck_Click;		

            // Initialize iteration
			InitIteration();
			return view;
		}

		private void InitIteration()
		{
			var currentIteration = iterations.ElementAt(currentIterationIndex);
			currentOptions.Clear();

			// Choose a random correct option
            var correctOptions = options.Where(o => o.Letter.Equals(currentIteration.LettersToLearn.First(), StringComparison.InvariantCultureIgnoreCase)).ToList();
			var correctOption = correctOptions.ElementAt(new Random().Next(0, correctOptions.Count() - 1));
			correctOption.IsCorrect = true;
			currentOptions.Add(correctOption);

			// Choose three other false Options
			var random = new Random();
            var falseOptions = options.Where(o => !o.Letter.Equals(currentIteration.LettersToLearn.First(), StringComparison.InvariantCultureIgnoreCase)).ToList();
			currentOptions.Add(falseOptions.ElementAt(random.Next(0, falseOptions.Count() - 1)));
			currentOptions.Add(falseOptions.ElementAt(random.Next(0, falseOptions.Count() - 1)));
			currentOptions.Add(falseOptions.ElementAt(random.Next(0, falseOptions.Count() - 1)));

			// Randomize list
			currentOptions.Shuffle();

            // Fill view
			var medidaElementAdapter = new FourPicturesAdapter(Activity.BaseContext, 0, currentOptions);
			gvFourPictures.Adapter = medidaElementAdapter;
			tvLetter.Text = currentIteration.LettersToLearn.First();
			btnCheck.Enabled = false;
		}

		void GvFourPictures_ItemClick(object sender, AdapterView.ItemClickEventArgs e)
		{
			// Play Sound
			SoundPlayer.PlaySound(Activity.BaseContext, currentOptions.ElementAt(e.Position).Media.SoundPath);

			// Enable check button
			btnCheck.Enabled = e.Position >= 0;
		}

		void BtnCheck_Click(object sender, EventArgs e)
		{
			if (gvFourPictures.CheckedItemPosition >= 0)
			{
				var selectedItem = currentOptions.ElementAt(gvFourPictures.CheckedItemPosition);								
				if (selectedItem.IsCorrect) 
				{
					Toast.MakeText (Activity.BaseContext, "Rrrrichtiiig", ToastLength.Short).Show();
					if (currentIterationIndex == iterations.Count - 1) {
						// All iterations done. Finish lesson
						LessonFinished ();
					} else {
						currentIterationIndex++;
						InitIteration();
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