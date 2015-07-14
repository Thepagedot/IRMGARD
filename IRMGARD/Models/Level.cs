using System;
using System.Collections.Generic;
using Newtonsoft.Json;

namespace IRMGARD.Models
{
	public class Level
	{
		public List<Module> ModulesList { get; set; }

		public Level () {}

		public Level (List<Module> modulesList)
		{
			this.ModulesList = modulesList;
		}
	}
}

