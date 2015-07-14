using System;
using System.Collections.Generic;

namespace IRMGARD.Models
{
	public class FourPictures : Lesson
	{
		public string LetterToLearn { get; set; }
		public List<FourPicturesOption> Options { get; set; }


		public FourPictures(){}

		public FourPictures(string title, string soundPath, string hint, LevelType typeOfLevel, string letterToLearn, List<FourPicturesOption> options) 
			: base(title, soundPath, hint, typeOfLevel)
		{
			this.LetterToLearn = letterToLearn;
			this.Options = options;
		}
	}

	public class FourPicturesOption
	{
		public bool IsCorrect { get; set; }
		public LevelElement Element  { get; set; }


		public FourPicturesOption () {}

		public FourPicturesOption (bool isCorrect, LevelElement element)
		{
			this.IsCorrect = isCorrect;
			this.Element = element;
		}
	}
}

