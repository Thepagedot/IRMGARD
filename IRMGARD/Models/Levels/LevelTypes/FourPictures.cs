using System;
using System.Collections.Generic;

namespace IRMGARD.Models
{
	public class FourPictures : Lesson
	{
		public FourPictures(){}

		public FourPictures(string title, string soundPath, string hint, LevelType typeOfLevel, List<Iteration> iterations) : base (title, soundPath, hint, typeOfLevel, iterations)
		{
		}
	}

	public class FourPicturesIteration : Iteration
	{
		public List<FourPicturesOption> Options { get; set; }

		public FourPicturesIteration (List<string> lettersToLearn, List<FourPicturesOption> options) : base (lettersToLearn)
		{
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

