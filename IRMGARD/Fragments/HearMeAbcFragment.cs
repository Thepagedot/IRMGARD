
using System;
using System.Linq;

using Android.OS;
using Android.Views;
using Android.Widget;

using IRMGARD.Models;

namespace IRMGARD
{
    public class HearMeAbcFragment : LessonFragment<HearMeAbc>
    {
        ImageButton ibSpeakerABC;
        ImageButton ibSpeaker;
        GridView gridView;

        public override View OnCreateView (LayoutInflater inflater, ViewGroup container, Bundle savedInstanceState)
        {
            // Prepare view
            var view = inflater.Inflate(Resource.Layout.HearMeAbc, container, false);
            ibSpeakerABC = view.FindViewById<ImageButton>(Resource.Id.ibSpeakerABC);
            ibSpeakerABC.Click += ((e, sender) => PlayABCSong());
            ibSpeaker = view.FindViewById<ImageButton>(Resource.Id.ibSpeaker);
            ibSpeaker.Click += ((e, sender) => PlayTaskDesc());
            gridView = view.FindViewById<GridView>(Resource.Id.gridview);
            gridView.ItemClick += GridView_ItemClick;

            // Initialize iteration
            InitIteration();

            return view;
        }

        protected override void InitIteration()
        {
            base.InitIteration();

            var currentIteration = GetCurrentIteration<HearMeAbcIteration>();
            gridView.Adapter = new HearMeAbcAdapter(Activity.BaseContext, 0, currentIteration.Letters);

            if (Lesson.Iterations.IndexOf(currentIteration) > 0)
            {
                PlayTaskDesc();
            }

            FireUserInteracted(true);
        }

        void PlayABCSong()
        {
            SoundPlayer.PlaySound(Activity.BaseContext, Lesson.SoundPathABC);
        }

        void PlayTaskDesc()
        {
            SoundPlayer.PlaySound(Activity.BaseContext, GetCurrentIteration<HearMeAbcIteration>().SoundPath);
        }

        void GridView_ItemClick (object sender, AdapterView.ItemClickEventArgs e)
        {
            var hearMeAbcLetter = GetCurrentIteration<HearMeAbcIteration>().Letters.ElementAt(e.Position);
            SoundPlayer.PlaySound(Activity.BaseContext, hearMeAbcLetter.Media.SoundPath);
        }

        public override void CheckSolution()
        {
            FinishIteration(true, false);
        }
    }
}

