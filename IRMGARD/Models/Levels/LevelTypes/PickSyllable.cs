using System;
using System.Collections.Generic;

namespace IRMGARD.Models
{
	public class PickSyllable : ILesson
	{
		public string Title { get; set; }
		public string SoundPath { get; set; }
		public string Hint { get; set; }
		public LevelType TypeOfLevel { get; set; }
		public string SyllableToLearn { get; set; }
		public List<string> SyllableParts { get; set; }
		public List<PickSyllableOption> Options { get; set; }

<<<<<<< Upstream, based on origin/master
		public PickSyllable () {}
=======

		public PickSyllable (){}
>>>>>>> 2e23853 now lessons can be serialized as there original type

		public PickSyllable (string title, string soundPath, string hint, LevelType typeOfLevel, string syllableToLearn, List<string> syllableParts, List<PickSyllableOption> options) 
		{
			this.Title = title;
			this.SoundPath = soundPath;
			this.Hint = hint;
			this.TypeOfLevel = typeOfLevel;
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
