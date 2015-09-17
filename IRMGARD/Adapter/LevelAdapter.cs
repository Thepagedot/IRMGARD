using System;
using Android.Widget;
using Android.App;
using Android.Views;
using IRMGARD.Models;

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
			View view = convertView;
			if (view == null)
				view = context.LayoutInflater.Inflate (Android.Resource.Layout.SimpleListItem2, null);

			view.FindViewById<TextView> (Android.Resource.Id.Text1).Text = items[position].Name;
			view.FindViewById<TextView> (Android.Resource.Id.Text2).Text = "This level has " + items[position].Modules.Count + " modules";
			view.SetBackgroundColor(Android.Graphics.Color.ParseColor (items [position].Color));
            if (!items[position].IsEnabled)
                view.Alpha = (float)0.5;
            
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

