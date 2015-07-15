using System;

namespace IRMGARD.Models
{
	public class HearMe : ILesson
	{
		public string Title { get; set; }
		public string SoundPath { get; set; }
		public string Hint { get; set; }
		public LevelType TypeOfLevel { get; set; }
		public string LetterToLearn;
		public string Name;
		public LevelElement Element;

<<<<<<< Upstream, based on origin/master
		public HearMe () {}
=======

		public HearMe () : base () {}
>>>>>>> 2e23853 now lessons can be serialized as there original type

		public HearMe (string title, string soundPath, string hint, LevelType typeOfLevel, string letterToLearn, string name, LevelElement element) 
		{
			this.Title = title;
			this.SoundPath = soundPath;
			this.Hint = hint;
			this.TypeOfLevel = typeOfLevel;
			this.LetterToLearn = letterToLearn;
			this.Name = name;
			this.Element = element;
		}
	}
}
