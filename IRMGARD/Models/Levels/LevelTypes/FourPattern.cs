using System;
using System.Collections.ObjectModel;

namespace IRMGARD.Models
{
	public class FourPattern : LessonData
	{
		private ObservableCollection<LevelOption> levelOptionsList;
		public ObservableCollection<LevelOption> LevelOptionsList
		{
			get { return levelOptionsList; }
			set { LevelOptionsList = levelOptionsList; }
		}

		public FourPattern(){}

		public FourPattern(string letterToLearn, ObservableCollection<LevelOption> levelOptionsList)
		{
			LetterToLearn = letterToLearn;
			LevelOptionsList = levelOptionsList;
		}
	}
}

