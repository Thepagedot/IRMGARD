using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

using Android.App;
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
        const int CountOfCardPairs = 8;

        private GridView gridView;
        private IList<MemoryOption> memoryCards = new List<MemoryOption>(CountOfCardPairs * 2);
        private IDictionary<string, FrameLayout> memoryCardsRevealed = new Dictionary<string, FrameLayout>();
        private IList<FrameLayout> memoryCardsMarked = new List<FrameLayout>();
        private int countOfPairsMatched;

        public MemoryFragment (Lesson lesson) : base(lesson) {}

        public override View OnCreateView(LayoutInflater inflater, ViewGroup container, Bundle savedInstanceState)
        {
            // Prepare view
            var view = inflater.Inflate(Resource.Layout.Memory, container, false);
            gridView = view.FindViewById<GridView>(Resource.Id.gridview);
            gridView.ItemClick += GridView_ItemClick;

            // Initialize iteration
            InitIteration();

            return view;
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
                        var firstParentView = memoryCardsRevealed.Single().Value;
                        memoryCardsRevealed.Clear();
                        parentView.Background = Resources.GetDrawable(Resource.Drawable.rectangle_green);
                        firstParentView.Background = Resources.GetDrawable(Resource.Drawable.rectangle_green);
                        await Task.Delay(1500);
                        parentView.Visibility = ViewStates.Gone;
                        firstParentView.Visibility = ViewStates.Gone;
                        countOfPairsMatched++;
                        CheckSolution();
                    }
                    else
                    {
                        memoryCardsRevealed.Add(memoryCard.Name, parentView);
                    }
                }
                else
                {
                    foreach (var memoryCardRevealed in memoryCardsRevealed)
                    {
                        memoryCardRevealed.Value.Background = Resources.GetDrawable(Resource.Drawable.rectangle_red);
                        memoryCardsMarked.Add(memoryCardRevealed.Value);
                    }
                    await Task.Delay(1000);
                    foreach (var memoryCardMarked in memoryCardsMarked)
                    {
                        memoryCardMarked.Background = null;
                    }
                    memoryCardsMarked.Clear();
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

            memoryCards.Clear();
            memoryCardsRevealed.Clear();
            countOfPairsMatched = 0;

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

            // Add memory cards to adapter
            gridView.Adapter = new MemoryAdapter(Activity.BaseContext, 0, memoryCards);
        }

        public override void CheckSolution()
        {
            if (countOfPairsMatched == CountOfCardPairs)
            {
                FinishIteration(true);
            }
        }

        // public override void OnDestroy()
        // {
        //     base.OnDestroy();
        //     BitmapLoader.Instance.ReleaseCache();
        // }
    }
}
