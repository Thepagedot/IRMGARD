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
        private List<PickSyllableIteration> iterations;

        public PickSyllableFragment(Lesson lesson)
        {
            this.lesson = lesson as PickSyllable;

            this.iterations = new List<PickSyllableIteration>();
            foreach (var iteration in lesson.Iterations)
            {
                if (iteration is PickSyllableIteration)
                {
                    this.iterations.Add(iteration as PickSyllableIteration);
                }
            }

            if (this.lesson == null)
                throw new NotSupportedException("Wrong lesson type.");
        }

        public override void OnCreate (Bundle savedInstanceState)
        {
            base.OnCreate (savedInstanceState);
        }

        public override View OnCreateView (LayoutInflater inflater, ViewGroup container, Bundle savedInstanceState)
        {
            var view = inflater.Inflate(Resource.Layout.PickSyllable, container, false);

            var tvPickSyllable = view.FindViewById<TextView>(Resource.Id.tvPickSyllable);

            foreach (var letter in this.iterations.First().SyllableParts)
            {
                tvPickSyllable.Text += letter;
                if (this.iterations.First().SyllableParts.IndexOf(letter) == 0)
                    tvPickSyllable.Text += "+";
            }

            tvPickSyllable.Text += " = " + this.iterations.First().SyllableToLearn;

            var syllableAdapter = new PickSyllableAdapter(Activity.BaseContext, 0, this.iterations.First().Options);
            var gvPickSyllable = view.FindViewById<GridView>(Resource.Id.gvPickSyllable);
            gvPickSyllable.Adapter = syllableAdapter;
            gvPickSyllable.ItemClick += GridViewSyllableClicked;

            return view;
        }

        void GridViewSyllableClicked (object sender, AdapterView.ItemClickEventArgs e)
        {
            var option = this.iterations.First().Options.ElementAt(e.Position);
            Toast.MakeText (Activity.BaseContext, option.Media.SoundPath, ToastLength.Short).Show();
        }
    }
}

