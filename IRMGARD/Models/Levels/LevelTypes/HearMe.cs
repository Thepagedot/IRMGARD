using System;

namespace IRMGARD.Models
{
	[Serializable]
	public class HearMe : Lesson
	{
		public string LetterToLearn;
		public string Name;
		public LevelElement Element;


		public HearMe () {}

		public HearMe (string title, string soundPath, string hint, LevelType typeOfLevel, string letterToLearn, string name, LevelElement element) 
			: base (title, soundPath, hint, typeOfLevel)
		{
			this.LetterToLearn = letterToLearn;
			this.Name = name;
			this.Element = element;
		}
	}
}