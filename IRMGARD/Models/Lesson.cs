using System;
using System.Collections.ObjectModel;

namespace IRMGARD.Models
{
	public class Lesson
	{
		private string title;
		public string Title
		{
			get { return title; }
			set { Title = title; }
		}

		private string soundPath;
		public string SoundPath
		{
			get { return soundPath; }
			set { SoundPath = soundPath; }
		}

		private string hint;
		public string Hint
		{
			get { return hint; }
			set { Hint = hint; }
		}

		private LevelType typeOfLevel;
		public LevelType TypeOfLevel
		{
			get { return typeOfLevel; }
			set { TypeOfLevel = typeOfLevel; }
		}

		private LessonData data;
		public LessonData Data
		{
			get { return data; }
			set { Data = data; }
		}

		public Lesson (){}

		public Lesson (string title, string soundPath, string hint, LevelType typeOfLevel, LessonData data)
		{
			Title = title;
			SoundPath = soundPath;
			Hint = hint;
			TypeOfLevel = typeOfLevel;
			Data = data;
		}
	}
}

