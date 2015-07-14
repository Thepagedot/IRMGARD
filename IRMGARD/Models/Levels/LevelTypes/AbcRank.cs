using System;
using System.Collections.Generic;

namespace IRMGARD.Models
{
	public class AbcRank : Lesson
	{
		public List<AbcRankOption> LettersToLearn { get; set; }


		public AbcRank () : base () {}

		public AbcRank (string title, string soundPath, string hint, LevelType typeOfLevel, List<AbcRankOption> lettersToLearn) 
			: base (title, soundPath, hint, typeOfLevel)
		{
			this.LettersToLearn = lettersToLearn;
		}
	}

	public class AbcRankOption
	{
		public string Name { get; set; }
		public LevelElement Element { get; set; }


		public AbcRankOption () {}

		public AbcRankOption (string name, LevelElement element)
		{
			this.Name = name;
			this.Element = element;
		}
	}
}

