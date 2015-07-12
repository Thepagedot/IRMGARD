using System;

namespace IRMGARD.Models
{
	public class HearMe : LessonData
	{
		private LevelOption levelOptions;
		public LevelOption LevelOptions
		{
			get { return levelOptions; }
			set { LevelOptions = levelOptions; }
		}

		public HearMe(){}

		public HearMe(string letterToLearn, LevelOption levelOptions)
		{
			LetterToLearn = letterToLearn;
			LevelOptions = levelOptions;
		}
	}
}

