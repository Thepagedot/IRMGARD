using System;
using System.Collections.Generic;

namespace IRMGARD.Models
{
	public class HearMe : Lesson
	{
		public HearMe () : base () {}

        public HearMe (int id, string title, bool isRecurringTask, string soundPath, string hint, LevelType typeOfLevel, List<Iteration> iterations)
            : base (id, title, isRecurringTask, soundPath, hint, typeOfLevel, iterations)
		{
		}
	}

	public class HearMeIteration : Iteration
	{
		public string Name;
		public Media Media;

		public HearMeIteration () {}

		public HearMeIteration (int id, List<string> lettersToLearn, string name, Media media) : base (id, lettersToLearn)
		{
			this.Name = name;
			this.Media = media;
		}
	}
}
