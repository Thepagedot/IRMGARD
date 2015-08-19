using System;
using IRMGARD.Models;
using System.Collections.Generic;
using System.Linq;
using Android.Views;
using Android.OS;
using Android.Widget;
using Android.Graphics;
using Android.Content;

namespace IRMGARD
{
    public class BuildSyllableFragment : LessonFragment
    {
        private BuildSyllable lesson;
        private List<BuildSyllableIteration> iterations;
        private int currentIterationIndex;
        private BuildSyllableIteration currentIteration;

		private ImageView ivBuildSyllable;
		private FlowLayout flLetters;

        public BuildSyllableFragment(Lesson lesson)
        {
            this.lesson = lesson as BuildSyllable;
            if (this.lesson == null)
                throw new NotSupportedException("Wrong lesson type.");


            this.iterations = new List<BuildSyllableIteration>();
            foreach (var iteration in lesson.Iterations)
            {
                if (iteration is BuildSyllableIteration)
                    this.iterations.Add(iteration as BuildSyllableIteration);
            }
        }

        public override void OnCreate (Bundle savedInstanceState)
        {
            base.OnCreate (savedInstanceState);
            currentIterationIndex = 0;
        }

        public override View OnCreateView (LayoutInflater inflater, ViewGroup container, Bundle savedInstanceState)
        {
            var view = inflater.Inflate(Resource.Layout.BuildSyllable, container, false);

			if (view != null) {
				ivBuildSyllable = view.FindViewById<ImageView> (Resource.Id.ivBuildSyllable);
				flLetters = view.FindViewById<FlowLayout> (Resource.Id.flLetters);
			}

            InitIteration ();


            return view;
        }

        private void InitIteration()
        {
            var bitmap = BitmapFactory.DecodeResource(Activity.BaseContext.Resources, Resource.Drawable.ic_volume_up_black_24dp);
            ivBuildSyllable.SetImageBitmap (bitmap);
			currentIteration = iterations.ElementAt (currentIterationIndex);

			var letterAdapter = new LetterAdapter(Activity.BaseContext, 0, currentIteration.Options);
			for (int i = 0; i < currentIteration.Options.Count; i++) {
				// Add letter to view
				var view = letterAdapter.GetView (i, null, null);
				var letter = iterations.ElementAt (currentIterationIndex).Options.ElementAt (i).Letter;

				view.Touch += (sender, e) => {
					var data = ClipData.NewPlainText ("letter", letter);
					(sender as View).StartDrag (data, new View.DragShadowBuilder (sender as View), null, 0);
				};

				flLetters.AddView (view);
			}

			ivBuildSyllable.Click += PlaySoundOnImageClick;
        }

        void PlaySoundOnImageClick (object sender, EventArgs e)
        {
            // Play Sound
            SoundPlayer.PlaySound(Activity.BaseContext, currentIteration.Syllables.First().SoundPath);

        }
    }
}

