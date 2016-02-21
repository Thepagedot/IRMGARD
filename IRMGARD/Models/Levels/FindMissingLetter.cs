using System;
using System.Collections.Generic;
using IRMGARD.Shared;

namespace IRMGARD.Models
{
	public class FindMissingLetter : Lesson
	{
		public FindMissingLetter () {}

        public FindMissingLetter (int id, string title, bool isRecurringTask, string soundPath, string hint, LevelType typeOfLevel, List<Iteration> iterations)
            : base (id, title, isRecurringTask, soundPath, hint, typeOfLevel, iterations)
		{
		}
	}

    public class FindMissingLetterIteration : Iteration
    {
        public List<TaskItem> TaskItems { get; set; }
        public List<FindMissingLetterOption> Options { get; set; }
        public bool HasLongAndShortLetters { get; set; }
        public bool RandomizeCase { get; set; }

        public FindMissingLetterIteration(int id, List<string> lettersToLearn, List<TaskItem> taskItems, bool hasLongAndShortLetters, bool randomizeCase) : base(id, lettersToLearn)
        {
            TaskItems = taskItems;
            HasLongAndShortLetters = hasLongAndShortLetters;
            RandomizeCase = randomizeCase;

            TaskItems.Shuffle();
        }
    }

    public class FindMissingLetterOption : LetterBase
	{
		public int CorrectPos { get; set; }

        public FindMissingLetterOption (string letter, bool isShort, bool isLong, int correctPos) : base (letter, isShort, isLong)
		{
			CorrectPos = correctPos;
		}
	}
}