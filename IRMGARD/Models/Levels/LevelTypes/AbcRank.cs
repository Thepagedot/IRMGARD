using System;
using System.Collections.Generic;

namespace IRMGARD.Models
{
	public class AbcRank : Lesson
	{		
		public AbcRank () {}

		public AbcRank (int id, string title, string soundPath, string hint, LevelType typeOfLevel, List<Iteration> iterations) : base (id, title, soundPath, hint, typeOfLevel, iterations)
		{			
		}
	}

	public class AbcRankIteration : Iteration
	{
		public List<AbcRankOption> Options { get; set; }

		public AbcRankIteration (List<string> lettersToLearn, List<AbcRankOption> options) : base (lettersToLearn)
		{
			this.Options = options;
		}
	}

	public class AbcRankOption
	{
		public string Name { get; set; }
		public Media Media { get; set; }

		public AbcRankOption () {}

		public AbcRankOption (string name, Media media)
		{
			this.Name = name;
			this.Media = media;
		}
	}
}

