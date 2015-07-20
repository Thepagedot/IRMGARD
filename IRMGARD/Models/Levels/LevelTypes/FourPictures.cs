using System;
using System.Collections.Generic;

namespace IRMGARD.Models
{
	public class FourPictures : Lesson
	{
		public List<FourPicturesIterations> Iterations { get; set; }

		public FourPictures(){}

		public FourPictures(string title, string soundPath, string hint, LevelType typeOfLevel, List<FourPicturesIterations> iterations)  
			: base (title, soundPath, hint, typeOfLevel)
		{
			this.Iterations = iterations;
		}
	}

	public class FourPicturesIterations
	{
		public string LetterToLearn { get; set; }
		public List<FourPicturesOption> Options { get; set; }


		public FourPicturesIterations () {}

		public FourPicturesIterations (string letterToLearn, List<FourPicturesOption> options)
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

