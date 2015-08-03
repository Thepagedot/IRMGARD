using System;
using Android.Widget;
using Android.Content;
using Android.Views;
using System.Collections.Generic;

namespace IRMGARD
{
    public class FindMissingLetterTaskItemAdapter : ArrayAdapter<string>
    {
        private LayoutInflater layoutInflater;

        public FindMissingLetterTaskItemAdapter(Context context, int resourceId, List<string> items) : base (context, resourceId, items)
        {
            layoutInflater = LayoutInflater.From(context);
        }

        public override View GetView(int position, Android.Views.View convertView, Android.Views.ViewGroup parent)
        {
            View view = convertView;
            if (view == null)
                view = layoutInflater.Inflate(Resource.Layout.FindMissingLetterTaskItem, null);

            view.FindViewById<TextView>(Resource.Id.tvLetter).Text = GetItem(position);
            return view;
        }
    }
}