using System;
using System.Collections.Generic;
using Newtonsoft.Json;
using System.Linq;

namespace IRMGARD.Models
{
	public class Level
	{
		public string Name { get; set; }
		public string Color { get; set; }
        public bool IsEnabled { get; set; }
		public List<Module> Modules { get; set; }

		public Level () {}

        public Level (string name, string color, bool isEnabled, List<Module> modules)
		{
			this.Name = name;
			this.Color = color;
            this.IsEnabled = isEnabled;
			this.Modules = modules;
		}

		/// <summary>
		/// Determines whether there is a next module available from the provided module
		/// </summary>
		/// <returns><c>true</c> if this level has a next module; otherwise, <c>false</c>.</returns>
		/// <param name="currentModule">Current module.</param>
		public bool HasNextModule(Module currentModule)
		{
			var index = Modules.IndexOf(currentModule);
			return Modules.Count - 1 > index;
		}

		/// <summary>
		/// Gets the next module if available.
		/// </summary>
		/// <returns>The next module.</returns>
		/// <param name="currentModule">Current module.</param>
		public Module GetNextModule(Module currentModule)
		{
			if (HasNextModule(currentModule))
			{
				var index = Modules.IndexOf (currentModule) + 1;
				return Modules.ElementAt(index); 
			}

			return null;
		}

		/// <summary>
		/// Determines whether there is a previous module available from the provided module
		/// </summary>
		/// <returns><c>true</c> if this level has a previous module; otherwise, <c>false</c>.</returns>
		/// <param name="currentModule">Current module.</param>
		public bool HasPreviousModule(Module currentModule)
		{
			var index = Modules.IndexOf(currentModule);
			return index > 0;
		}

		/// <summary>
		/// Gets the previoust module if available.
		/// </summary>
		/// <returns>The previous module.</returns>
		/// <param name="currentModule">Current module.</param>
		public Module GetPreviousModule(Module currentModule)
		{
			if (HasPreviousModule(currentModule))
			{
				var index = Modules.IndexOf(currentModule) - 1;
				return Modules.ElementAt(index);
			}

			return null;
		}
	}
}