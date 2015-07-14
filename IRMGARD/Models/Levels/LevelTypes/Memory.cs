using System;
using System.Collections.Generic;

namespace IRMGARD.Models
{
	public class Memory : Lesson
	{
		public List<MemoryOption> LevelOptionsList { get; set; }


		public Memory () {}

		public Memory (string title, string soundPath, string hint, LevelType typeOfLevel, List<MemoryOption> levelOptionsList)
			: base (title, soundPath, hint, typeOfLevel)
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

