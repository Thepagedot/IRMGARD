using System;
using IRMGARD.Models;
using System.Collections.Generic;
using System.Linq;
using Android.OS;
using Android.Widget;
using Android.Views;
using Android.Content;

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
        private TextView textViewDragZone;
        private ImageButton btnCheck;
        private int correctPosition = -1;

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
            textViewDragZone = view.FindViewById<TextView>(Resource.Id.tvPickSyllableDragZone);
            textViewDragZone.Drag += textViewDragZoneDrag;
            gvPickSyllable = view.FindViewById<GridView>(Resource.Id.gvPickSyllable);
            gvPickSyllable.ItemClick += GridViewSyllableClicked;

            btnCheck = view.FindViewById<ImageButton>(Resource.Id.btnCheck);
            btnCheck.Click += BtnCheck_Click;       

            InitIteration ();


            return view;
        }

        private void InitIteration()
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
            if (gvPickSyllable.CheckedItemPosition >= 0)
            {
                var selectedItem = currentOptions.ElementAt(gvPickSyllable.CheckedItemPosition);                                
                if (selectedItem.IsCorrect) 
                {
                    Toast.MakeText (Activity.BaseContext, "Rrrrichtiiig", ToastLength.Short).Show();
                    if (currentIterationIndex == iterations.Count - 1) {
                        // All iterations done. Finish lesson
                        LessonFinished ();
                    } else {
                        currentIterationIndex++;
                        InitIteration ();
                    }
                } 
                else
                {
                    Toast.MakeText (Activity.BaseContext, "Leider verloren", ToastLength.Short).Show();
                }
            }
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
                        if (data.GetItemAt(0).Text.Equals(correctPosition.ToString()))
                            textViewDragZone.Text = data.GetItemAt(0).Text + " correct data.";
                        else
                            textViewDragZone.Text = data.GetItemAt(0).Text + " wrong data.";
                    }
                    break;
            }
        }
    }
}

