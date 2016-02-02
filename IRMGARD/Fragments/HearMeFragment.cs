
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

//using Square.Picasso;

namespace IRMGARD
{
    public class HearMeFragment : LessonFragment<HearMe>
	{
        private TextView letterToLearnView;
        private ImageView imageButtonView;
        private TextView nameView;

        public HearMeFragment (Lesson lesson) : base(lesson) {}

		public override View OnCreateView (LayoutInflater inflater, ViewGroup container, Bundle savedInstanceState)
		{
            View view = inflater.Inflate(Resource.Layout.HearMe, container, false);

            if (view != null)
            {
                letterToLearnView = view.FindViewById<TextView>(Resource.Id.hearMeLetterToLearn);
                imageButtonView = view.FindViewById<ImageView>(Resource.Id.hearMeImageView);
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
            imageButtonView.Click -= PlayImageSound;

            if (currentIteration != null)
            {
                foreach (string letter in currentIteration.LettersToLearn)
                {
                    letterToLearnView.Text += letter.ToUpper() + letter.ToLower();
                }
//                Picasso.With(Activity.BaseContext).Load("file:///android_asset/Images/"+currentIteration.Media.ImagePath).Into(imageButtonView);
                imageButtonView.SetImageBitmap(BitmapLoader.Instance.LoadBitmap(0, Activity.BaseContext, currentIteration.Media.ImagePath));
                imageButtonView.Click += PlayImageSound;

                nameView.Text = currentIteration.Name;
            }
        }

        void PlayImageSound(object sender, EventArgs e)
        {
            FireUserInteracted(true);
            SoundPlayer.PlaySound(Activity.BaseContext, GetCurrentIteration<HearMeIteration>().Media.SoundPath);
        }

        public override void CheckSolution()
        {
            FinishIteration(true, false);
        }            
	}
}