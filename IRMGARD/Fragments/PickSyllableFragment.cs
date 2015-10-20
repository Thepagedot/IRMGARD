using System;
using IRMGARD.Models;
using System.Collections.Generic;
using System.Linq;
using Android.OS;
using Android.Widget;
using Android.Views;
using Android.Content;
using Android.Graphics;

namespace IRMGARD
{
    public class PickSyllableFragment : LessonFragment<PickSyllable>
    {
        private List<PickSyllableOption> currentOptions;

        private FlowLayout flTaskItems;
        private LinearLayout llLayout;
        private TextView tvPickSyllable;
        private ImageButton btnCheck;
        private ImageView ivDropZone;
        private View originalView;
        private int correctPosition = -1;
        private bool isSoundPlayedForSelectedItem = false;

        public PickSyllableFragment(Lesson lesson) : base(lesson) {}

        public override void OnCreate (Bundle savedInstanceState)
        {
            base.OnCreate (savedInstanceState);
        }

        public override View OnCreateView (LayoutInflater inflater, ViewGroup container, Bundle savedInstanceState)
        {
            originalView = inflater.Inflate(Resource.Layout.PickSyllable, container, false);

            tvPickSyllable = originalView.FindViewById<TextView>(Resource.Id.tvPickSyllable);
            llLayout = originalView.FindViewById<LinearLayout>(Resource.Id.llLayout);
            llLayout.Drag += textViewDragZoneDrag;
            flTaskItems = originalView.FindViewById<FlowLayout>(Resource.Id.pickSyllableTaskItems);


            ivDropZone = originalView.FindViewById<ImageView>(Resource.Id.ivPickSyllableDropZone);

            btnCheck = originalView.FindViewById<ImageButton>(Resource.Id.btnCheck);
            btnCheck.Click += BtnCheck_Click;       

            InitIteration();


            return originalView;
        }

        protected override void InitIteration()
        {
            base.InitIteration();

            tvPickSyllable.Text = string.Empty;
            var currentIteration = GetCurrentIteration<PickSyllableIteration>();
            currentOptions = new List<PickSyllableOption>();

            // Choose a random correct option
            var correctOptions = lesson.Options.Where(o => o.Letter.Equals(currentIteration.SyllableParts.ElementAt(1), StringComparison.InvariantCultureIgnoreCase));
            var correctOption = correctOptions.ElementAt(new Random().Next(0, correctOptions.Count() - 1));
            correctOption.IsCorrect = true;
            currentOptions.Add(correctOption);

            // Choose three other false Options
            var falseOptions = lesson.Options.Where(o => !o.Letter.Equals(currentIteration.SyllableParts.ElementAt(1), StringComparison.InvariantCultureIgnoreCase)).ToList();

            falseOptions.Shuffle();
            currentOptions.AddRange(falseOptions.Take(3));

            // Randomize list
            currentOptions.Shuffle();

            correctPosition = currentOptions.IndexOf(currentOptions.Where(x => x.IsCorrect).FirstOrDefault());


            foreach (var letter in currentIteration.SyllableParts)
            {
                tvPickSyllable.Text += letter;
                if (currentIteration.SyllableParts.IndexOf(letter) == 0)
                    tvPickSyllable.Text += "+";
            }

            tvPickSyllable.Text += " = " + currentIteration.SyllableToLearn;
            correctOption.IsCorrect = false;

            BuildPickSyllableTaskItems(currentOptions);
        }

        void BuildPickSyllableTaskItems(List<PickSyllableOption> currentOptions)
        {
            var syllableAdapter = new PickSyllableAdapter(Activity.BaseContext, 0, currentOptions);

            flTaskItems.RemoveAllViews();
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
                flTaskItems.AddView(view);
            }
            btnCheck.Enabled = false;
        }



        void imageClickedForSound (PickSyllableOption item)
        {
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
                    // Dragged element enters the drop zone
                case DragAction.Entered:                   
                    //textViewDragZone.Text = "Drop it like it's hot!";
                    break;
                    // Dragged element exits the drop zone
                case DragAction.Exited:                   
                    //textViewDragZone.Text = "Drop something here!";
                    break;
                    // Dragged element has been dropped at the drop zone
                case DragAction.Drop:
                    // You can check if element may be dropped here
                    // If not do not set e.Handled to true
                    e.Handled = true;

                    // Try to get clip data
                    var data = e.Event.ClipData;
                    if (data != null)
                    {
                        selectedIndex = Convert.ToInt32(data.GetItemAt(0).Text);
                        var bitmap = BitmapFactory.DecodeResource(Activity.BaseContext.Resources, Resource.Drawable.ic_volume_up_black_24dp);

                        ivDropZone.Click -= DropZoneItemClicked;
                        ivDropZone.SetImageBitmap (bitmap);
                        ivDropZone.Click += DropZoneItemClicked;
                    }
                    btnCheck.Enabled = true;
                    break;
            }
        }

        private void DropZoneItemClicked(object sender, EventArgs e)
        {
            if (selectedIndex >= 0 && selectedIndex < currentOptions.Count)
                SoundPlayer.PlaySound(Activity.BaseContext, currentOptions.ElementAt(selectedIndex).Media.SoundPath);
        }

        protected override void CheckSolution()
        {                     
            if (selectedIndex > -1 && selectedIndex == correctPosition) 
            {                
                FinishIteration(true);

                var bitmap = BitmapFactory.DecodeResource(Activity.BaseContext.Resources, Resource.Drawable.ic_help_black_24dp);
                originalView.FindViewById<ImageView>(Resource.Id.ivPickSyllableDropZone).SetImageBitmap(bitmap);
                btnCheck.Enabled = false;
                currentOptions.Clear();
            } 
            else
            {
                FinishIteration(false);
            }
        }
    }
}

