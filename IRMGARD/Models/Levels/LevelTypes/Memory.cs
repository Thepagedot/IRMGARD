using System;
using System.Collections.Generic;

namespace IRMGARD.Models
{
	public class Memory : ILesson
	{
		public string Title { get; set; }
		public string SoundPath { get; set; }
		public string Hint { get; set; }
		public LevelType TypeOfLevel { get; set; }
		public List<MemoryOption> LevelOptionsList { get; set; }

		public Memory () {}

		public Memory (string title, string soundPath, string hint, LevelType typeOfLevel, List<MemoryOption> levelOptionsList)
		{
			this.LevelOptionsList = levelOptionsList;
		}
	}

	public class MemoryOption
	{
		public string Name { get; set; }
		public string SoundPath { get; set; }


		public MemoryOption () {}

		public MemoryOption (string name, string soundPath) 
		{
			this.Name = name;
			this.SoundPath = soundPath;
		}
	}
}

