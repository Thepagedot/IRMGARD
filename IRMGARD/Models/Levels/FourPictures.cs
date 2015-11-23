using System;
using System.Collections.Generic;

namespace IRMGARD.Models
{
	public class FourPictures : Lesson
	{
		public List<FourPicturesOption> Options { get; set; }

		public FourPictures(){}

		public FourPictures(int id, string title, string soundPath, string hint, LevelType typeOfLevel, List<FourPicturesOption> options, List<Iteration> iterations) : base (id, title, soundPath, hint, typeOfLevel, iterations)
		{
			this.Options = options;
		}
	}

	public class FourPicturesIteration : Iteration
	{		
		public FourPicturesIteration (List<string> lettersToLearn) : base (lettersToLearn)
		{
			
		}
	}

	public class FourPicturesOption
	{
		public string Letter { get; set; }
		public bool IsCorrect { get; set; }
		public Media Media  { get; set; }

		public FourPicturesOption () {}

		public FourPicturesOption (string letter, Media media)
		{
			this.Letter = letter;
			this.IsCorrect = false;
			this.Media = media;
		}

		public FourPicturesOption (string letter, bool isCorrect, Media media)
		{
			this.Letter = letter;
			this.IsCorrect = isCorrect;
			this.Media = media;
		}
	}
}

