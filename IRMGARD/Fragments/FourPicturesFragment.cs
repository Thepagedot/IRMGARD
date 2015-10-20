
using System;
using System.Collections.Generic;
using System.Linq;

using Android.OS;
using Android.Views;
using Android.Widget;
using IRMGARD.Models;

namespace IRMGARD
{	
    public class FourPicturesFragment : LessonFragment<FourPictures>
	{
		private GridView gvFourPictures;
		private TextView tvLetter;
		private ImageButton btnCheck;
        private List<FourPicturesOption> currentOptions;

        public FourPicturesFragment (Lesson lesson) : base(lesson) {}

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

        protected override void InitIteration()
		{
            base.InitIteration();

            var currentIteration = GetCurrentIteration<FourPicturesIteration>();
            currentOptions = new List<FourPicturesOption>();

			// Choose a random correct option
            var correctOptions = lesson.Options.Where(o => o.Letter.Equals(currentIteration.LettersToLearn.First(), StringComparison.InvariantCultureIgnoreCase)).ToList();
			var correctOption = correctOptions.ElementAt(new Random().Next(0, correctOptions.Count() - 1));
			correctOption.IsCorrect = true;
			currentOptions.Add(correctOption);

			// Choose three other false Options
			var random = new Random();
            var falseOptions = lesson.Options.Where(o => !o.Letter.Equals(currentIteration.LettersToLearn.First(), StringComparison.InvariantCultureIgnoreCase)).ToList();
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
            CheckSolution();
		}

        protected override void CheckSolution()
        {
            if (gvFourPictures.CheckedItemPosition >= 0)
            {
                var selectedItem = currentOptions.ElementAt(gvFourPictures.CheckedItemPosition);                                
                if (selectedItem.IsCorrect) 
                {                    
                    FinishIteration(true);
                } 
                else
                {
                    FinishIteration(false);
                }
            }
        }
	}
}