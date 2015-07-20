using System;
using System.Collections.Generic;

namespace IRMGARD.Models
{
	public class FindMissingLetter : Lesson
	{
		public FindMissingLetter () {}

		public FindMissingLetter (string title, string soundPath, string hint, LevelType typeOfLevel, List<Iteration> iterations)  : base (title, soundPath, hint, typeOfLevel, iterations)
		{
		}
	}

    public class FindMissingLetterIteration : Iteration
    {
        public List<string> TaskLetters { get; set; }
        public List<FindMissingLetterOption> Options { get; set; }

        public FindMissingLetterIteration(List<string> lettersToLearn, List<string> taskLetters, List<FindMissingLetterOption> options): base(lettersToLearn)
        {
            this.TaskLetters = taskLetters;
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

