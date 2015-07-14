using System;
using System.Collections.Generic;
using Newtonsoft.Json;

namespace IRMGARD.Models
{
	public class Module
	{
		public List<Lesson> LessonsList;
		public int NumberOfLessons;
		public int NumberOfLessonsDone;

		public Module () {}

		public Module (List<Lesson> lessonsList, int numberOfLessons, int numberOfLessonsDone)
		{
			this.LessonsList = lessonsList;
			this.NumberOfLessons = numberOfLessons;
			this.NumberOfLessonsDone = numberOfLessonsDone;
		}
	}
}
