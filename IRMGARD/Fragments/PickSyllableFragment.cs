using System;
using IRMGARD.Models;
using System.Collections.Generic;
using System.Linq;
using Android.OS;
using Android.Widget;
using Android.Views;

namespace IRMGARD
{
    public class PickSyllableFragment : LessonFragment
    {
        private PickSyllable lesson;
        private List<PickSyllableOption> options;
        private List<PickSyllableOption> currentOptions;
        private List<PickSyllableIteration> iterations;
        private int currentIterationIndex;

        private GridView gvPickSyllable;
        private TextView tvPickSyllable;
        private ImageButton btnCheck;

        public PickSyllableFragment(Lesson lesson)
        {
            this.lesson = lesson as PickSyllable;
            if (this.lesson == null)
                throw new NotSupportedException("Wrong lesson type.");

            this.options = this.lesson.Options;
            this.currentOptions = new List<PickSyllableOption>();

            this.iterations = new List<PickSyllableIteration>();
            foreach (var iteration in lesson.Iterations)
            {
                if (iteration is PickSyllableIteration)
                    this.iterations.Add(iteration as PickSyllableIteration);
            }
        }

        public override void OnCreate (Bundle savedInstanceState)
        {
            base.OnCreate (savedInstanceState);
            currentIterationIndex = 0;
        }

        public override View OnCreateView (LayoutInflater inflater, ViewGroup container, Bundle savedInstanceState)
        {
            var view = inflater.Inflate(Resource.Layout.PickSyllable, container, false);

            tvPickSyllable = view.FindViewById<TextView>(Resource.Id.tvPickSyllable);
            gvPickSyllable = view.FindViewById<GridView>(Resource.Id.gvPickSyllable);
            gvPickSyllable.ItemClick += GridViewSyllableClicked;

            btnCheck = view.FindViewById<ImageButton>(Resource.Id.btnCheck);
            btnCheck.Click += BtnCheck_Click;       

            initIteration ();


            return view;
        }

        private void initIteration()
        {
            tvPickSyllable.Text = string.Empty;
            var currentIteration = iterations.ElementAt (currentIterationIndex);
            currentOptions.Clear();

            // Choose a random correct option
            var correctOptions = options.Where(o => o.Letter.Equals(currentIteration.SyllableParts.ElementAt(1), StringComparison.InvariantCultureIgnoreCase));
            var correctOption = correctOptions.ElementAt(new Random().Next(0, correctOptions.Count() - 1));
            correctOption.IsCorrect = true;
            currentOptions.Add(correctOption);

            // Choose three other false Options
            var random = new Random();
            var falseOptions = options.Where(o => !o.Letter.Equals(currentIteration.SyllableParts.ElementAt(1), StringComparison.InvariantCultureIgnoreCase));
            currentOptions.Add(falseOptions.ElementAt(random.Next(0, falseOptions.Count() - 1)));
            currentOptions.Add(falseOptions.ElementAt(random.Next(0, falseOptions.Count() - 1)));
            currentOptions.Add(falseOptions.ElementAt(random.Next(0, falseOptions.Count() - 1)));

            // Randomize list
            currentOptions.Shuffle();


            foreach (var letter in currentIteration.SyllableParts)
            {
                tvPickSyllable.Text += letter;
                if (currentIteration.SyllableParts.IndexOf(letter) == 0)
                    tvPickSyllable.Text += "+";
            }

            tvPickSyllable.Text += " = " + currentIteration.SyllableToLearn;

            var syllableAdapter = new PickSyllableAdapter(Activity.BaseContext, 0, currentOptions);
            gvPickSyllable.Adapter = syllableAdapter;
            btnCheck.Enabled = false;
        }

        void GridViewSyllableClicked (object sender, AdapterView.ItemClickEventArgs e)
        {
            // Play Sound
            SoundPlayer.PlaySound(Activity.BaseContext, currentOptions.ElementAt(e.Position).Media.SoundPath);

            // Enable check button
            btnCheck.Enabled = e.Position >= 0;
        }

        void BtnCheck_Click (object sender, EventArgs e)
        {
            if (gvPickSyllable.CheckedItemPosition >= 0)
            {
                var selectedItem = currentOptions.ElementAt(gvPickSyllable.CheckedItemPosition);                                
                if (selectedItem.IsCorrect) 
                {
                    Toast.MakeText (Activity.BaseContext, "Rrrrichtiiig", ToastLength.Short).Show ();
                    if (currentIterationIndex == iterations.Count - 1) {
                        // All iterations done. Finish lesson
                        LessonFinished ();
                    } else {
                        currentIterationIndex++;
                        initIteration ();
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

