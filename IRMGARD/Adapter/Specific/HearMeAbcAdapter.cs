using System;
using System.Collections.Generic;

using Android.Content;
using Android.Graphics;
using Android.Text;
using Android.Text.Style;
using Android.Views;
using Android.Widget;

using IRMGARD.Models;

namespace IRMGARD
{
    public class HearMeAbcAdapter : ArrayAdapter<HearMeAbcLetter>
    {
        public HearMeAbcAdapter(Context context, int resourceId, IList<HearMeAbcLetter> items) : base (context, resourceId, items)
        {
        }

        public override View GetView(int position, View convertView, ViewGroup parent)
        {
            var view = convertView;
            if (view == null)
            {
                view = LayoutInflater.From(Context).Inflate(Resource.Layout.HearMeAbcItem, null);

                var item = GetItem(position);
                var tvHearMeAbc = view.FindViewById<TextView>(Resource.Id.tvHearMeAbc);

                // Append colorized text
                if (!String.IsNullOrEmpty(item.Prepend))
                {
                    tvHearMeAbc.Append(GetTextColorized(item.Prepend, Color.LightGray));
                }
                if (!String.IsNullOrEmpty(item.Letter))
                {
                    if (String.IsNullOrEmpty(item.Prepend) && String.IsNullOrEmpty(item.Append))
                    {
                        tvHearMeAbc.Append(GetTextColorized(item.Letter, Color.Red));
                    }
                    else
                    {
                        tvHearMeAbc.Append(GetTextColorized(item.Letter, Color.Black));
                    }
                }
                if (!String.IsNullOrEmpty(item.Append))
                {
                    tvHearMeAbc.Append(GetTextColorized(item.Append, Color.LightGray));
                }
            }

            return view;
        }

        ISpannable GetTextColorized(string text, Color color)
        {
            var span = new SpannableString(text);
            if (text.Length > 4)
            {
                span.SetSpan(new TextAppearanceSpan(Context, Android.Resource.Style.TextAppearanceDeviceDefaultMedium), 0, text.Length, SpanTypes.ExclusiveExclusive);
            }
            span.SetSpan(new ForegroundColorSpan(color), 0, text.Length, SpanTypes.ExclusiveExclusive);

            return span;
        }
    }
}

