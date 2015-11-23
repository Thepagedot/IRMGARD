using System;
using System.Collections.Generic;

namespace IRMGARD.Models
{
	public class Memory : Lesson
	{		
		public Memory () {}

		public Memory (int id, string title, string soundPath, string hint, LevelType typeOfLevel, List<Iteration> iterations) : base (id, title, soundPath, hint, typeOfLevel, iterations)
		{
		}
	}

    public class MemoryIteration : Iteration
    {
        public List<MemoryOption> LevelOptionsList { get; set; }

        public MemoryIteration(List<string> lettersToLearn, List<MemoryOption> levelOptionsList) : base (lettersToLearn)
        {
            LevelOptionsList = levelOptionsList;
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

