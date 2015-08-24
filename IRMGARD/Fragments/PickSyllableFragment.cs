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

        private GridView gvPickSyllable;
        private LinearLayout llLayout;
        private TextView tvPickSyllable;
        private ImageButton btnCheck;
        private ImageView ivDropZone;
        private View originalView;
        private int correctPosition = -1;

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
            gvPickSyllable = originalView.FindViewById<GridView>(Resource.Id.gvPickSyllable);
            gvPickSyllable.ItemClick += GridViewSyllableClicked;


            ivDropZone = originalView.FindViewById<ImageView>(Resource.Id.ivPickSyllableDropZone);

            btnCheck = originalView.FindViewById<ImageButton>(Resource.Id.btnCheck);
            btnCheck.Click += BtnCheck_Click;       

            InitIteration();


            return originalView;
        }

        protected override void InitIteration()
        {
            tvPickSyllable.Text = string.Empty;
            var currentIteration = GetCurrentIteration<PickSyllableIteration>();
            currentOptions = new List<PickSyllableOption>();

            // Choose a random correct option
            var correctOptions = lesson.Options.Where(o => o.Letter.Equals(currentIteration.SyllableParts.ElementAt(1), StringComparison.InvariantCultureIgnoreCase));
            var correctOption = correctOptions.ElementAt(new Random().Next(0, correctOptions.Count() - 1));
            correctOption.IsCorrect = true;
            currentOptions.Add(correctOption);

            // Choose three other false Options
            var random = new Random();
            var falseOptions = lesson.Options.Where(o => !o.Letter.Equals(currentIteration.SyllableParts.ElementAt(1), StringComparison.InvariantCultureIgnoreCase));
            currentOptions.Add(falseOptions.ElementAt(random.Next(0, falseOptions.Count() - 1)));
            currentOptions.Add(falseOptions.ElementAt(random.Next(0, falseOptions.Count() - 1)));
            currentOptions.Add(falseOptions.ElementAt(random.Next(0, falseOptions.Count() - 1)));

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

            var syllableAdapter = new PickSyllableAdapter(Activity.BaseContext, 0, currentOptions);
            gvPickSyllable.Adapter = syllableAdapter;
            gvPickSyllable.ItemLongClick += imageViewLongClick;
            btnCheck.Enabled = false;
        }

        void GridViewSyllableClicked (object sender, AdapterView.ItemClickEventArgs e)
        {
            // Play Sound
            SoundPlayer.PlaySound(Activity.BaseContext, currentOptions.ElementAt(e.Position).Media.SoundPath);

            // Enable check button
            btnCheck.Enabled = e.Position >= 0;
        }

        void BtnCheck_Click(object sender, EventArgs e)
        {
            CheckSolution();
        }

        void imageViewLongClick(object sender, AdapterView.ItemLongClickEventArgs e)
        {
            // Generate clip data package to attach it to the drag
            using (var data = ClipData.NewPlainText("position", e.Position.ToString()))
            {
                View v = e.View;
                v.StartDrag(data, new View.DragShadowBuilder(v), null, 0);
            }
        }

        private int selectedIndex = -1;

        void textViewDragZoneDrag (object sender, View.DragEventArgs e)
        {
            // React on different dragging events
            var evt = e.Event;
            switch (evt.Action) 
            {
                case DragAction.Ended:  
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
                    break;
            }
        }

        private void DropZoneItemClicked(object sender, EventArgs e)
        {
            if (selectedIndex >= 0)
                SoundPlayer.PlaySound(Activity.BaseContext, currentOptions.ElementAt(selectedIndex).Media.SoundPath);
        }

        protected override void CheckSolution()
        {                     
            if (selectedIndex > -1 && selectedIndex == correctPosition) 
            {
                Toast.MakeText (Activity.BaseContext, "Rrrrichtiiig", ToastLength.Short).Show();
                FinishIteration();
            } 
            else
            {
                Toast.MakeText (Activity.BaseContext, "Leider verloren", ToastLength.Short).Show();
            }
        }
    }
}

