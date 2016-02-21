using System;
using System.Collections.Generic;

namespace IRMGARD.Models
{
	public class AbcRank : Lesson
	{
		public AbcRank () {}

        public AbcRank (int id, string title, bool isRecurringTask, string soundPath, string hint, LevelType typeOfLevel, List<Iteration> iterations)
            : base (id, title, isRecurringTask, soundPath, hint, typeOfLevel, iterations)
		{
		}
	}

	public class AbcRankIteration : Iteration
	{
        public List<TaskItem> TaskItems { get; set; }
		public List<AbcRankOption> Options { get; set; }

		public AbcRankIteration (int id, List<string> lettersToLearn, List<AbcRankOption> options) : base (id, lettersToLearn)
		{
			this.Options = options;
            this.TaskItems = new List<TaskItem>();
		}
	}

	public class AbcRankOption
	{
		public string Name { get; set; }
		public Media Media { get; set; }

		public AbcRankOption () {}

		public AbcRankOption (string name, Media media)
		{
			this.Name = name;
			this.Media = media;
		}
	}
}

