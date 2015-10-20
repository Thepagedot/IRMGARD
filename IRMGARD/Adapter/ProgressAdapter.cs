using System;
using Android.Widget;
using Android.Content;
using System.Collections.Generic;
using Android.Views;
using Android.Graphics.Drawables;
using Android.Support.V7.Widget;
using System.Linq;

namespace IRMGARD
{
    public class ProgressAdapter : RecyclerView.Adapter
    {
        List<Progress> items;

        public ProgressAdapter(List<Progress> items)
        {
            this.items = items;
        }            

        #region implemented abstract members of Adapter

        public override void OnBindViewHolder(RecyclerView.ViewHolder holder, int position)
        {
            var progress = items.ElementAt(position);
            ((ProgressViewHolder)holder).BindProgress(progress);
        }

        public override RecyclerView.ViewHolder OnCreateViewHolder(ViewGroup parent, int viewType)
        {
            var view = LayoutInflater.From(parent.Context).Inflate(Resource.Layout.ProgressItem, parent, false);
            return new ProgressViewHolder(view);
        }

        public override int ItemCount
        {
            get
            {
                return items.Count;
            }
        }

        #endregion
    }

    public class ProgressViewHolder : RecyclerView.ViewHolder
    {
        private ImageView imageView { get; set; }
        private Progress progress { get; set; }

        public ProgressViewHolder(View view) : base(view)
        {
            imageView = view.FindViewById<ImageView>(Resource.Id.ivStatus);
        }

        public void BindProgress(Progress progress)
        {
            switch (progress.Status)
            {
                case ProgressStatus.Success:
                    imageView.SetImageResource(Resource.Drawable.ic_check_box_black_24dp);
                    break;
                case ProgressStatus.Failed:
                    imageView.SetImageResource(Resource.Drawable.ic_indeterminate_check_box_black_24dp);
                    break;
                case ProgressStatus.Pending:
                    imageView.SetImageResource(Resource.Drawable.ic_check_box_outline_blank_black_24dp);
                    break;
            }
        }
    }
}