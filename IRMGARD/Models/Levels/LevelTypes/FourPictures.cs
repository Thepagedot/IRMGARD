using System;
using System.Collections.Generic;

namespace IRMGARD.Models
{
	public class FourPictures : ILesson
	{
		public string Title { get; set; }
		public string SoundPath { get; set; }
		public string Hint { get; set; }
		public LevelType TypeOfLevel { get; set; }
		public string LetterToLearn { get; set; }
		public List<FourPicturesOption> Options { get; set; }

		public FourPictures(){}

		public FourPictures(string title, string soundPath, string hint, LevelType typeOfLevel, string letterToLearn, List<FourPicturesOption> options) 
		{
			this.LetterToLearn = letterToLearn;
			this.Options = options;
		}
	}

	public class FourPicturesOption
	{
		public bool IsCorrect { get; set; }
		public Media Media  { get; set; }

		public FourPicturesOption () {}

		public FourPicturesOption (bool isCorrect, Media media)
		{
			this.IsCorrect = isCorrect;
			this.Media = media;
		}
	}
}

