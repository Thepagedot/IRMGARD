using System;
using IRMGARD.Models;
using System.Collections.Generic;
using System.Linq;
using Android.OS;
using Android.Widget;
using Android.Views;
using Android.Content;
using Android.Graphics;
using Android.Graphics.Drawables;
using Android.Support.V7.Widget;
using IRMGARD.Shared;

namespace IRMGARD
{
    public class PickSyllableFragment : LessonFragment<PickSyllable>
    {
        private List<PickSyllableOption> currentOptions;

        private LinearLayout llTaskItems;
        private LinearLayout llLayout;
        private TextView tvPickSyllable;
        private ImageView ivDropZone;
        private CardView cvDropZone;

        private Bitmap emptyDropZoneImage;
        private Bitmap volumeUpDropZoneImage;

        private int correctPosition = -1;
        private bool isSoundPlayedForSelectedItem = false;

        public override View OnCreateView (LayoutInflater inflater, ViewGroup container, Bundle savedInstanceState)
        {
            // Prepare view
            var view = inflater.Inflate(Resource.Layout.PickSyllable, container, false);

            tvPickSyllable = view.FindViewById<TextView>(Resource.Id.tvPickSyllable);
            llLayout = view.FindViewById<LinearLayout>(Resource.Id.llLayout);
            llLayout.Drag += textViewDragZoneDrag;
            llTaskItems = view.FindViewById<LinearLayout>(Resource.Id.pickSyllableTaskItems);

            ivDropZone = view.FindViewById<ImageView>(Resource.Id.ivPickSyllableDropZone);
            cvDropZone = view.FindViewById<CardView>(Resource.Id.cardView);     


            emptyDropZoneImage = BitmapFactory.DecodeResource(Activity.BaseContext.Resources, Resource.Drawable.ic_help_black_24dp);
            volumeUpDropZoneImage = BitmapFactory.DecodeResource(Activity.BaseContext.Resources, Resource.Drawable.ic_volume_up_black_24dp);

            InitIteration();

            return view;
        }

        protected override void InitIteration()
        {
            base.InitIteration();

            // Reset drop zone
            ivDropZone.Click -= DropZoneItemClicked;
            ivDropZone.SetImageBitmap(emptyDropZoneImage);
            cvDropZone.SetCardBackgroundColor(Color.GetAlphaComponent(0));
            cvDropZone.CardElevation = 0;
            llLayout.Background = GetDrawable(Resource.Drawable.dotted);

            var currentIteration = GetCurrentIteration<PickSyllableIteration>();
            currentOptions = new List<PickSyllableOption>();

            // Get all syllable files in directory
            var syllableFilesAvail = AssetHelper.Instance.List(System.IO.Path.Combine(DataHolder.Current.Common.AssetSoundDir, Lesson.SyllablePath));

            // Pick a syllable for the current iteration
            var syllableToLearn = currentIteration.SyllablesToLearn.PickRandomItems(1).FirstOrDefault();

            // Choose a random correct option
            var syllableFiles = syllableFilesAvail.FindAll(f => f.StartsWith(String.Concat(syllableToLearn), StringComparison.InvariantCultureIgnoreCase));
            syllableFilesAvail.RemoveAll(f => f.StartsWith(String.Concat(syllableToLearn), StringComparison.InvariantCultureIgnoreCase));
            var syllableFile = syllableFiles.PickRandomItems(1).FirstOrDefault();
            currentOptions.Add(new PickSyllableOption(true, new Media(null, System.IO.Path.Combine(Lesson.SyllablePath, syllableFile))));
            
            // Choose three other false options
            foreach (var item in syllableFilesAvail.PickRandomItems(3))
            {
                currentOptions.Add(new PickSyllableOption(false, new Media(null, System.IO.Path.Combine(Lesson.SyllablePath, item))));
            }

            // Randomize list
            currentOptions.Shuffle();

            correctPosition = currentOptions.IndexOf(currentOptions.Where(x => x.IsCorrect).FirstOrDefault());

            // Prepare task description
            tvPickSyllable.Text = String.Empty;
            foreach (var part in syllableToLearn)
            {
                if (!String.IsNullOrEmpty(tvPickSyllable.Text))
                {
                    tvPickSyllable.Text += "+";
                }
                tvPickSyllable.Text += part.ToUpper();
            }
            tvPickSyllable.Text += " = " + String.Concat(syllableToLearn).ToUpper();

            BuildPickSyllableTaskItems(currentOptions);
        }

        void BuildPickSyllableTaskItems(List<PickSyllableOption> currentOptions)
        {
            var syllableAdapter = new PickSyllableAdapter(Activity.BaseContext, 0, currentOptions);

            llTaskItems.RemoveAllViews();
            for (int i = 0; i < syllableAdapter.Count; i++) {
                var view = syllableAdapter.GetView(i, null, null);
                var item = currentOptions.ElementAt(i);
                var index = currentOptions.IndexOf(item);

                view.Touch += (sender, e) => {
                    if (isSoundPlayedForSelectedItem == false)
                    {
                        imageClickedForSound(item); 
                        isSoundPlayedForSelectedItem = true;
                    }

                    using (var data = ClipData.NewPlainText("position", index.ToString()))
                    {
                        view.StartDrag(data, new View.DragShadowBuilder(view), null, 0);
                    }
                };
                llTaskItems.AddView(view);
            }
        }



        void imageClickedForSound (PickSyllableOption item)
        {
            if (SoundPlayer.IsPlaying)
                SoundPlayer.Stop();
            SoundPlayer.PlaySound(Activity.BaseContext, item.Media.SoundPath);
        }

        void BtnCheck_Click(object sender, EventArgs e)
        {
            CheckSolution();
        }

        private int selectedIndex = -1;

        void textViewDragZoneDrag (object sender, View.DragEventArgs e)
        {
            // React on different dragging events
            var evt = e.Event;
            switch (evt.Action) 
            {
                case DragAction.Ended:  
                    isSoundPlayedForSelectedItem = false;
                    break;
                case DragAction.Started:
                    e.Handled = true;
                    break;
                case DragAction.Drop:
                    e.Handled = true;

                    // Try to get clip data
                    var data = e.Event.ClipData;
                    if (data != null)
                    {
                        FireUserInteracted(true);

                        selectedIndex = Convert.ToInt32(data.GetItemAt(0).Text);

                        llLayout.Background = null;
                        cvDropZone.SetCardBackgroundColor(Color.White);
                        ivDropZone.SetImageBitmap(volumeUpDropZoneImage);
                        ivDropZone.Click += DropZoneItemClicked;
                    }
                    break;
            }
        }

        private void DropZoneItemClicked(object sender, EventArgs e)
        {
            if (selectedIndex >= 0 && selectedIndex < currentOptions.Count)
            {
                if (SoundPlayer.IsPlaying)
                    SoundPlayer.Stop();
                SoundPlayer.PlaySound(Activity.BaseContext, currentOptions.ElementAt(selectedIndex).Media.SoundPath);
            }
        }

        Drawable GetDrawable(int id)
        {
            return (Env.LollipopSupport) ? Resources.GetDrawable(id, Activity.BaseContext.Theme) : Resources.GetDrawable(id);
        }

        public override void CheckSolution()
        {                     
            if (selectedIndex > -1 && selectedIndex == correctPosition) 
            {                
                FinishIteration(true);
            } 
            else
            {
                FinishIteration(false);
            }
        }
    }
}

