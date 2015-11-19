using System;
using System.Collections.Generic;

namespace IRMGARD.Models
{
	public class LetterDrop : Lesson
	{
		public LetterDrop () {}

		public LetterDrop (int id, string title, string soundPath, string hint, LevelType typeOfLevel, List<Iteration> iterations) : base (id, title, soundPath, hint, typeOfLevel, iterations)
		{
		}
	}

	public class LetterDropIteration : Iteration
	{
        public List<TaskItem> TaskItems { get; set; }
        public List<LetterBase> Options { get; set; }

        public LetterDropIteration(List<string> lettersToLearn) : base (lettersToLearn)
		{
		}
	}
}