using System;
using System.Collections.Generic;

namespace IRMGARD
{
	public class DataHolder
	{
		public static DataHolder Current;

		public List<string> Levels { get; set; }

		public DataHolder()
		{
			Levels = new List<string> { "Level  1", "Level 2", "Level 3", "Level 4", "Level 5", "Level 6", "Level 7", "Level 8", };
			Current = this;
		}
	}
}

