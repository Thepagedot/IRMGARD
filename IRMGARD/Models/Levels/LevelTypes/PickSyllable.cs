using System;
using System.Collections.ObjectModel;

namespace IRMGARD.Models
{
	public class PickSyllable : LessonData
	{
		private string syllableToLearn;
		public string SyllableToLearn
		{
			get { return syllableToLearn; }
			set { SyllableToLearn = syllableToLearn; }
		}

		private ObservableCollection<string> syllableParts;
		public ObservableCollection<string> SyllableParts
		{
			get { return syllableParts; }
			set { SyllableParts = syllableParts; }
		}

		private ObservableCollection<LevelOption> levelOptionsList;
		public ObservableCollection<LevelOption> LevelOptionsList
		{
			get { return levelOptionsList; }
			set { LevelOptionsList = levelOptionsList; }
		}

		public PickSyllable(){}

		public PickSyllable(string syllableToLearn, ObservableCollection<string> syllableParts, ObservableCollection<LevelOption> levelOptionsList)
		{
			SyllableToLearn = syllableToLearn;
			SyllableParts = syllableParts;
			LevelOptionsList = levelOptionsList;
		}
	}
}

