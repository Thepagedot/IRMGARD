using System;
using System.Collections.Generic;

namespace IRMGARD.Models
{
    // The common base for all derived concepts
    public abstract class Concept
    {
        public bool Fixed { get; set; }

        public virtual Concept DeepCopy()
        {
            Concept clone = (Concept)this.MemberwiseClone();
            clone.Fixed = Fixed;
            return clone;
        }

        public override bool Equals(object obj)
        {
            if (obj == null || obj as Concept == null) { return false; }
            return true;
        }

        public override int GetHashCode()
        {
            return base.GetHashCode();
        }
    }

    // An item with sound
    public interface ISound
    {
        string SoundPath { get; set; }
    }

    // the base for all text based concepts
    public abstract class BaseText : Concept, ISound
    {
        public string Text { get; set; }
        public string SoundPath { get; set; }

        public override Concept DeepCopy()
        {
            BaseText clone = (BaseText)base.DeepCopy();
            clone.Text = Text != null ? String.Copy(Text) : null;
            clone.SoundPath = SoundPath != null ? String.Copy(SoundPath) : null;
            return clone;
        }

        public override bool Equals(object obj)
        {
            if (obj == null || obj as BaseText == null) { return false; }
            var other = obj as BaseText;
            return base.Equals(obj) && (Text == other.Text) && (SoundPath == other.SoundPath);
        }

        public override int GetHashCode()
        {
            return base.GetHashCode() ^ (Text == null ? 0 : Text.GetHashCode()) ^ (SoundPath == null ? 0 : SoundPath.GetHashCode());
        }
    }

    // A letter item
    public class Letter : BaseText { }

    // A syllable item
    public class Syllable : BaseText { }

    // A word item
    public class Word : BaseText { }

    // A sound only item
    public class Speaker : Concept, ISound
    {
        public string SoundPath { get; set; }

        public override Concept DeepCopy()
        {
            Speaker clone = (Speaker)base.DeepCopy();
            clone.SoundPath = SoundPath != null ? String.Copy(SoundPath) : null;
            return clone;
        }

        public override bool Equals(object obj)
        {
            if (obj == null || obj as Speaker == null) { return false; }
            var other = obj as Speaker;
            return base.Equals(obj) && (SoundPath == other.SoundPath);
        }

        public override int GetHashCode()
        {
            return base.GetHashCode() ^ (SoundPath == null ? 0 : SoundPath.GetHashCode());
        }
    }

    // An helper image (not embedded in a card view)
    public class Image : Concept
    {
        public string ImagePath { get; set; }

        public override Concept DeepCopy()
        {
            Image clone = (Image)base.DeepCopy();
            clone.ImagePath = ImagePath != null ? String.Copy(ImagePath) : null;
            return clone;
        }

        public override bool Equals(object obj)
        {
            if (obj == null || obj as Image == null) { return false; }
            var other = obj as Image;
            return base.Equals(obj) && (ImagePath == other.ImagePath);
        }

        public override int GetHashCode()
        {
            return base.GetHashCode() ^ (ImagePath == null ? 0 : ImagePath.GetHashCode());
        }
    }

    // A picture as a required part of the solution (embedded in a card view)
    public class Picture : Image, ISound
    {
        public string SoundPath { get; set; }

        public override Concept DeepCopy()
        {
            Picture clone = (Picture)base.DeepCopy();
            clone.SoundPath = SoundPath != null ? String.Copy(SoundPath) : null;
            return clone;
        }

        public override bool Equals(object obj)
        {
            if (obj == null || obj as Picture == null) { return false; }
            var other = obj as Picture;
            return base.Equals(obj) && (SoundPath == other.SoundPath);
        }

        public override int GetHashCode()
        {
            return base.GetHashCode() ^ (SoundPath == null ? 0 : SoundPath.GetHashCode());
        }
    }

    public class JavaObjectWrapper<T> : Java.Lang.Object
    {
        public T Obj { get; set; }
    }
}

