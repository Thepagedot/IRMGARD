using System;
using System.Collections.ObjectModel;
using Newtonsoft.Json;

namespace IRMGARD.Models
{
	public class Module
	{
		public ObservableCollection<Lesson> LessonsList;
		public void setLessonsList(ObservableCollection<Lesson> lessonsList){
			this.LessonsList = lessonsList;
		}
		public ObservableCollection<Lesson> getLessonsList(){
			return this.LessonsList;
		}

		public int NumberOfLessons;
		public void setNumberOfLessons(int numberOfLessons){
			this.NumberOfLessons = numberOfLessons;
		}
		public int getNumberOfLessons(){
			return this.NumberOfLessons;
		}

		public int NumberOfLessonsDone;
		public void setNumberOfLessonsDone(int numberOfLessonsDone){
			this.NumberOfLessonsDone = numberOfLessonsDone;
		}
		public int getNumberOfLessonsDone(){
			return this.NumberOfLessonsDone;
		}


		public Module (){}

		public Module (ObservableCollection<Lesson> lessonsList, int numberOfLessons, int numberOfLessonsDone)
		{
			this.setLessonsList(lessonsList);
			this.setNumberOfLessons(numberOfLessons);
			this.setNumberOfLessonsDone(numberOfLessonsDone);
		}
	}
}
