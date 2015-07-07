using System;
using Android.Widget;
using Android.App;
using Android.Views;

namespace IRMGARD
{
	public class LevelAdapter : BaseAdapter
	{
		Activity context;
		string[] items;

		public LevelAdapter (Activity context, string[] items) : base()
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
				view = context.LayoutInflater.Inflate (Android.Resource.Layout.SimpleListItem1, null);

			view.FindViewById<TextView> (Android.Resource.Id.Text1).Text = items [position];

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

