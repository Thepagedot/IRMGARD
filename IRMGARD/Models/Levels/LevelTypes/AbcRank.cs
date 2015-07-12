using System;
using System.Collections.ObjectModel;

namespace IRMGARD.Models
{
	public class AbcRank : LessonData
	{
		private ObservableCollection<LevelOption> lettersToLearn;
		public ObservableCollection<LevelOption> LettersToLearn
		{
			get { return lettersToLearn; }
			set { LettersToLearn = lettersToLearn; }
		}

		public AbcRank (){}

		public AbcRank(ObservableCollection<LevelOption> lettersToLearn)
		{
			LettersToLearn = lettersToLearn;
		}
	}
}

