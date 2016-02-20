using System;
using IRMGARD.Models;
using Android.Widget;
using Android.Views;
using Android.Content;
using System.Collections.Generic;

namespace IRMGARD
{
    public class PickSyllableAdapter : ArrayAdapter<PickSyllableOption>
    {
        public PickSyllableAdapter(Context context, int resourceId, List<PickSyllableOption> items) : base (context, resourceId, items)
        {
        }

        public override View GetView(int position, Android.Views.View convertView, Android.Views.ViewGroup parent)
        {
            return convertView ?? LayoutInflater.From(Context).Inflate(Resource.Layout.SpeakerElement, null);
        }
    }
}

