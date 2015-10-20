using System;
using IRMGARD.Models;
using Android.Views;
using Android.OS;
using System.Collections.Generic;
using System.Linq;
using Android.Widget;
using Android.Content;

namespace IRMGARD
{
    public class AbcRankFragment : LessonFragment<AbcRank>
    {
        private FlowLayout flAbcRank;
        private LinearLayout llAbcRank;
        private List<AbcRankOption> currentOptionsSorted;
        private List<AbcRankOption> currentsolutionList;
        private List<AbcRankOption> randomized;
        private ImageButton btnCheck;
        private bool isSoundPlayedForSelectedItem = false;

        public AbcRankFragment(Lesson lesson) : base(lesson) {}

        public override View OnCreateView (LayoutInflater inflater, ViewGroup container, Bundle savedInstanceState)
        {
            View view = inflater.Inflate(Resource.Layout.AbcRank, container, false);

            if (view != null) {
                flAbcRank = view.FindViewById<FlowLayout> (Resource.Id.rankTaskItems);
                llAbcRank = view.FindViewById<LinearLayout>(Resource.Id.resolvedRankTaskItems);


                btnCheck = view.FindViewById<ImageButton>(Resource.Id.btnCheck);
                btnCheck.Click += BtnCheck_Click;
            }

            InitIteration ();
            return view;
        }

        #region implemented abstract members of LessonFragment

        protected override void InitIteration()
        {
            base.InitIteration();

            var currentIteration = GetCurrentIteration<AbcRankIteration>();

            currentsolutionList = new List<AbcRankOption>();
            BuildAbcRankTaskElements(currentIteration.Options);
        }

        void BuildAbcRankTaskElements(List<AbcRankOption> currentOptions)
        {
            randomized = new List<AbcRankOption>(currentOptions);
            randomized.Shuffle();

            /*if(randomized.Any(x => x.IsWithImage))
                randomized = randomized.Take(3).ToList();
            else
                randomized = randomized.Take(5).ToList();*/

            if (randomized.Any(x => x.Media != null))
                randomized = getDistinctLetterOptions(randomized, 3);
            else
                randomized = getDistinctLetterOptions(randomized, 5);
<<<<<<< HEAD
            
            var abcRankElementAdapter = new AbcRankAdapter(Activity.BaseContext, 0, randomized);
=======

            var abcRankElementAdapter = new AbcRankAdapter(Activity.BaseContext, 0, randomized, randomized.FirstOrDefault().IsWithImage);
>>>>>>> origin/master
            for (int i = 0; i < randomized.Count; i++) {
                // Add letter to view
                var view = abcRankElementAdapter.GetView (i, null, null);
                var item = randomized.ElementAt(i);

                if (item.Media != null)
                {
                    var imagePath = item.Media.ImagePath;

                    view.Touch += (sender, e) => {
                        if (isSoundPlayedForSelectedItem == false)
                        {
                            imageClickedForSound(item);
                            isSoundPlayedForSelectedItem = true;
                        }

                        var data = ClipData.NewPlainText ("ImagePath", imagePath);
                        (sender as View).StartDrag (data, new View.DragShadowBuilder (sender as View), null, 0);
                    };
                }
                else
                {
                    var name = item.Name;

                    view.Touch += (sender, e) => {
                        var data = ClipData.NewPlainText ("Name", name);
                        (sender as View).StartDrag (data, new View.DragShadowBuilder (sender as View), null, 0);
                    };
                }

                flAbcRank.AddView (view);
            }

            currentOptionsSorted = new List<AbcRankOption>(randomized);
            currentOptionsSorted = currentOptionsSorted.OrderBy(x => x.Name).ToList();

            BuildAbcRankSolutionElements(currentOptionsSorted);
        }

        List<AbcRankOption> getDistinctLetterOptions(List<AbcRankOption> randomized, int i)
        {
            List<AbcRankOption> tempList = new List<AbcRankOption>();
            int count = i;

            do
            {
                var item = randomized.ElementAt(0);
                if (tempList.Where(x => x.Name.Substring(0, 1).Equals(item.Name.Substring(0,1))).ToList().Count == 0)
                {
                    tempList.Add(item);
                    randomized.Shuffle();
                    count--;
                }
                else
                {
                    randomized.Shuffle();
                }

            } while(count > 0);


            tempList.Shuffle();
            return tempList;
        }

        void BuildAbcRankSolutionElements(List<AbcRankOption> currentOptionsSorted, bool createEmptyList = true)
        {
            if (createEmptyList)
                foreach (var item in currentOptionsSorted)
                    currentsolutionList.Add(new AbcRankOption());

            var abcRaknkSolutionElementAdapter = new AbcRankSolutionElementAdapter(Activity.BaseContext, 0, currentsolutionList);
            llAbcRank.RemoveAllViews();
            for (int i = 0; i < currentOptionsSorted.Count; i++) {
                // Add letter to view
                var view = abcRaknkSolutionElementAdapter.GetView (i, null, null);
                view.Drag += View_Drag;
                llAbcRank.AddView (view);
            }
        }

        void View_Drag (object sender, View.DragEventArgs e)
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
                        var position = llAbcRank.IndexOfChild(sender as View);
                        var draggedItem = currentOptionsSorted.ElementAt(position);

                        if (draggedItem.Media != null)
                        {
                            if (draggedItem.Media.ImagePath.Equals(data.GetItemAt(0).Text))
                            {
                                currentsolutionList.RemoveAt(position);
                                currentsolutionList.Insert(position, draggedItem);
                            }
                            else
                            {
                                currentsolutionList.RemoveAt(position);
                                currentsolutionList.Insert(position, new AbcRankOption());
                            }
                        }
                        else
                        {
                            if (draggedItem.Name.Equals(data.GetItemAt(0).Text))
                            {
                                currentsolutionList.RemoveAt(position);
                                currentsolutionList.Insert(position, draggedItem);
                            }
                            else
                            {
                                currentsolutionList.RemoveAt(position);
                                currentsolutionList.Insert(position, new AbcRankOption());
                            }
                        }

                        BuildAbcRankSolutionElements(currentsolutionList, false);
                    }

                    break;
            }
        }

        void imageClickedForSound (AbcRankOption item)
        {
            SoundPlayer.PlaySound(Activity.BaseContext, item.Media.SoundPath);
        }

        void BtnCheck_Click(object sender, EventArgs e)
        {
            CheckSolution();
        }

        protected override void CheckSolution()
        {
            bool isCorrect = true;
            foreach (var item in currentsolutionList)
            {
                if (string.IsNullOrEmpty(item.Name))
                {
                    isCorrect = false;
                    break;
                }
            }

            if (isCorrect)
            {
                FinishIteration(true);
            }
            else
            {
                FinishIteration(false);
            }
        }

        #endregion
    }
}

