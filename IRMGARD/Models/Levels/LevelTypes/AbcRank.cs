using System;
using System.Collections.Generic;

namespace IRMGARD.Models
{
	public class AbcRank : Lesson
	{
		public List<AbcRankOption> LettersToLearn { get; set; }

		public AbcRank () {}

		public AbcRank (string title, string soundPath, string hint, LevelType typeOfLevel, List<AbcRankOption> lettersToLearn)  
			: base (title, soundPath, hint, typeOfLevel)
		{
			this.LettersToLearn = lettersToLearn;
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

