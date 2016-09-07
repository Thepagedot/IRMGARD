using System;
using System.Collections.Generic;
using System.Linq;

namespace IRMGARD.Models
{
    // The common base for all derived concepts
    public abstract class Concept
    {
        // The following properies are excluded from comparison
        public bool IsSolution { get; set; }
        public bool IsOption { get; set; }
        public bool ActivateOnSuccess { get; set; }
        public bool ActivateOnMistake { get; set; }

        public virtual Concept DeepCopy()
        {
            Concept clone = (Concept)this.MemberwiseClone();
            clone.IsSolution = IsSolution;
            clone.IsOption = IsOption;
            clone.ActivateOnSuccess = ActivateOnSuccess;
            clone.ActivateOnMistake = ActivateOnMistake;

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

    public enum LetterTag { None, Short, Long };

    // The base for all text based concepts
    public abstract class BaseText : Concept, ISound
    {
        public string Text { get; set; }
        public List<LetterTag> LetterTags { get; set; }
        public string SoundPath { get; set; }

        // The following properies are excluded from comparison
        public List<List<int>> Highlights { get; set; }
        public string Color { get; set; }
        public bool ShowAsPlainText { get; set; }
        public int TextSize { get; set; }

        public override Concept DeepCopy()
        {
            BaseText clone = (BaseText)base.DeepCopy();
            clone.Text = Text != null ? String.Copy(Text) : null;
            if (LetterTags != null && LetterTags.Count > 0)
            {
                clone.LetterTags = new List<LetterTag>(LetterTags);
            }
            clone.SoundPath = SoundPath != null ? String.Copy(SoundPath) : null;

            if (Highlights != null && Highlights.Count > 0)
            {
                clone.Highlights = new List<List<int>>(Highlights);
            }
            clone.Color = Color != null ? String.Copy(Color) : null;
            clone.ShowAsPlainText = ShowAsPlainText;
            clone.TextSize = TextSize;

            return clone;
        }

        public override bool Equals(object obj)
        {
            if (obj == null || obj as BaseText == null) { return false; }
            var other = obj as BaseText;
            return base.Equals(obj)
                && (Text == other.Text)
                && (LetterTags == null && other.LetterTags == null
                    ? true
                    : (LetterTags != null && other.LetterTags != null
                        ? Enumerable.SequenceEqual(LetterTags, other.LetterTags)
                        : false))
                && (SoundPath == other.SoundPath);
        }

        public override int GetHashCode()
        {
            return base.GetHashCode()
                ^ (Text == null ? 0 : Text.GetHashCode())
                ^ (LetterTags == null ? 0 : LetterTags.GetHashCode())
                ^ (SoundPath == null ? 0 : SoundPath.GetHashCode());
        }
    }

    // A letter item
    public class Letter : BaseText { }

    // A syllable item
    public class Syllable : BaseText { }

    // A word item
    public class Word : BaseText { }

    // A sentence item
    public class Sentence : BaseText { }

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

    // A picture
    public class Picture : Concept, ISound
    {
        public string ImagePath { get; set; }
        public string SoundPath { get; set; }

        // The following properies are excluded from comparison
        public int Size { get; set; }

        public override Concept DeepCopy()
        {
            Picture clone = (Picture)base.DeepCopy();
            clone.ImagePath = ImagePath != null ? String.Copy(ImagePath) : null;
            clone.SoundPath = SoundPath != null ? String.Copy(SoundPath) : null;
            clone.Size = Size;

            return clone;
        }

        public override bool Equals(object obj)
        {
            if (obj == null || obj as Picture == null) { return false; }
            var other = obj as Picture;

            return base.Equals(obj)
                && (ImagePath == other.ImagePath)
                && (SoundPath == other.SoundPath);
        }

        public override int GetHashCode()
        {
            return base.GetHashCode()
                ^ (ImagePath == null ? 0 : ImagePath.GetHashCode())
                ^ (SoundPath == null ? 0 : SoundPath.GetHashCode());
        }
    }

    // A bit of space
    public class Space : Concept
    {
        public int Width { get; set; }
        public int Height { get; set; }

        public Space()
        {
            // Default Width: 15dp
            this.Width = 15;

            // Default Height: 15dp
            this.Height = 15;
        }
    }

    public class JavaObjectWrapper<T> : Java.Lang.Object
    {
        public T Obj { get; set; }
    }
}

