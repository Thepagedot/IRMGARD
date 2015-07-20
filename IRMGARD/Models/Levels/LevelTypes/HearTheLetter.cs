using System;

namespace IRMGARD.Models
{
	public class HearTheLetter : Lesson
	{
		public string LetterToLearn { get; set; }
		public Media Media { get; set; }
		public int NumberOfOptions { get; set; }
		public int CorrectOption { get; set; }

		public HearTheLetter () {}

		public HearTheLetter (string title, string soundPath, string hint, LevelType typeOfLevel, string letterToLearn, Media media, int numberOfOptions, int correctOption) 
			: base (title, soundPath, hint, typeOfLevel)
		{
			this.LetterToLearn = letterToLearn;
			this.Media = media;
			this.NumberOfOptions = numberOfOptions;
			this.CorrectOption = correctOption;
		}
	}
}
