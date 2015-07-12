using System;
using System.Collections.ObjectModel;

namespace IRMGARD.Models
{
	public class Module
	{
		private ObservableCollection<Lesson> lessonsList;
		public ObservableCollection<Lesson> LessonsList
		{
			get { return lessonsList; }
			set { LessonsList = lessonsList; }
		}

		private int numberOfLessons;
		public int NumberOfLessons
		{
			get { return numberOfLessons; }
			set { NumberOfLessons = numberOfLessons; }
		}

		private int numberOfLessonsDone;
		public int NumberOfLessonsDone
		{
			get { return numberOfLessonsDone; }
			set { NumberOfLessonsDone = numberOfLessonsDone; }
		}


		public Module (){}

		public Module (ObservableCollection<Lesson> lessonsList, int numberOfLessons, int numberOfLessonsDone)
		{
			LessonsList = lessonsList;
			NumberOfLessons = numberOfLessons;
			NumberOfLessonsDone = numberOfLessonsDone;
		}
	}
}
