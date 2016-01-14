using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Android.App;
using Android.Content;
using Android.OS;
using Android.Views;
using Android.Widget;

using IRMGARD.Models;
using IRMGARD.Shared;

namespace IRMGARD
{
    public class MemoryFragment : LessonFragment<Memory>
    {
        // Represents the amount of card pairs
        const int CountOfCardPairs = 6;

        private GridView gridView;
        private IList<MemoryOption> memoryCards = new List<MemoryOption>(CountOfCardPairs * 2);
        private IDictionary<string, FrameLayout> memoryCardsRevealed = new Dictionary<string, FrameLayout>();
        private int countOfPairsMatched;

        public MemoryFragment (Lesson lesson) : base(lesson) {}

        public override View OnCreateView(LayoutInflater inflater, ViewGroup container, Bundle savedInstanceState)
        {
            // Prepare view
            var view = inflater.Inflate(Resource.Layout.Memory, container, false);
            gridView = view.FindViewById<GridView>(Resource.Id.gridview);
            gridView.ItemClick += GridView_ItemClick;

            CreateMemoryCards();
            gridView.Adapter = new MemoryAdapter(Activity.BaseContext, 0, memoryCards);

            // Initialize iteration
            InitIteration();

            return view;
        }

        void CreateMemoryCards()
        {
            // Choose and initialize six random card pairs
            foreach (MemoryOption option in Lesson.Options.PickRandomItems<MemoryOption>(CountOfCardPairs))
            {
                memoryCards.Add(option);
                memoryCards.Add(new MemoryOption {
                    Name = option.Name,
                    Media = null,
                });
            }
            memoryCards.Shuffle();
        }

        async void GridView_ItemClick (object sender, AdapterView.ItemClickEventArgs e)
        {
            var parentView = (FrameLayout) e.View;

            MemoryOption memoryCard = memoryCards[e.Position];
            var cardBack = parentView.FindViewById<ImageView>(Resource.Id.ivCardBack);
            var cardPictureFront = parentView.FindViewById<ImageView>(Resource.Id.ivCardPictureFront);
            var cardTextFront = parentView.FindViewById<TextView>(Resource.Id.tvCardTextFront);

            if (cardBack.Visibility == ViewStates.Visible)
            {
                if (memoryCardsRevealed.Count < 2)
                {
                    cardBack.Visibility = ViewStates.Gone;
                    if (memoryCard.Media != null)
                    {
                        cardPictureFront.Visibility = ViewStates.Visible;
                        SoundPlayer.PlaySound(Activity.BaseContext, memoryCard.Media.SoundPath);
                    }
                    else
                    {
                        cardTextFront.Visibility = ViewStates.Visible;
                    }
                    if (memoryCardsRevealed.Count == 1 && memoryCardsRevealed.ContainsKey(memoryCard.Name)) {
                        Toast.MakeText(Activity.BaseContext, Resource.String.memory_cards_match, ToastLength.Short).Show();
                        var firstParentView = memoryCardsRevealed.Single().Value;
                        memoryCardsRevealed.Clear();
                        await Task.Delay(1500);
                        parentView.Visibility = ViewStates.Gone;
                        firstParentView.Visibility = ViewStates.Gone;
                        if (++countOfPairsMatched == CountOfCardPairs) {
                            CheckSolution();
                        }
                    }
                    else
                    {
                        memoryCardsRevealed.Add(memoryCard.Name, parentView);
                    }
                }
                else
                {
                    Toast.MakeText(Activity.BaseContext, Resource.String.memory_too_many_cards_rev, ToastLength.Short).Show();
                }
            }
            else
            {
                cardPictureFront.Visibility = ViewStates.Gone;
                cardTextFront.Visibility = ViewStates.Gone;
                cardBack.Visibility = ViewStates.Visible;
                memoryCardsRevealed.Remove(memoryCard.Name);
            }
        }

        protected override void InitIteration()
        {
            base.InitIteration();
        }

        public override void CheckSolution()
        {
            FinishIteration(true);
        }

        public override void OnDestroy()
        {
            base.OnDestroy();
            BitmapLoader.Instance.ReleaseCache();
        }
    }
}
