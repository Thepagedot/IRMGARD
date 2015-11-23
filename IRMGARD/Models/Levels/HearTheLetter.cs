using System;
using System.Collections.Generic;

namespace IRMGARD.Models
{
	public class HearTheLetter : Lesson
	{
		public HearTheLetter () {}

		public HearTheLetter(int id, string title, string soundPath, string hint, LevelType typeOfLevel, List<Iteration> iterations) : base (id, title, soundPath, hint, typeOfLevel, iterations)
		{
		}

        public class HearTheLetterIteration : Iteration
        {
            public string LetterToLearn { get; set; }
            public Media Media { get; set; }
            public int NumberOfOptions { get; set; }
            public int CorrectOption { get; set; }

            public HearTheLetterIteration(List<string> lettersToLearn, string letterToLearn, Media media, int numberOfOptions, int correctOption) : base(lettersToLearn)
            {
                LetterToLearn = letterToLearn;
                Media = media;
                NumberOfOptions = numberOfOptions;
                CorrectOption = correctOption;
            }
        }
	}
}
