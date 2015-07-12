using System;
using System.Collections.ObjectModel;

namespace IRMGARD.Models
{
	public class LetterDrop : LessonData
	{
		private ObservableCollection<string> lettersToLearn;
		public ObservableCollection<string> LettersToLearn
		{
			get { return lettersToLearn; }
			set { LettersToLearn = lettersToLearn; }
		}

		private ObservableCollection<string> options;
		public ObservableCollection<string> Options
		{
			get { return options; }
			set { Options = options; }
		}

		public LetterDrop(){}

		public LetterDrop(ObservableCollection<string> lettersToLearn, ObservableCollection<string> options)
		{
			LettersToLearn = lettersToLearn;
			Options = options;
		}
	}
}

