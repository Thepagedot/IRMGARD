using System;
using System.Collections.Generic;

namespace IRMGARD.Models
{
	public class HearMe : Lesson
	{
		public HearMe () : base () {}

		public HearMe (int id, string title, string soundPath, string hint, LevelType typeOfLevel, List<Iteration> iterations) : base (id, title, soundPath, hint, typeOfLevel, iterations)
		{
		}
	}

	public class HearMeIteration : Iteration
	{
		public string Name;
		public Media Media;

		public HearMeIteration () {}

		public HearMeIteration (List<string> lettersToLearn, string name, Media media) : base (lettersToLearn)
		{
			this.Name = name;
			this.Media = media;
		}
	}
}
