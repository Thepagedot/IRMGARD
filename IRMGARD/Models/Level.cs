using System;
using System.Collections.ObjectModel;
using Newtonsoft.Json;

namespace IRMGARD.Models
{
	public class Level
	{
		public ObservableCollection<Module> ModulesList;
		public void setModulesList(ObservableCollection<Module> modulesList){
			this.ModulesList = modulesList;
		}
		public ObservableCollection<Module> getModulesList(){
			return this.ModulesList;
		}

		public Level (){}

		public Level (ObservableCollection<Module> modulesList)
		{
			this.setModulesList(modulesList);
		}
	}
}

