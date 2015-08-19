using System;
using System.Collections.Generic;

namespace IRMGARD.Models
{
	public class FindMissingLetter : Lesson
	{
		public FindMissingLetter () {}

		public FindMissingLetter (int id, string title, string soundPath, string hint, LevelType typeOfLevel, List<Iteration> iterations)  : base (id, title, soundPath, hint, typeOfLevel, iterations)
		{
		}
	}

    public class FindMissingLetterIteration : Iteration
    {
        public List<FindMissingLetterTaskLetter> TaskLetters { get; set; }
        public List<FindMissingLetterOption> Options { get; set; }
        public bool HasLongAndShortLetters { get; set; }
        public bool RandomizeCase { get; set; }

        public FindMissingLetterIteration(List<string> lettersToLearn, List<FindMissingLetterTaskLetter> taskLetters, bool hasLongAndShortLetters, bool randomizeCase): base(lettersToLearn)
        {
            this.TaskLetters = taskLetters;
            this.HasLongAndShortLetters = hasLongAndShortLetters;
            this.RandomizeCase = randomizeCase;
        }
    }

    public class FindMissingLetterTaskLetter : LetterBase
    {
        public bool IsSearched { get; set; }
        public string CorrectLetter { get; set; }

        public FindMissingLetterTaskLetter(string letter, bool isShort, bool isLong, bool isSearched) : base (letter, isShort, isLong)
        {
            this.IsSearched = isSearched;
            CorrectLetter = Letter;
            Letter = isSearched ? "" : letter;
        }
    }

    public class FindMissingLetterOption : LetterBase
	{
		public int CorrectPos { get; set; }

        public FindMissingLetterOption (string letter, bool isShort, bool isLong, int correctPos) : base (letter, isShort, isLong)
		{
			this.CorrectPos = correctPos;
		}
	}
}