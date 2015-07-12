using System;
using System.Collections.ObjectModel;

namespace IRMGARD.Models
{
	public class FindMissingLetter : LessonData
	{
		private ObservableCollection<LevelOption> levelOptionsList;
		public ObservableCollection<LevelOption> LevelOptionsList
		{
			get { return levelOptionsList; }
			set { LevelOptionsList = levelOptionsList; }
		}

		public FindMissingLetter(){}

		public FindMissingLetter(string letterToLearn, ObservableCollection<LevelOption> levelOptionsList)
		{
			LetterToLearn = letterToLearn;
			LevelOptionsList = levelOptionsList;
		}
	}
}

