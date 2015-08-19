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

        public BuildSyllableTaskItemAdapter(Context context, int resourceId, List<BuildSyllableOption> items) : base (context, resourceId, items)
        {
            layoutInflater = LayoutInflater.From(context);
        }

        public override View GetView(int position, Android.Views.View convertView, Android.Views.ViewGroup parent)
        {
            View view = convertView;
            if (view == null)
                view = layoutInflater.Inflate(Resource.Layout.FindMissingLetterTaskItem, null);

            view.FindViewById<TextView>(Resource.Id.tvLetter).Text = GetItem(position).DraggedLetter;
            return view;
        }
    }
}

