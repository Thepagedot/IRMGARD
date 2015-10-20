using System;
using System.Collections.Generic;

namespace IRMGARD.Models
{
	public class BuildSyllable : Lesson
	{		
		public BuildSyllable () {}

		public BuildSyllable (int id, string title, string soundPath, string hint, LevelType typeOfLevel, List<Iteration> iterations) : base (id, title, soundPath, hint, typeOfLevel, iterations)
		{
		}
	}

	public class BuildSyllableIteration : Iteration
	{
		public List<Syllable> Syllables { get; set; }
		public List<LetterBase> Options { get; set; }
        public bool HasLongAndShortLetters { get; set; }

        public BuildSyllableIteration(List<string> lettersToLearn, List<Syllable> syllables, bool hasLongAndShortLetters) : base (lettersToLearn)
	    {
	        this.Syllables = syllables;
            this.HasLongAndShortLetters = hasLongAndShortLetters;
	    }
	}        

	public class Syllable
	{
        public List<TaskLetter> SyllableParts { get; set; }
		public string SoundPath { get; set; }

		public Syllable () {}

        public Syllable (List<TaskLetter> syllableParts, string soundPath)
		{
			this.SyllableParts = syllableParts;
			this.SoundPath = soundPath;
		}
	}
}   