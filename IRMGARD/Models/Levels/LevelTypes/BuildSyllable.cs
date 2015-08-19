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
		public List<BuildSyllableOption> Options { get; set; }

		public BuildSyllableIteration(List<string> lettersToLearn, List<Syllable> syllables, List<BuildSyllableOption> options) : base (lettersToLearn)
	    {
	        this.Syllables = syllables;
	        this.Options = options;
	    }
	}

	public class BuildSyllableOption
	{
		public string Letter { get; set; }
		public bool IsShort { get; set; }
		public bool IsLong { get; set; }
		public int CorrectPos { get; set; }

		public BuildSyllableOption () {}

		public BuildSyllableOption (string letter, bool isShort, bool isLong, int correctPos)
		{
			this.Letter = letter;
			this.IsShort = isShort;
			this.IsLong = isLong;
			this.CorrectPos = correctPos;
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

