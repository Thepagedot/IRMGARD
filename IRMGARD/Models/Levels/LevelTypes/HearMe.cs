using System;
using System.Collections.Generic;

namespace IRMGARD.Models
{
	public class HearMe : Lesson
	{
		public List<HearMeIteration> Iterations { get; set; }


		public HearMe () {}

		public HearMe (string title, string soundPath, string hint, LevelType typeOfLevel, List<HearMeIteration> iterations) 
			: base (title, soundPath, hint, typeOfLevel)
		{
			this.Iterations = iterations;
		}
	}

	public class HearMeIteration
	{
		public string LetterToLearn;
		public string Name;
		public Media Media;


		public HearMeIteration () {}

		public HearMeIteration (string letterToLearn, string name, Media media)
		{
			this.LetterToLearn = letterToLearn;
			this.Name = name;
			this.Media = media;
		}
	}
}
