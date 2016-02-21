using System;
using System.Collections.Generic;
using IRMGARD.Models;
using System.IO;
using Android.App;
using Newtonsoft.Json.Linq;
using Newtonsoft.Json;
using System.Threading.Tasks;
using IRMGARD.Utilities;

namespace IRMGARD
{
	public class DataHolder
	{
        // Singleton
		public static DataHolder Current;

		// Current variables
		public Level CurrentLevel { get; set; }
		public Module CurrentModule { get; set; }
		public Lesson CurrentLesson { get; set; }
		public Iteration CurrentIteration { get; set; }

		public List<Level> Levels { get; set; }

        public Common Common { get; private set; }

		public DataHolder()
		{
			Levels = new List<Level>();
			Current = this;
		}

		public async Task LoadLevelAsync(int levelNumber)
		{
            var level = await LocalStorage.LoadLevelAsync(levelNumber);
            if (level != null)
                Levels.Add(level);
		}

        public async Task LoadCommonAsync()
        {
            Common = await LocalStorage.LoadCommonAsync();
        }

        public async Task SaveProgressAsync()
        {
            var dict = new Dictionary<int, IterationStatus>();

            foreach (var level in Levels)
            {
                foreach (var module in level.Modules)
                {
                    foreach (var lesson in module.Lessons)
                    {
                        foreach (var iteration in lesson.Iterations)
                        {
                            try
                            {
                                dict.Add(iteration.Id, iteration.Status);
                            }
                            catch (ArgumentException)
                            {
                                throw new ArgumentException("Multiple iteration id (" + iteration.Id + ") occurs. Please check, if all iterations have different ids!");
                            }
                        }
                    }
                }
            }

            await LocalStorage.SaveToFileAsync("progress.json", dict);
        }

        public async Task LoadProgressAsync()
        {
            var dict = await LocalStorage.LoadFromFileAsync<Dictionary<int, IterationStatus>>("progress.json");
            if (dict == null)
                return;

            foreach (var level in Levels)
            {
                foreach (var module in level.Modules)
                {
                    foreach (var lesson in module.Lessons)
                    {
                        foreach (var iteration in lesson.Iterations)
                        {
                            if (dict.ContainsKey(iteration.Id))
                            {
                                iteration.Status = dict[iteration.Id];
                            }
                        }
                    }
                }
            }
        }
    }
}

