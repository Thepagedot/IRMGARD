
using System;
using System.Collections.Generic;
using System.Linq;

using Android.OS;
using Android.Views;
using Android.Widget;
using IRMGARD.Models;
using IRMGARD.Shared;
using Android.Support.V7.Widget;
using Android.Graphics;

namespace IRMGARD
{	
    public class FourPicturesFragment : LessonFragment<FourPictures>
	{
        private ImageView ivImage1;
        private ImageView ivImage2;
        private ImageView ivImage3;
        private ImageView ivImage4;
		private TextView tvLetter;
        private List<FourPicturesOption> currentOptions;
        private List<CardView> cards;
        private int selectedPosition = -1;

		public override View OnCreateView (LayoutInflater inflater, ViewGroup container, Bundle savedInstanceState)
		{
            // Prepare view
			var view = inflater.Inflate(Resource.Layout.FourPictures, container, false);

            ivImage1 = view.FindViewById<ImageView>(Resource.Id.ivImage1);
            ivImage2 = view.FindViewById<ImageView>(Resource.Id.ivImage2);
            ivImage3 = view.FindViewById<ImageView>(Resource.Id.ivImage3);
            ivImage4 = view.FindViewById<ImageView>(Resource.Id.ivImage4);
            ivImage1.Click += (sender, e) => Image_Click(sender, e, 0);
            ivImage2.Click += (sender, e) => Image_Click(sender, e, 1);
            ivImage3.Click += (sender, e) => Image_Click(sender, e, 2);
            ivImage4.Click += (sender, e) => Image_Click(sender, e, 3);

			tvLetter = view.FindViewById<TextView>(Resource.Id.tvLetter);	

            cards = new List<CardView>
            {
                view.FindViewById<CardView>(Resource.Id.card1),
                view.FindViewById<CardView>(Resource.Id.card2),
                view.FindViewById<CardView>(Resource.Id.card3),
                view.FindViewById<CardView>(Resource.Id.card4)
            };

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
            var correctOptions = Lesson.Options.Where(o => o.Letter.Equals(currentIteration.LettersToLearn.First(), StringComparison.InvariantCultureIgnoreCase)).ToList();
			var correctOption = correctOptions.ElementAt(new Random().Next(0, correctOptions.Count() - 1));
			correctOption.IsCorrect = true;
			currentOptions.Add(correctOption);

			// Choose three other false Options
            var falseOptions = 
                Lesson.Options.Where(o => 
                    !o.Letter.Equals(currentIteration.LettersToLearn.First(), StringComparison.InvariantCultureIgnoreCase) &&
                    !o.Letter.Equals(correctOption.Letter, StringComparison.InvariantCultureIgnoreCase)).ToList();
            
            foreach (var option in falseOptions)
                option.IsCorrect = false;

            currentOptions.AddRange(falseOptions.PickRandomItems(3));

			// Randomize list
			currentOptions.Shuffle();

            foreach (var card in cards)
                card.SetCardBackgroundColor(Color.White);

            // Fill view
            ivImage1.SetImageBitmap(BitmapLoader.Instance.LoadBitmap(4, Activity, currentOptions[0].Media.ImagePath));
            ivImage2.SetImageBitmap(BitmapLoader.Instance.LoadBitmap(4, Activity, currentOptions[1].Media.ImagePath));
            ivImage3.SetImageBitmap(BitmapLoader.Instance.LoadBitmap(4, Activity, currentOptions[2].Media.ImagePath));
            ivImage4.SetImageBitmap(BitmapLoader.Instance.LoadBitmap(4, Activity, currentOptions[3].Media.ImagePath));

			tvLetter.Text = currentIteration.LettersToLearn.First();
		}

        private void Image_Click (object sender, EventArgs e, int position)
        {
            selectedPosition = position;

            foreach (var card in cards)
                card.SetCardBackgroundColor(Color.White);
            
            cards[position].SetCardBackgroundColor(Resources.GetColor(Resource.Color.selected_background));

            SoundPlayer.PlaySound(Activity.BaseContext, currentOptions.ElementAt(position).Media.SoundPath);           

            FireUserInteracted(true);
        }

        public override void CheckSolution()
        {
            if (selectedPosition > -1)
            {
                FinishIteration(currentOptions.ElementAt(selectedPosition).IsCorrect);
            }
        }

        // public override void OnDestroy()
        // {
        //     base.OnDestroy();
        //     BitmapLoader.Instance.ReleaseCache();
        // }
	}
}