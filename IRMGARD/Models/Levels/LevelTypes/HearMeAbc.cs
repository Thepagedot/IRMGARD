using System;
using System.Collections.Generic;

namespace IRMGARD.Models
{
	public class HearMeAbc : Lesson
	{
		public HearMeAbc () {}

		public HearMeAbc (string title, string soundPath, string hint, LevelType typeOfLevel, List<Iteration> iterations) : base (title, soundPath, hint, typeOfLevel, iterations)
		{
		}

        public class HearMeAbcIteration : Iteration
        {
            public string Prepend { get; set; }
            public string Append { get; set; }
            public Media Media { get; set; }

            public HearMeAbcIteration(List<string> lettersToLearn, string letterToLearn, string prepend, string append, Media media) : base(lettersToLearn)
            {
                Prepend = prepend;
                Append = append;
                Media = media;
            }
        }
	}
}
