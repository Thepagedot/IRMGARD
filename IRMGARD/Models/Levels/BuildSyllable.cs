using System;
using System.Collections.Generic;

namespace IRMGARD.Models
{
	public class BuildSyllable : Lesson
	{
		public BuildSyllable () {}

        public BuildSyllable (int id, string title, bool isRecurringTask, string soundPath, string hint, LevelType typeOfLevel, List<Iteration> iterations)
            : base (id, title, isRecurringTask, soundPath, hint, typeOfLevel, iterations)
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
		public List<SyllableAggregate> Syllables { get; set; }
		public List<LetterBase> Options { get; set; }
        public bool HasLongAndShortLetters { get; set; }
        public Media Media { get; set; }
        
        public SyllablesToLearn(List<SyllableAggregate> syllables, bool hasLongAndShortLetters, Media media)
	    {
	        this.Syllables = syllables;
            this.HasLongAndShortLetters = hasLongAndShortLetters;
            this.Media = media;
	    }
	}

	public class SyllableAggregate
	{
        public List<TaskItem> SyllableParts { get; set; }
		public string SoundPath { get; set; }

		public SyllableAggregate () {}

        public SyllableAggregate (List<TaskItem> syllableParts, string soundPath)
		{
			this.SyllableParts = syllableParts;
			this.SoundPath = soundPath;
		}
	}
}