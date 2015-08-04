using System;
using IRMGARD.Models;
using Android.Widget;
using Android.Views;
using Android.Content;
using System.Collections.Generic;

namespace IRMGARD
{
    public class FindMissingLetterAdapter : ArrayAdapter<FindMissingLetterOption>
    {
        public FindMissingLetterAdapter(Context context, int resourceId, List<FindMissingLetterOption> items) : base (context, resourceId, items)
        {
         
        }

        public override View GetView(int position, Android.Views.View convertView, Android.Views.ViewGroup parent)
        {
            View view = convertView;
            if (view == null)
                view = LayoutInflater.From(Context).Inflate(Android.Resource.Layout.SimpleListItem1, null);

            view.FindViewById<TextView>(Android.Resource.Id.Text1).Text = GetItem(position).Letter;
            return view;
        }
    }
}