using System;
using System.Collections.ObjectModel;
using System.Collections.Generic;

namespace IRMGARD.Models
{
	public abstract class Lesson
	{
		public int Id { get; set; }
		public string Title { get; set; }
		public string SoundPath { get; set; }
		public string Hint { get; set; }
		public LevelType TypeOfLevel { get; set; }
		public List<Iteration> Iterations { get; set; }

	    protected Lesson () {}

	    protected Lesson (int id, string title, string soundPath, string hint, LevelType typeOfLevel, List<Iteration> iterations)
		{
			this.Id = id;
			this.Title = title;
			this.SoundPath = soundPath;
			this.Hint = hint;
			this.TypeOfLevel = typeOfLevel;
			this.Iterations = iterations;
		}
	}
}