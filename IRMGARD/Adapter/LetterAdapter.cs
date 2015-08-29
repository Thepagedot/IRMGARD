using System;
using IRMGARD.Models;
using Android.Widget;
using Android.Views;
using Android.Content;
using System.Collections.Generic;

namespace IRMGARD
{
    public class LetterAdapter : ArrayAdapter<LetterBase>
    {
        public LetterAdapter(Context context, int resourceId, IList<LetterBase> items) : base (context, resourceId, items)
        {
        }

        public override View GetView(int position, Android.Views.View convertView, Android.Views.ViewGroup parent)
        {
            View view = convertView;
            if (view == null)
                view = LayoutInflater.From(Context).Inflate(Resource.Layout.Letter, null);

            view.FindViewById<TextView>(Resource.Id.tvLetter).Text = GetItem(position).Letter;

            if (GetItem(position).IsShort)
                view.FindViewById(Resource.Id.shortIndicator).Visibility = ViewStates.Visible;
            if (GetItem(position).IsLong)
                view.FindViewById(Resource.Id.longIndicator).Visibility = ViewStates.Visible;

            return view;
        }
    }
}