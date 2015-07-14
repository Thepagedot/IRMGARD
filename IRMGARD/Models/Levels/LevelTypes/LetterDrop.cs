using System;
using System.Collections.Generic;

namespace IRMGARD.Models
{
	public class LetterDrop : Lesson
	{
		public List<string> LettersToLearn { get; set; }
		public List<string> Options { get; set; }

		public LetterDrop () {}

		public LetterDrop (string title, string soundPath, string hint, LevelType typeOfLevel, List<string> lettersToLearn, List<string> options) 
			: base(title, soundPath, hint, typeOfLevel)
		{
			this.LettersToLearn = lettersToLearn;
			this.Options = options;
		}
	}
}

