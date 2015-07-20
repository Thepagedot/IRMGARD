using System;
using System.Collections.Generic;

namespace IRMGARD.Models
{
	public class FindMissingLetter : Lesson
	{
		public List<string> LettersToLearn { get; set; }
		public List<FindMissingLetterOption> Options { get; set; }

		public FindMissingLetter () {}

		public FindMissingLetter (string title, string soundPath, string hint, LevelType typeOfLevel, List<string> lettersToLearn, List<FindMissingLetterOption> options)  
			: base (title, soundPath, hint, typeOfLevel)
		{
			this.LettersToLearn = lettersToLearn;
			this.Options = options;
		}
	}

	public class FindMissingLetterOption
	{
		public string Letter { get; set; }
		public bool IsShort { get; set; }
		public bool IsLong { get; set; }
		public int CorrectPos { get; set; }

		public FindMissingLetterOption () {}

		public FindMissingLetterOption (string letter, bool isShort, bool isLong, int correctPos)
		{
			this.Letter = letter;
			this.IsShort = isShort;
			this.IsLong = isLong;
			this.CorrectPos = correctPos;
		}
	}
}

