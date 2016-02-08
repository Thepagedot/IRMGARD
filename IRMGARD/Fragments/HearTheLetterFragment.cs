using Android.OS;
using Android.Views;
using Android.Widget;

using IRMGARD.Models;
using IRMGARD.Shared;

namespace IRMGARD
{
    public class HearTheLetterFragment : LessonFragment<HearTheLetter>
    {
        ImageButton ibSpeaker;
        SeekBar sbLetterPos;

        HearTheLetterOption currentOption;
        int selectedLocation;

        public HearTheLetterFragment(Lesson lesson) : base(lesson) {}

        public override View OnCreateView(LayoutInflater inflater, ViewGroup container, Bundle savedInstanceState)
        {
            View view = inflater.Inflate(Resource.Layout.HearTheLetter, container, false);
            ibSpeaker = view.FindViewById<ImageButton>(Resource.Id.ibSpeaker);
            ibSpeaker.Click += ((e, sender) => PlayTaskDesc());

            sbLetterPos = view.FindViewById<SeekBar>(Resource.Id.sbLetterPos);
            sbLetterPos.ProgressChanged += (object sender, SeekBar.ProgressChangedEventArgs e) => {
                selectedLocation = ConvertProgressToLocation(e.Progress);
            };

            InitIteration();

            return view;
        }

        private int ConvertProgressToLocation(int p)
        {
            return (p < 10) ? 0 : ((p <= 20) ? 1 : ((p <= 30) ? 2 : 0));
        }

        protected override void InitIteration()
        {
            base.InitIteration();
            selectedLocation = ConvertProgressToLocation(sbLetterPos.Progress);

            var currentIteration = GetCurrentIteration<HearTheLetterIteration>();
            currentOption = currentIteration.LetterLocations.PickRandomItems(1)[0];

            if (DataHolder.Current.CurrentLesson.Iterations.IndexOf(currentIteration) > 0)
            {
                PlayTaskDesc();
            }

            FireUserInteracted(true);
        }

        void PlayTaskDesc()
        {
            SoundPlayer.PlaySound(Activity.BaseContext, currentOption.SoundPath);
        }

        public override void CheckSolution()
        {
            FinishIteration(selectedLocation == currentOption.Location);
        }
    }
}

