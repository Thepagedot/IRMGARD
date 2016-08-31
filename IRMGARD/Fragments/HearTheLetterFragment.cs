using System;
using Android.OS;
using Android.Views;
using Android.Widget;

using IRMGARD.Models;
using IRMGARD.Shared;

namespace IRMGARD
{
    public class HearTheLetterFragment : BaseConceptFragment<HearTheLetter>
    {
        ImageButton ibSpeaker;
        SeekBar sbLetterPos;
        RelativeLayout rlSliderLabels;

        HearTheLetterOption currentOption;
        int selectedLocation;

        public override View OnCreateView(LayoutInflater inflater, ViewGroup container, Bundle savedInstanceState)
        {
            View view = inflater.Inflate(Resource.Layout.HearTheLetter, container, false);
            ibSpeaker = view.FindViewById<ImageButton>(Resource.Id.ibSpeaker);
            ibSpeaker.Click += ((e, sender) => PlayTaskDesc());

            sbLetterPos = view.FindViewById<SeekBar>(Resource.Id.sbLetterPos);
            if (IsEven())
            {
                sbLetterPos.SetBackgroundResource(0);
            }
            sbLetterPos.ProgressChanged += (object sender, SeekBar.ProgressChangedEventArgs e) => {
                selectedLocation = ConvertProgressToLocation(e.Progress);
            };

            rlSliderLabels = view.FindViewById<RelativeLayout>(Resource.Id.rlSliderLabels);
            int align = 0;
            foreach (var label in Lesson.SliderLabels)
            {
                var labelView = CreateConceptView(label);
                rlSliderLabels.AddView(labelView);
                var lp = (labelView.LayoutParameters as RelativeLayout.LayoutParams);
                switch (align)
                {
                    case 0:
                        lp.AddRule(LayoutRules.AlignParentLeft);
                        break;
                    case 1:
                        lp.AddRule(LayoutRules.AlignParentRight);
                        break;
                    case 2:
                        lp.AddRule(LayoutRules.CenterInParent);
                        break;
                }
                align++;
            }

            InitIteration();

            return view;
        }

        private int ConvertProgressToLocation(int p)
        {
            return IsEven()
                ? (p <= 15) ? 0 : ((p <= 30) ? 1 : 0)
                : (p < 10) ? 0 : ((p <= 20) ? 1 : ((p <= 30) ? 2 : 0));
        }

        bool IsEven()
        {
            return (Lesson.SliderLabels == null || Lesson.SliderLabels.Count % 2 == 0);
        }

        protected override void InitIteration()
        {
            base.InitIteration();
            selectedLocation = ConvertProgressToLocation(sbLetterPos.Progress);

            var currentIteration = GetCurrentIteration<HearTheLetterIteration>();
            currentOption = currentIteration.LetterLocations.PickRandomItems(1)[0];

            if (Lesson.Iterations.IndexOf(currentIteration) > 0)
            {
                PlayTaskDesc();
            }

            FireUserInteracted(true);
        }

        void PlayTaskDesc()
        {
            if (SoundPlayer.IsPlaying)
                SoundPlayer.Stop();
            SoundPlayer.PlaySound(Activity.BaseContext, currentOption.SoundPath);
        }

        public override void CheckSolution()
        {
            FinishIteration(selectedLocation == currentOption.Location);
        }
    }
}

