using System;
using System.Collections.ObjectModel;

namespace IRMGARD.Models
{
	public class Level
	{
		private ObservableCollection<Module> modulesList;
		public ObservableCollection<Module> ModulesList
		{
			get { return modulesList; }
			set { ModulesList = modulesList; }
		}

		public Level (){}

		public Level (ObservableCollection<Module> modulesList)
		{
			ModulesList = modulesList;
		}
	}
}

