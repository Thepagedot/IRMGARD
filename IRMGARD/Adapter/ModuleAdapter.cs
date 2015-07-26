using System;
using Android.App;
using IRMGARD.Models;
using Android.Views;
using Android.Widget;

namespace IRMGARD
{
	public class ModuleAdapter : BaseAdapter
	{
		Activity context;
		Module[] items;

		public ModuleAdapter (Activity context, Module[] items) : base()
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
			view.FindViewById<TextView> (Android.Resource.Id.Text2).Text = "This level has " + items[position].Lessons.Count + " lessons";
			view.SetBackgroundColor(Android.Graphics.Color.ParseColor (items [position].Color));
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