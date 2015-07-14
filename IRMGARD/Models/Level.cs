using System;
using System.Collections.Generic;
using Newtonsoft.Json;

namespace IRMGARD.Models
{
	public class Level
	{
		public string Name { get; set; }
		public List<Module> ModulesList { get; set; }

		public Level () {}

		public Level (string name, List<Module> modulesList)
		{
			this.Name = name;
			this.ModulesList = modulesList;
		}
	}
}