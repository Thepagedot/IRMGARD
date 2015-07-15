using System;
using System.Collections.Generic;
using Newtonsoft.Json;
using System.Linq;

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

		public bool HasNextLesson(Lesson currentLesson)
		{
			var index = LessonsList.IndexOf(currentLesson);
			return LessonsList.Count - 1 > index;
		}

		public Lesson GetNextLesson(Lesson currentLesson)
		{
			if (HasNextLesson(currentLesson))
			{
				var index = LessonsList.IndexOf(currentLesson) + 1;
				return LessonsList.ElementAt(index); 
			}

			return null;
		}

		public bool HasPreviousLesson(Lesson currentLesson)
		{
			var index = LessonsList.IndexOf(currentLesson);
			return index > 0;
		}

		public Lesson GetPrevioustLesson(Lesson currentLesson)
		{
			if (HasPreviousLesson(currentLesson))
			{
				var index = LessonsList.IndexOf(currentLesson) - 1;
				return LessonsList.ElementAt(index);
			}

			return null;
		}
	}
}
