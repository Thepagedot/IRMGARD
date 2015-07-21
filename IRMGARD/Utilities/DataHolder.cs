using System;
using System.Collections.Generic;
using IRMGARD.Models;
using System.IO;
using Android.App;
using Newtonsoft.Json.Linq;
using Newtonsoft.Json;
using System.Threading.Tasks;

namespace IRMGARD
{
	public class DataHolder
	{
		public static DataHolder Current;

		// Current variables
		public Level CurrentLevel { get; set; }
		public Module CurrentModule { get; set; }
		public Lesson CurrentLesson { get; set; }
		public Iteration CurrentIteration { get; set; }

		public List<Level> Levels { get; set; }

		public DataHolder()
		{
			Levels = new List<Level>();
			Current = this;
		}

		public async Task LoadLevelAsync(int levelNumber)
		{
			using (var reader = new StreamReader(Application.Context.Assets.Open("level" + levelNumber + ".json")))
			{				
				try
				{
					var jsonContent = await reader.ReadToEndAsync();
					var json = JObject.Parse(jsonContent);
					var level = JsonConvert.DeserializeObject<Level> (json.ToString(), new JsonSerializerSettings
					{
						TypeNameHandling = TypeNameHandling.All
					});				
					
					Levels.Add(level);
				} 
				catch (Exception ex) 
				{
					Console.WriteLine("JSON reader Exception on reading level {0}", levelNumber);
					Console.WriteLine("Message: {0}", ex.Message);
				}
			}
		}
	}
}

