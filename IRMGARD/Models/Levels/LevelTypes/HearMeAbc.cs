using System;

namespace IRMGARD.Models
{
	public class HearMeAbc : ILesson
	{
		public string Title { get; set; }
		public string SoundPath { get; set; }
		public string Hint { get; set; }
		public LevelType TypeOfLevel { get; set; }
		public string LetterToLearn { get; set; }
		public string Prepend { get; set;}
		public string Append { get; set; }
		public Media Media { get; set; }

		public HearMeAbc () {}

		public HearMeAbc (string title, string soundPath, string hint, LevelType typeOfLevel, string letterToLearn, string prepend, string append, Media media) 
		{
			this.LetterToLearn = letterToLearn;
			this.Prepend = prepend;
			this.Append = append;
			this.Media = media;
		}
	}
}
