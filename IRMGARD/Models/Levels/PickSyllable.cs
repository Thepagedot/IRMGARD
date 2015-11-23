using System;
using System.Collections.Generic;

namespace IRMGARD.Models
{
	public class PickSyllable : Lesson
	{
        public List<PickSyllableOption> Options { get; set; }

		public PickSyllable () {}

        public PickSyllable (int id, string title, string soundPath, string hint, LevelType typeOfLevel, List<PickSyllableOption> options, List<Iteration> iterations) : base (id, title, soundPath, hint, typeOfLevel, iterations)
		{
            this.Options = options;
		}
	}

    public class PickSyllableIteration : Iteration
    {
        public string SyllableToLearn { get; set; }
        public List<string> SyllableParts { get; set; }

        public PickSyllableIteration(List<string> lettersToLearn, string syllableToLearn, List<string> syllableParts) : base (lettersToLearn)
        {
            SyllableToLearn = syllableToLearn;
            SyllableParts = syllableParts;
        }
    }

	public class PickSyllableOption
    {
        public string Letter { get; set; }
		public bool IsCorrect { get; set; }
		public Media Media  { get; set; }

		public PickSyllableOption () {}

        public PickSyllableOption (string letter, bool isCorrect, Media media)
        {
            this.Letter = letter;
            this.IsCorrect = isCorrect;
            this.Media = media;
        }

        public PickSyllableOption (string letter, Media media)
        {
            this.Letter = letter;
            this.Media = media;
        }

		public PickSyllableOption (bool isCorrect, Media media)
		{
			this.IsCorrect = isCorrect;
			this.Media = media;
		}
	}
}
