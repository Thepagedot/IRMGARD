using System;
using IRMGARD.Models;
using System.Collections.Generic;
using System.Linq;
using Android.Views;
using Android.OS;
using Android.Widget;
using Android.Graphics;
using Android.Content;

namespace IRMGARD
{
    public class BuildSyllableFragment : LessonFragment<BuildSyllable>
    {
        LinearLayout llTaskItems;
		ImageView ivBuildSyllable;
        FlowLayout flLetters;
        ImageButton btnCheck;

        public BuildSyllableFragment(Lesson lesson) : base(lesson) {}

        public override View OnCreateView (LayoutInflater inflater, ViewGroup container, Bundle savedInstanceState)
        {
            var view = inflater.Inflate(Resource.Layout.BuildSyllable, container, false);

			if (view != null) {
                ivBuildSyllable = view.FindViewById<ImageView> (Resource.Id.ivBuildSyllable);
                llTaskItems = view.FindViewById<LinearLayout>(Resource.Id.llTaskItems);
                flLetters = view.FindViewById<FlowLayout> (Resource.Id.flLetters);
                btnCheck = view.FindViewById<ImageButton>(Resource.Id.btnCheck);
                btnCheck.Click += BtnCheck_Click;
			}

            InitIteration ();
            return view;
        }

        protected override void InitIteration()
        {
            var bitmap = BitmapFactory.DecodeResource(Activity.BaseContext.Resources, Resource.Drawable.ic_volume_up_black_24dp);
            ivBuildSyllable.SetImageBitmap (bitmap);
            var currentIteration = GetCurrentIteration<BuildSyllableIteration>();

            BuildSyllableLetters(currentIteration.Syllables);

            var buildSyllableAdapter = new BuildSyllableAdapter(Activity.BaseContext, 0, currentIteration.Options);
			for (int i = 0; i < currentIteration.Options.Count; i++) {
				// Add letter to view
                var view = buildSyllableAdapter.GetView (i, null, null);
                var letter = currentIteration.Options.ElementAt (i).Letter;

				view.Touch += (sender, e) => {
					var data = ClipData.NewPlainText ("letter", letter);
					(sender as View).StartDrag (data, new View.DragShadowBuilder (sender as View), null, 0);
				};

				flLetters.AddView (view);
			}

            ivBuildSyllable.Click += PlaySoundOnImageClick;
            btnCheck.Enabled = false;
        }

        void PlaySoundOnImageClick (object sender, EventArgs e)
        {
            // Play Sound
            SoundPlayer.PlaySound(Activity.BaseContext, GetCurrentIteration<BuildSyllable>().SoundPath);// currentIteration.Syllables.First().SoundPath);

        }

        private void BuildSyllableLetters(List<Syllable> syllables)
        {
            llTaskItems.RemoveAllViews();
            var syllableItemsAdapter = new BuildSyllableTaskItemAdapter(Activity.BaseContext, 0, syllables.First().SyllableParts);
            for (int i = 0; i < syllables.First().SyllableParts.Count; i++)
            {
                var view = syllableItemsAdapter.GetView(i, null, null);
                view.Drag += View_Drag;    

                // Add letter to view
                llTaskItems.AddView(view);
            }
        }

        void View_Drag (object sender, View.DragEventArgs e)
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
                    break;

                    // Dragged element exits the drop zone
                case DragAction.Exited:                                       
                    break;

                    // Dragged element has been dropped at the drop zone
                case DragAction.Drop:
                    e.Handled = true;

                    // Try to get clip data
                    var data = e.Event.ClipData;
                    if (data != null)
                    {
                        var taskLetters = GetCurrentIteration<BuildSyllableIteration>().Syllables;
                        var draggedLetter = data.GetItemAt(0).Text;
                        var position = llTaskItems.IndexOfChild(sender as View);

                        taskLetters.First().SyllableParts.ElementAt(position).DraggedLetter = draggedLetter;

                        BuildSyllableLetters(taskLetters);
                    }

                    btnCheck.Enabled = true;
                    break;
            }
        }        

        void BtnCheck_Click (object sender, EventArgs e)
        {
            CheckSolution();
        }

        protected override void CheckSolution()
        {
            var isCorrect = true;
            var currentIteration = GetCurrentIteration<BuildSyllableIteration>();
            for(var i = 0; i < currentIteration.Syllables.First().SyllableParts.Count; i++ )
            {
                if (!currentIteration.Syllables.First().SyllableParts.ElementAt(i).Letter.Equals(currentIteration.Syllables.First().SyllableParts.ElementAt(i).DraggedLetter))
                    isCorrect = false;

                if (!isCorrect)
                    break;
            }

            if (isCorrect)
                FinishIteration();
            else
                Toast.MakeText(Activity.BaseContext, "Leider verloren", ToastLength.Short).Show();
        }
    }
}

