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
		public List<SyllablesToLearn> SyllablePool { get; set; }

        public BuildSyllableIteration(int id, List<string> lettersToLearn, List<SyllablesToLearn> syllablePool) : base (id, lettersToLearn)
	    {
	        this.SyllablePool = syllablePool;
	    }
	}        

	public class SyllablesToLearn
	{
		public List<Syllable> Syllables { get; set; }
		public List<LetterBase> Options { get; set; }
        public bool HasLongAndShortLetters { get; set; }
        public Media Media { get; set; }
        
        public SyllablesToLearn(List<Syllable> syllables, bool hasLongAndShortLetters, Media media)
	    {
	        this.Syllables = syllables;
            this.HasLongAndShortLetters = hasLongAndShortLetters;
            this.Media = media;
	    }
	}

	public class Syllable
	{
        public List<TaskItem> SyllableParts { get; set; }
		public string SoundPath { get; set; }

		public Syllable () {}

        public Syllable (List<TaskItem> syllableParts, string soundPath)
		{
			this.SyllableParts = syllableParts;
			this.SoundPath = soundPath;
		}
	}
}