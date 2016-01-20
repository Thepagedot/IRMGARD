using Android.Graphics;
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
        ImageView ivHeadImage;
        ImageView ivBodyImage;
        ImageView ivTailImage;

        HearTheLetterOption currentOption;
        int selectedLocation;

        public HearTheLetterFragment() {}
        public HearTheLetterFragment(Lesson lesson) : base(lesson) {}

        public override View OnCreateView(LayoutInflater inflater, ViewGroup container, Bundle savedInstanceState)
        {
            View view = inflater.Inflate(Resource.Layout.HearTheLetter, container, false);
            ibSpeaker = view.FindViewById<ImageButton>(Resource.Id.ibSpeaker);
            ibSpeaker.Click += ((e, sender) => PlayTaskDesc());
            InitClickImage(view);

            InitIteration();

            return view;
        }

        void InitClickImage(View view)
        {
            using (var stream = Activity.Assets.Open(Lesson.ClickImagePath))
            {
                BitmapRegionDecoder brd = BitmapRegionDecoder.NewInstance(stream, false);
                Bitmap bmpHead = brd.DecodeRegion(new Rect(0, 195, 332, 1000), null);
                Bitmap bmpBody = brd.DecodeRegion(new Rect(333, 195, 666, 1000), null);
                Bitmap bmpTail = brd.DecodeRegion(new Rect(667, 195, 1000, 1000), null);

                ivHeadImage = view.FindViewById<ImageView>(Resource.Id.ivHeadImage);
                ivHeadImage.SetImageBitmap(bmpHead);
                ivHeadImage.Click += ((sender, e) => ClickImage_Click(0, new float[] { 1, 0.6f, 0.6f }));

                ivBodyImage = view.FindViewById<ImageView>(Resource.Id.ivBodyImage);
                ivBodyImage.SetImageBitmap(bmpBody);
                ivBodyImage.Click += ((sender, e) => ClickImage_Click(1, new float[] { 0.6f, 1, 0.6f }));

                ivTailImage = view.FindViewById<ImageView>(Resource.Id.ivTailImage);
                ivTailImage.SetImageBitmap(bmpTail);
                ivTailImage.Click += ((sender, e) => ClickImage_Click(2, new float[] { 0.6f, 0.6f, 1 }));
            }
        }

        protected override void InitIteration()
        {
            base.InitIteration();
            selectedLocation = -1;
            ivHeadImage.Alpha = ivBodyImage.Alpha = ivTailImage.Alpha = 1;

            var currentIteration = GetCurrentIteration<HearTheLetterIteration>();
            currentOption = currentIteration.LetterLocations.PickRandomItems(1)[0];

            if (DataHolder.Current.CurrentLesson.Iterations.IndexOf(currentIteration) == 0)
            {
                PlayLessonDesc();
            }
            else
            {
                PlayTaskDesc();
            }
        }

        void PlayLessonDesc()
        {
            SoundPlayer.PlaySound(Activity.BaseContext, Lesson.SoundPath);
        }

        void PlayTaskDesc()
        {
            SoundPlayer.PlaySound(Activity.BaseContext, currentOption.SoundPath);
        }

        void ClickImage_Click(int location, float[] alphaValues)
        {
            ivHeadImage.Alpha = alphaValues[0];
            ivBodyImage.Alpha = alphaValues[1];
            ivTailImage.Alpha = alphaValues[2];

            selectedLocation = location;
            FireUserInteracted(true);
        }

        public override void CheckSolution()
        {
            FinishIteration(selectedLocation == currentOption.Location);
        }
    }
}

