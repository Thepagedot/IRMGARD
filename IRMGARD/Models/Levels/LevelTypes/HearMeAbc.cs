using System;

namespace IRMGARD.Models
{
	public class HearMeAbc : Lesson
	{
		public string LetterToLearn { get; set; }
		public string Prepend { get; set;}
		public string Append { get; set; }
		public Media Media { get; set; }

		public HearMeAbc () {}

		public HearMeAbc (string title, string soundPath, string hint, LevelType typeOfLevel, string letterToLearn, string prepend, string append, Media media)  
			: base (title, soundPath, hint, typeOfLevel)
		{
			this.LetterToLearn = letterToLearn;
			this.Prepend = prepend;
			this.Append = append;
			this.Media = media;
		}
	}
}
