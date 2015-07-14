using System;
using System.Threading.Tasks;
using System.IO;
using System.Collections.Generic;
using IRMGARD.Models;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using Android.App;
using Android.Runtime;


namespace IRMGARD
{
	public class DataManager
	{
		// Copied to DataHolder
//		public static Level GetLevel (int level)
//		{
//			//demoFunctionForSerialization ();
//			string content = string.Empty;
//			using (StreamReader sr = new StreamReader (Application.Context.Assets.Open (getNumberAsString(level) + ".json")))
//			{
//				content = sr.ReadToEnd();
//				try
//				{
//					
//					JObject json = JObject.Parse(content);
//					return JsonConvert.DeserializeObject<Level> (json.ToString(), new JsonSerializerSettings
//						{
//							TypeNameHandling = TypeNameHandling.Objects
//						});
//
//				} 
//				catch (Exception ex) 
//				{
//					Console.WriteLine ("JSON reader Exception on reading level {0}", level);
//					Console.WriteLine ("Message: {0}", ex.Message);
//				}
//			}
//			return new Level ();
//		}
			

		private static void demoFunctionForSerialization(){



			var test = new List<Lesson> ();
			test.Add(new HearMe("test", "soundpath1.mp3", "", LevelType.HearMe, "A", "Aal", new LevelElement("Lesson1.png", "Lesson1.mp3")));
			test.Add(new HearMe("test", "soundpath2.mp3", "", LevelType.HearMe, "E", "Esel", new LevelElement("Lesson2.png", "Lesson2.mp3")));
			test.Add (new PickSyllable ("syllable", "soundpath3,mp3", "", LevelType.PickSyllable, "S", new List<String> (), new List<PickSyllableOption> ()));


			var module = new Module (test, 3, 0);
			var moduleList = new List<Module> ();
			moduleList.Add (module);
			var level = new Level ("1", moduleList);

			var json = JsonConvert.SerializeObject (level);

			string name = "TEST";

			/*Lesson l = new Lesson("Lesson 1", "Lesson1.mp3", string.Empty, LevelType.HearMe, new LessonData());
			Lesson l1 = new Lesson("Lesson 2", "Lesson2.mp3", "", LevelType.FourPictures, new LessonData());
			Lesson l2 = new Lesson("Lesson 3", "Lesson3.mp3", "", LevelType.AbcRank, new LessonData());

			//LessonData data = new LessonData ("A", new LevelOption("Aal", new LevelElement("", "")));
			//l.Data = data;

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

			var mno = JsonConvert.DeserializeObject<Level>(t);*/
		}
	}
}
