using System;
using Android.Widget;
using Android.Content;
using Android.Views;
using System.Collections.Generic;
using IRMGARD.Models;

namespace IRMGARD
{
    public class FindMissingLetterTaskItemAdapter : ArrayAdapter<FindMissingLetterTaskLetter>
    {
        private LayoutInflater layoutInflater;

        public FindMissingLetterTaskItemAdapter(Context context, int resourceId, List<FindMissingLetterTaskLetter> items) : base (context, resourceId, items)
        {
            layoutInflater = LayoutInflater.From(context);
        }

        public override View GetView(int position, Android.Views.View convertView, Android.Views.ViewGroup parent)
        {
            View view = convertView;
            if (view == null)
                view = layoutInflater.Inflate(Resource.Layout.FindMissingLetterTaskItem, null);

            view.FindViewById<TextView>(Resource.Id.tvLetter).Text = GetItem(position).Letter;

            if (!GetItem(position).IsSearched)
                view.FindViewById<LinearLayout>(Resource.Id.llLayout).Background = null;

            return view;
        }
    }
}