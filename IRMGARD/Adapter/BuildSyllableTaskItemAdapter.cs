using System;
using Android.Content;
using System.Collections.Generic;
using Android.Views;
using Android.Widget;
using IRMGARD.Models;
using System.Linq;

namespace IRMGARD
{
    public class BuildSyllableTaskItemAdapter : ArrayAdapter<BuildSyllableOption>
    {
        private LayoutInflater layoutInflater;
        private Boolean addMultiIcon;
        private List<BuildSyllableOption> items;

        public BuildSyllableTaskItemAdapter(Context context, int resourceId, List<BuildSyllableOption> items, Boolean addMultiIcon) : base (context, resourceId, items)
        {
            layoutInflater = LayoutInflater.From(context);
            this.addMultiIcon = addMultiIcon;
            this.items = items;
        }

        public override View GetView(int position, Android.Views.View convertView, Android.Views.ViewGroup parent)
        {
            View view = convertView;
            if (view == null)
                view = layoutInflater.Inflate(Resource.Layout.BuildSyllableLetterTaskItem, null);

            view.FindViewById<TextView>(Resource.Id.tvLetter).Text = GetItem(position).DraggedLetter;
            if (addMultiIcon && (position + 1 == items.Count))
                view.FindViewById<TextView>(Resource.Id.tvAddMultiIcon).Text = "+";

            return view;
        }
    }
}

