using System;
using System.Collections.Generic;
using Newtonsoft.Json;

namespace IRMGARD.Models
{
	public class Module
	{
		public List<Lesson> LessonsList;
		public int NumberOfLessonsDone;

		public Module () {}

		public Module (List<Lesson> lessonsList, int numberOfLessonsDone)
		{
			this.LessonsList = lessonsList;
			this.NumberOfLessonsDone = numberOfLessonsDone;
		}
	}
}
