using System;
using System.Collections.ObjectModel;

namespace IRMGARD.Models
{
	public abstract class Lesson
	{
		public string Title { get; set; }
		public string SoundPath { get; set; }
		public string Hint { get; set; }
		public LevelType TypeOfLevel { get; set; }
		// ToDo: Liste für rot markierte Buchstaben

		public Lesson () {}

		public Lesson (string title, string soundPath, string hint, LevelType typeOfLevel)
		{
			this.Title = title;
			this.SoundPath = soundPath;
			this.Hint = hint;
			this.TypeOfLevel = typeOfLevel;
		}
	}
}

