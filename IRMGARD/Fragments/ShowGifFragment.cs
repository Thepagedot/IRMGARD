
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using IRMGARD.Models;

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

        private void BuildGifElements(List<GifTask> gifItems)
        {
            // Add task items to view and attach Drag and Drop handler
            llGifItems.RemoveAllViews();
            var adapter = new GifAdapter(Activity.BaseContext, 0, gifItems);
            for (var i = 0; i < gifItems.Count; i++)
            {
                var view = adapter.GetView(i, null, null);


                //view.Drag += View_Drag;

                llGifItems.AddView(view);
            }
        }


        public override void CheckSolution()
        {  
        }
    }
}

