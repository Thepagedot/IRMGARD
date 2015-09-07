using System;
using Android.Widget;
using Android.Content;
using Android.Views;
using System.Collections.Generic;
using IRMGARD.Models;

namespace IRMGARD
{
    public class TaskLetterAdapter : ArrayAdapter<TaskLetter>
    {
        private readonly LayoutInflater layoutInflater;

        public TaskLetterAdapter(Context context, int resourceId, IList<TaskLetter> items) : base (context, resourceId, items)
        {
            layoutInflater = LayoutInflater.From(context);
        }

        public override View GetView(int position, Android.Views.View convertView, Android.Views.ViewGroup parent)
        {
            var view = convertView ?? layoutInflater.Inflate(Resource.Layout.LetterTaskItem, null);

            view.FindViewById<TextView>(Resource.Id.tvLetter).Text = GetItem(position).Letter;

            if (!GetItem(position).IsSearched)
                view.FindViewById<LinearLayout>(Resource.Id.llLayout).Background = null;

            return view;
        }
    }
}