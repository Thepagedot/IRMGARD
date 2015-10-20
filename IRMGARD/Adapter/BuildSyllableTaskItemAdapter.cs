using System;
using Android.Content;
using System.Collections.Generic;
using Android.Views;
using Android.Widget;
using IRMGARD.Models;
using System.Linq;

namespace IRMGARD
{
    public class BuildSyllableTaskItemAdapter : ArrayAdapter<TaskLetter>
    {
        private readonly bool addMultiIcon;

        public BuildSyllableTaskItemAdapter(Context context, int resourceId, List<TaskLetter> items, bool addMultiIcon) : base (context, resourceId, items)
        {
            this.addMultiIcon = addMultiIcon;
        }

        public override View GetView(int position, Android.Views.View convertView, Android.Views.ViewGroup parent)
        {
            var view = convertView ?? LayoutInflater.From(Context).Inflate(Resource.Layout.BuildSyllableLetterTaskItem, null);

            view.FindViewById<TextView>(Resource.Id.tvLetter).Text = GetItem(position).Letter;

            if (addMultiIcon && (position + 1 == Count))
                view.FindViewById<TextView>(Resource.Id.tvAddMultiIcon).Text = "+";

            return view;
        }
    }
}