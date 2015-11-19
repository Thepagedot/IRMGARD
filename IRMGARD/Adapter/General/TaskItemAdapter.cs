using System;
using Android.Widget;
using Android.Content;
using Android.Views;
using System.Collections.Generic;
using IRMGARD.Models;
using Android.Support.V7.Widget;
using Android.Graphics;
using Android.Media;

namespace IRMGARD
{
    public class TaskItemAdapter : ArrayAdapter<TaskItem>
    {
        private readonly LayoutInflater layoutInflater;

        public TaskItemAdapter(Context context, int resourceId, IList<TaskItem> items) : base (context, resourceId, items)
        {
            layoutInflater = LayoutInflater.From(context);
        }

        public override View GetView(int position, Android.Views.View convertView, Android.Views.ViewGroup parent)
        {
            var item = GetItem(position);
            var view = convertView ?? layoutInflater.Inflate(Resource.Layout.TaskItem, null);

            if (item.Media != null)
            {
                var imageView = view.FindViewById<ImageView>(Resource.Id.image);
                imageView.SetImageBitmap(AssetHelper.GetBitmap(Context, item.Media.ImagePath));
                imageView.Visibility = ViewStates.Visible;
                view.FindViewById<TextView>(Resource.Id.letter).Visibility = ViewStates.Invisible;
            }
            else if (item.TaskLetter != null)
            {
                view.FindViewById<TextView>(Resource.Id.letter).Text = GetItem(position).TaskLetter.Letter;
            }

            if (item.IsSearched)
            {
                if (item.IsDirty)
                {
                    view.FindViewById<CardView>(Resource.Id.cardView).SetCardBackgroundColor(Color.White);
                    view.FindViewById<CardView>(Resource.Id.cardView).Elevation = 8f;
                    view.FindViewById<RelativeLayout>(Resource.Id.llLayout).Background = null;
                    view.FindViewById<View>(Resource.Id.underscore).Visibility = ViewStates.Invisible;
                }
            }
            else
            {
                view.FindViewById<RelativeLayout>(Resource.Id.llLayout).Background = null;
            }

            return view;
        }
    }
}