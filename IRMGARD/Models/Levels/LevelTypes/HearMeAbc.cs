using System;

namespace IRMGARD.Models
{
	public class HearMeAbc : LessonData
	{
		private string prepend;
		public string Prepend
		{
			get { return prepend; }
			set { Prepend = prepend; }
		}

		private string append;
		public string Append
		{
			get { return append; }
			set { Append = append; }
		}

		private LevelElement levelElements;
		public LevelElement LevelElements
		{
			get { return levelElements; }
			set { LevelElements = levelElements; }
		}

		public HearMeAbc (){}

		public HearMeAbc (string letterToLearn, string prepend, string append, LevelElement levelElements)
		{
			LetterToLearn = letterToLearn;
			Prepend = prepend;
			Append = append;
			LevelElements = levelElements;
		}
	}
}

