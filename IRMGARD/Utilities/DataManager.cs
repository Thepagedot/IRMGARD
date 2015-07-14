using System;
using System.Threading.Tasks;
using System.IO;
using System.Collections.ObjectModel;
using IRMGARD.Models;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using Android.App;
using Android.Runtime;


namespace IRMGARD
{
	public class DataManager
	{
		public static Level GetLevel (int level)
		{
			string content = string.Empty;
			using (StreamReader sr = new StreamReader (Application.Context.Assets.Open (getNumberAsString(level) + ".json")))
			{
				content = sr.ReadToEnd();
				try
				{
					
					JObject json = JObject.Parse(content);
					return JsonConvert.DeserializeObject<Level> (json.ToString());;

				} catch (Exception ex) 
				{
					Console.WriteLine ("JSON reader Exception on reading level {0}", level);
					Console.WriteLine ("Message: {0}", ex.Message);
				}
			}
			return new Level ();
		}

		private static string getNumberAsString(int level)
		{
			switch (level) 
			{
			case 1:
				return "one";
			case 2:
				return "two";
			case 3:
				return "three";
			case 4:
				return "four";
			case 5:
				return "five";
			case 6:
				return "six";
			}
			return string.Empty;
		}

		private static void demoFunctionForSerialization(){
			Lesson l = new Lesson("Lesson 1", "Lesson1.mp3", string.Empty, LevelType.HearMe, new LessonData());
			Lesson l1 = new Lesson("Lesson 2", "Lesson2.mp3", "", LevelType.FourPictures, new LessonData());
			Lesson l2 = new Lesson("Lesson 3", "Lesson3.mp3", "", LevelType.AbcRank, new LessonData());

			ObservableCollection<Lesson> cl = new ObservableCollection<Lesson>();
			cl.Add(l);
			cl.Add(l1);
			cl.Add(l2);

			Module m = new Module (cl, 3, 0);
			Module m1 = new Module (cl, 3, 0);
			Module m2 = new Module (cl, 3, 0);

			ObservableCollection<Module> ml = new ObservableCollection<Module>();
			ml.Add(m);
			ml.Add(m1);
			ml.Add(m2);

			Level levelOne = new Level(ml);

			var t = JsonConvert.SerializeObject (levelOne);

			var mno = JsonConvert.DeserializeObject<Level>(t);
		}
	}
}
