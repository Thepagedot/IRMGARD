using System;
using System.Collections.Generic;

namespace IRMGARD.Models
{
	[Serializable]
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
		public LevelElement Element  { get; set; }

		public PickSyllableOption () {}

		public PickSyllableOption (bool isCorrect, LevelElement element)
		{
			this.IsCorrect = isCorrect;
			this.Element = element;
		}
	}
}