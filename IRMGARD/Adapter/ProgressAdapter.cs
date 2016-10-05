using System;
using Android.Widget;
using System.Collections.Generic;
using Android.Views;
using Android.Graphics;
using Android.Graphics.Drawables;
using Android.Support.V7.Widget;
using System.Linq;

using IRMGARD.Models;

namespace IRMGARD
{
    public class ProgressAdapter : RecyclerView.Adapter
    {
        readonly List<Progress> items;
        readonly Color levelColor;
        readonly Color moduleColor;
        readonly float displayDensity;

        public ProgressAdapter(List<Progress> items, Color levelColor, Color moduleColor, float displayDensity)
        {
            this.items = items;
            this.levelColor = levelColor;
            this.moduleColor = moduleColor;
            this.displayDensity = displayDensity;
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
            return new ProgressViewHolder(view, levelColor, moduleColor, displayDensity);
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
        readonly Drawable shapeProgress;
        readonly Color levelColor;
        readonly Bitmap shapeModuleColor;

        readonly int dp2;
        readonly int dp3;
        readonly int dp24;

        private ImageView ImageViewStatus { get; set; }

        public ProgressViewHolder(View view, Color levelColor, Color moduleColor, float displayDensity) : base(view)
        {
            dp2 = (int)(2 * displayDensity);
            dp3 = (int)(3 * displayDensity);
            dp24 = (int)(24 * displayDensity);

            shapeProgress = GetDrawable(view, Resource.Drawable.rectangle_progress);
            this.levelColor = levelColor;
            shapeModuleColor = CreateRect(moduleColor);

            ImageViewStatus = view.FindViewById<ImageView>(Resource.Id.ivStatus);
        }

        public void BindProgress(Progress progress)
        {
            switch (progress.Status)
            {
                case IterationStatus.Success:
                    ImageViewStatus.SetImageResource(Resource.Drawable.ic_check_box_black_24dp);
                    break;
                case IterationStatus.Failed:
                    ImageViewStatus.SetImageResource(Resource.Drawable.ic_indeterminate_check_box_black_24dp);
                    break;
                case IterationStatus.Pending:
                    ImageViewStatus.SetImageBitmap(shapeModuleColor);
                    break;
            }

            if (progress.IsCurrent)
            {
                ImageViewStatus.Background = shapeProgress;
                GradientDrawable backgroundGradient = (GradientDrawable)ImageViewStatus.Background;
                backgroundGradient.SetStroke(dp2, levelColor);
            }
            else
            {
                ImageViewStatus.Background = null;
            }
        }

        Bitmap CreateRect(Color color)
        {
            var bitmap = Bitmap.CreateBitmap(dp24, dp24, Bitmap.Config.Argb8888);
            var canvas = new Canvas(bitmap); 

            var rectShape = new ShapeDrawable();
            rectShape.Paint.Color = color;
            rectShape.Paint.SetPathEffect(new CornerPathEffect(dp3));
            rectShape.SetBounds(dp3, dp3, canvas.Width - dp3, canvas.Height - dp3);
            rectShape.Draw(canvas);

            return bitmap;
        }

        Drawable GetDrawable(View view, int id)
        {
            return (Env.LollipopSupport) ? view.Resources.GetDrawable(id, view.Context.Theme) : view.Resources.GetDrawable(id);
        }
    }
}