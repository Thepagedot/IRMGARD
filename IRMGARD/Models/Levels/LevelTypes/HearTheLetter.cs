using System;

namespace IRMGARD.Models
{
	public class HearTheLetter : ILesson
	{
		public string Title { get; set; }
		public string SoundPath { get; set; }
		public string Hint { get; set; }
		public LevelType TypeOfLevel { get; set; }
		public string LetterToLearn { get; set; }
		public Media Media { get; set; }
		public int NumberOfOptions { get; set; }
		public int CorrectOption { get; set; }

		public HearTheLetter () {}

		public HearTheLetter (string title, string soundPath, string hint, LevelType typeOfLevel, string letterToLearn, Media media, int numberOfOptions, int correctOption) 
		{
			this.LetterToLearn = letterToLearn;
			this.Media = media;
			this.NumberOfOptions = numberOfOptions;
			this.CorrectOption = correctOption;
		}
	}
}
