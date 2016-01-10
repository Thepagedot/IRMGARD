using System;
using Android.App;
using IRMGARD.Models;
using Android.Views;
using Android.Widget;
using System.Linq;
using Android.Graphics;

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
            var view = convertView ?? context.LayoutInflater.Inflate (Resource.Layout.ModuleItem, null);

            // Set name
            var tvName = view.FindViewById<TextView>(Resource.Id.tvName);
            tvName.Text = items[position].Name.ToUpper();
            var font = Typeface.CreateFromAsset(context.Assets, "Fonts/Garaje_53_Uni_Black.otf");
            tvName.Typeface = font;

            // Set background color
            view.SetBackgroundColor(Android.Graphics.Color.ParseColor (items [position].Color));

            // Set lesson indicators
            var llLessons = view.FindViewById<LinearLayout>(Resource.Id.llLessons);
            llLessons.RemoveAllViews();
            foreach (var lesson in items[position].Lessons)
            {
                var indicator = context.LayoutInflater.Inflate(Resource.Layout.ModuleProgressItem, null);
                if (lesson.IsCompleted)
                {
                    indicator.FindViewById<ImageView>(Resource.Id.ivChecked).Visibility = ViewStates.Visible;
                    indicator.FindViewById<ImageView>(Resource.Id.ivUnchecked).Visibility = ViewStates.Gone;
                }
                else
                {
                    indicator.FindViewById<ImageView>(Resource.Id.ivChecked).Visibility = ViewStates.Gone;
                    indicator.FindViewById<ImageView>(Resource.Id.ivUnchecked).Visibility = ViewStates.Visible;
                }
                    
                llLessons.AddView(indicator);
            }

            // Reduce alpha when not available
            if (!items[position].Lessons.Any() || (position > 0 && !items[position - 1].IsCompleted))
                view.Alpha = (float)0.5;
            else
                view.Alpha = (float)1;
            
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