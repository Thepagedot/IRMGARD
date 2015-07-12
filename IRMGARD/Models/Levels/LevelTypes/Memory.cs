using System;
using System.Collections.ObjectModel;

namespace IRMGARD.Models
{
	public class Memory : LessonData
	{
		private ObservableCollection<LevelOption> levelOptionsList;
		public ObservableCollection<LevelOption> LevelOptionsList
		{
			get { return levelOptionsList; }
			set { LevelOptionsList = levelOptionsList; }
		}
		
		public Memory (){}

		public Memory (ObservableCollection<LevelOption> levelOptionsList)
		{
			LevelOptionsList = levelOptionsList;
		}
	}
}

