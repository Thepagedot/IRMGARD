
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
using System.Threading.Tasks;
using Android.Graphics;

namespace IRMGARD
{
    public class LetterWriteFragment : LessonFragment<LetterWrite>
    {
        private LinearLayout llGifItems;

        public override View OnCreateView(LayoutInflater inflater, ViewGroup container, Bundle savedInstanceState)
        {
            var view = inflater.Inflate(Resource.Layout.GifAnimation, container, false);
            llGifItems = view.FindViewById<LinearLayout>(Resource.Id.llGifItems);

            InitIteration();
            return view;
        }

        protected override void InitIteration()
        {
            base.InitIteration();

            var currentIteration = GetCurrentIteration<LetterWriteIteration>();

            BuildGifElements(currentIteration.TasksItems);
            FireUserInteracted(true);
        }

        private void BuildGifElements(List<LetterWriteTask> gifItems)
        {
            // Add task items to view and attach Drag and Drop handler
            llGifItems.RemoveAllViews();
            var adapter = new LetterWriteAdapter(Activity.BaseContext, 0, gifItems);
            for (var i = 0; i < gifItems.Count; i++)
            {
                // Start animation
                var view = adapter.GetView(i, null, null);
                var child = ((view as LinearLayout).GetChildAt(0) as GifImageView);
                child.StartAnimation();

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
            }
        }

        private void StopAllAnimations()
        {
            for (var i = 0; i < llGifItems.ChildCount; i++)
            {
                ((GifImageView)((LinearLayout)llGifItems.GetChildAt(i)).GetChildAt(0)).StopAnimation();
            }
        }

        public override void CheckSolution()
        {
            StopAllAnimations();            
            FinishIteration(true, false);
        }
 
        public override void OnStop()
        {
            base.OnStop();

            StopAllAnimations();
        }
    }
}

