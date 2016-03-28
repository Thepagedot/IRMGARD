
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
using Android.Support.V7.Widget;
using Android.Graphics;
using IRMGARD.Models;

namespace IRMGARD
{
    public class HearMeFragment : LessonFragment<HearMe>
	{
        private TextView letterToLearnView;
        private ImageView imageButtonView;
        private CardView cardView;
        private TextView nameView;

		public override View OnCreateView (LayoutInflater inflater, ViewGroup container, Bundle savedInstanceState)
		{
            View view = inflater.Inflate(Resource.Layout.HearMe, container, false);

            if (view != null)
            {
                letterToLearnView = view.FindViewById<TextView>(Resource.Id.hearMeLetterToLearn);
                imageButtonView = view.FindViewById<ImageView>(Resource.Id.hearMeImageView);
                cardView = view.FindViewById<CardView>(Resource.Id.cardView);
                nameView = view.FindViewById<TextView>(Resource.Id.hearMeName);
            }

            InitIteration();

			return view;
        }

        protected override void InitIteration()
        {
            base.InitIteration();

            var currentIteration = GetCurrentIteration<HearMeIteration>();
            letterToLearnView.Text = string.Empty;
            nameView.Text = string.Empty;
            cardView.SetCardBackgroundColor(Color.White);
            imageButtonView.Click -= ImageButton_Click;

            if (currentIteration != null)
            {
                foreach (string letter in currentIteration.LettersToLearn)
                {
                    letterToLearnView.Text += letter.ToUpper() + letter.ToLower();
                }
                imageButtonView.SetImageBitmap(BitmapLoader.Instance.LoadBitmap(1, Activity.BaseContext, currentIteration.Media.ImagePath));
                imageButtonView.Click += ImageButton_Click;

                nameView.Text = currentIteration.Name;
            }
        }

        void ImageButton_Click(object sender, EventArgs e)
        {
            cardView.SetCardBackgroundColor(Resources.GetColor(Resource.Color.selected_background));

            if (SoundPlayer.IsPlaying)
                SoundPlayer.Stop();
            SoundPlayer.PlaySound(Activity.BaseContext, GetCurrentIteration<HearMeIteration>().Media.SoundPath);

            FireUserInteracted(true);
        }

        public override void CheckSolution()
        {
            FinishIteration(true, false);
        }            
	}
}