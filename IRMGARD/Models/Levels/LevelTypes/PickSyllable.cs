using System;
using System.Collections.Generic;

namespace IRMGARD.Models
{
	public class PickSyllable : Lesson
	{
		public string SyllableToLearn { get; set; }
		public List<string> SyllableParts { get; set; }
		public List<PickSyllableOption> Options { get; set; }


		public PickSyllable () {}

		public PickSyllable (string title, string soundPath, string hint, LevelType typeOfLevel, string syllableToLearn, List<string> syllableParts, List<PickSyllableOption> options) 
			: base (title, soundPath, hint, typeOfLevel)
		{
			this.SyllableToLearn = syllableToLearn;
			this.SyllableParts = syllableParts;
			this.Options = options;
		}
	}

	public class PickSyllableOption
	{
		public bool IsCorrect { get; set; }
		public Media Media  { get; set; }

		public PickSyllableOption () {}

		public PickSyllableOption (bool isCorrect, Media media)
		{
			this.IsCorrect = isCorrect;
			this.Media = media;
		}
	}
}
