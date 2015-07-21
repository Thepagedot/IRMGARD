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
		public List<string> Options { get; set; }

	    public BuildSyllableIteration(List<string> lettersToLearn, List<Syllable> syllables, List<string> options) : base (lettersToLearn)
	    {
	        this.Syllables = syllables;
	        this.Options = options;
	    }
	}


	public class Syllable
	{
		public List<string> SyllableParts { get; set; }
		public string SoundPath { get; set; }

		public Syllable () {}

		public Syllable (List<string> syllableParts, string soundPath)
		{
			this.SyllableParts = syllableParts;
			this.SoundPath = soundPath;
		}
	}
}

