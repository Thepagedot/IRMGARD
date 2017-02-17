using System;
using Android.Widget;
using Android.App;
using Android.Views;
using IRMGARD.Models;
using Android.Graphics;

namespace IRMGARD
{
	public class LevelAdapter : BaseAdapter
	{
		Activity context;
		Level[] items;

		public LevelAdapter (Activity context, Level[] items) : base()
		{
			this.context = context;
			this.items = items;
		}

		#region implemented abstract members of BaseAdapter

		public override Java.Lang.Object GetItem (int position)
		{
			return null;
		}

		public override long GetItemId (int position)
		{
			return position;
		}

		public override Android.Views.View GetView (int position, Android.Views.View convertView, Android.Views.ViewGroup parent)
		{
			var view = convertView ?? context.LayoutInflater.Inflate (Android.Resource.Layout.SimpleListItem1, null);

            var text1 = view.FindViewById<TextView>(Android.Resource.Id.Text1);
            text1.Text = items[position].Name;
            text1.TextSize = 18;
            text1.Typeface = FontHelper.Get(context, FontHelper.Font.StandardBold);
            text1.SetPadding(text1.PaddingLeft, 38, text1.PaddingRight, 38);

            // Reduce alpha when not available
            if (!items[position].IsEnabled)
            {
                view.SetBackgroundColor(Android.Graphics.Color.WhiteSmoke);
                view.Alpha = (float)0.5;
            }
            else
            {
                view.SetBackgroundColor(Android.Graphics.Color.ParseColor(items[position].Color));
                view.Alpha = (float)1;
            }

            /*
            view.SetBackgroundColor(Android.Graphics.Color.ParseColor (items [position].Color));
            if (!items[position].IsEnabled)
                view.Alpha = (float)0.5;
            */

			return view;
		}

		public override int Count {
			get {
				return items.Length;
			}
		}

		#endregion
	}
}

