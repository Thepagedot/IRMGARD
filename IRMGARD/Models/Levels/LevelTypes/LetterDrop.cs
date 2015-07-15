using System;
using System.Collections.Generic;

namespace IRMGARD.Models
{
	public class LetterDrop : ILesson
	{
		public string Title { get; set; }
		public string SoundPath { get; set; }
		public string Hint { get; set; }
		public LevelType TypeOfLevel { get; set; }
		public List<string> LettersToLearn { get; set; }
		public List<string> Options { get; set; }

		public LetterDrop () {}

		public LetterDrop (string title, string soundPath, string hint, LevelType typeOfLevel, List<string> lettersToLearn, List<string> options) 
		{
			this.LettersToLearn = lettersToLearn;
			this.Options = options;
		}
	}
}

