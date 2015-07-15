using System;

namespace IRMGARD.Models
{
	public interface ILesson
	{
		string Title { get; set; }
		string SoundPath { get; set; }
		string Hint { get; set; }
		LevelType TypeOfLevel { get; set; }
	}
}

