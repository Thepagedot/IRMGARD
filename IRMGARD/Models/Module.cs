using System;
using System.Collections.Generic;
using Newtonsoft.Json;
using System.Linq;

namespace IRMGARD.Models
{
	public class Module
	{
		public string Color { get; set; }
		public List<Lesson> Lessons;
		public int NumberOfLessonsDone;

		public Module () {}

		public Module (string color, List<Lesson> lessons, int numberOfLessonsDone)
		{
			this.Color = color;
			this.Lessons = lessons;
			this.NumberOfLessonsDone = numberOfLessonsDone;
		}

		/// <summary>
		/// Determines whether there is a next lesson available from the provided lesson
		/// </summary>
		/// <returns><c>true</c> if this module has a next lesson; otherwise, <c>false</c>.</returns>
		/// <param name="currentLesson">Current lesson.</param>
		public bool HasNextLesson(Lesson currentLesson)
		{
			var index = Lessons.IndexOf(currentLesson);
			return Lessons.Count - 1 > index;
		}

		/// <summary>
		/// Gets the next lesson if available.
		/// </summary>
		/// <returns>The next lesson.</returns>
		/// <param name="currentLesson">Current lesson.</param>
		public Lesson GetNextLesson(Lesson currentLesson)
		{
			if (HasNextLesson(currentLesson))
			{
				var index = Lessons.IndexOf(currentLesson) + 1;
				return Lessons.ElementAt(index); 
			}

			return null;
		}

		/// <summary>
		/// Determines whether there is a previous lesson available from the provided lesson
		/// </summary>
		/// <returns><c>true</c> if this module has a previous lesson; otherwise, <c>false</c>.</returns>
		/// <param name="currentLesson">Current lesson.</param>
		public bool HasPreviousLesson(Lesson currentLesson)
		{
			var index = Lessons.IndexOf(currentLesson);
			return index > 0;
		}

		/// <summary>
		/// Gets the previoust lesson if available.
		/// </summary>
		/// <returns>The previoust lesson.</returns>
		/// <param name="currentLesson">Current lesson.</param>
		public Lesson GetPrevioustLesson(Lesson currentLesson)
		{
			if (HasPreviousLesson(currentLesson))
			{
				var index = Lessons.IndexOf(currentLesson) - 1;
				return Lessons.ElementAt(index);
			}

			return null;
		}
	}
}
