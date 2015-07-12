using System;

namespace IRMGARD.Models
{
	public class HearTheLetter : LessonData
	{
		private LevelElement levelElements;
		public LevelElement LevelElements
		{
			get { return levelElements; }
			set { LevelElements = levelElements; }
		}

		private int numberOfOptions;
		public int NumberOfOptions
		{
			get { return numberOfOptions; }
			set { NumberOfOptions = numberOfOptions; }
		}

		private int correctOption;
		public int CorrectOption
		{
			get { return correctOption; }
			set { CorrectOption = correctOption; }
		}

		public HearTheLetter(){}

		public HearTheLetter(string letterToLearn, LevelElement levelElements, int numberOfOptions, int correctOption)
		{
			LetterToLearn = letterToLearn;
			LevelElements = levelElements;
			NumberOfOptions = numberOfOptions;
			CorrectOption = correctOption;
		}
	}
}

