using System;
using System.Collections.Generic;

namespace IRMGARD.Models
{
	public class BuildSyllable : ILesson
	{
		public string Title { get; set; }
		public string SoundPath { get; set; }
		public string Hint { get; set; }
		public LevelType TypeOfLevel { get; set; }
		public List<BuildSyllableOption> SyllableOptions { get; set; }
		public List<string> Options { get; set; }

		public BuildSyllable () {}

		public BuildSyllable (string title, string soundPath, string hint, LevelType typeOfLevel, List<string> options, List<BuildSyllableOption> syllableOptions) 
		{
			this.SyllableOptions = syllableOptions;
			this.Options = options;
		}
	}

	public class BuildSyllableOption
	{
		public List<string> SyllableParts { get; set; }
		public string SoundPath { get; set; }


		public BuildSyllableOption () {}

		// needed for BuildSyllables
		public BuildSyllableOption (List<string> syllableParts, string soundPath)
		{
			this.SyllableParts = syllableParts;
			this.SoundPath = soundPath;
		}
	}
}

