
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using IRMGARD.Models;
using Felipecsl.GifImageViewLibrary;

using Android.App;
using Android.Content;
using Android.OS;
using Android.Runtime;
using Android.Util;
using Android.Views;
using Android.Widget;

namespace IRMGARD
{
    public class ShowGifFragment : LessonFragment<ShowGif>
    {
        private LinearLayout llGifItems;
        private Dictionary<int, bool> playedAllGifs = new Dictionary<int, bool>();

        public ShowGifFragment(Lesson lesson) : base(lesson) {}

        public override View OnCreateView(LayoutInflater inflater, ViewGroup container, Bundle savedInstanceState)
        {
            View view = inflater.Inflate(Resource.Layout.GifAnimation, container, false);
            llGifItems = view.FindViewById<LinearLayout>(Resource.Id.llGifItems);

            InitIteration();
            return view;
        }

        protected override void InitIteration()
        {
            base.InitIteration();

            var currentIteration = GetCurrentIteration<ShowGifIteration>();

            BuildGifElements(currentIteration.GifTasks);
        }

        private async void BuildGifElements(List<GifTask> gifItems)
        {
            // Add task items to view and attach Drag and Drop handler
            llGifItems.RemoveAllViews();
            playedAllGifs = new Dictionary<int, bool>();
            var adapter = new GifAdapter(Activity.BaseContext, 0, gifItems);
            for (var i = 0; i < gifItems.Count; i++)
            {
                playedAllGifs.Add(i, false);
                var view = adapter.GetView(i, null, null);
                var child = ((view as LinearLayout).GetChildAt(0) as GifImageView);
                if (child != null)
                {
                }
                view.Click += GifViewItemClicked;
                llGifItems.AddView(view);
            }
        }

        void GifViewItemClicked(object sender, EventArgs e)
        {
            var gifView = ((sender as LinearLayout).GetChildAt(0) as GifImageView);
            if (gifView != null)
            {
                if (gifView.IsAnimating)
                    gifView.StopAnimation();
                else 
                    gifView.StartAnimation();

                var index = llGifItems.IndexOfChild(sender as LinearLayout);
                playedAllGifs[index] = true;

                if (playedAllGifs.Where(x => x.Value == false).ToList().Count == 0)
                    FireUserInteracted(true);
                
                if (index == 0)
                    index++;
                else
                    index--;

                var nextChild = llGifItems.GetChildAt(index);
                if (nextChild != null && (nextChild as LinearLayout) != null)
                {
                    ((nextChild as LinearLayout).GetChildAt(0) as GifImageView).StopAnimation();
                }
            }
        }

        public override void CheckSolution()
        {  
            for (int i = 0; i < llGifItems.ChildCount; i++)
            {
                var child = llGifItems.GetChildAt(i) as LinearLayout;

                if (child == null)
                    continue;

                var gifView = child.GetChildAt(0) as GifImageView;

                if (gifView == null)
                    continue;

                if (gifView.IsAnimating)
                    gifView.StopAnimation();
            }

            FinishIteration(true, false);
        }
    }
}

