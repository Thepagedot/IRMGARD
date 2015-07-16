using System;
using System.Collections.Generic;

namespace IRMGARD.Models
{
	public class HearMe : ILesson
	{
		public string Title { get; set; }
		public string SoundPath { get; set; }
		public string Hint { get; set; }
		public LevelType TypeOfLevel { get; set; }
		public List<HearMeIteration> Iterations { get; set; }


		public HearMe () {}

		public HearMe (string title, string soundPath, string hint, LevelType typeOfLevel, List<HearMeIteration> iterations) 
		{
			this.Title = title;
			this.SoundPath = soundPath;
			this.Hint = hint;
			this.TypeOfLevel = typeOfLevel;
			this.Iterations = iterations;
		}
	}

	public class HearMeIteration
	{
		public string LetterToLearn;
		public string Name;
		public LevelElement Element;


		public HearMeIteration () {}

		public HearMeIteration (string letterToLearn, string name, LevelElement element)
		{
			this.LetterToLearn = letterToLearn;
			this.Name = name;
			this.Element = element;
		}
	}
}
