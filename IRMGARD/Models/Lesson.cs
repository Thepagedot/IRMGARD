using System;
using System.Collections.ObjectModel;

namespace IRMGARD.Models
{
	public class Lesson
	{
		public String Title;
		public void setTitle(String title){
			this.Title=title;
		}
		public String getTitle(){
			return this.Title;
		}

		public String SoundPath;
		public void setSoundPath(String soundPath){
			this.SoundPath=soundPath;
		}
		public String getSoundPath(){
			return this.SoundPath;
		}

		public String Hint;
		public void setHint(String hint){
			this.Hint=hint;
		}
		public String getHint(){
			return this.Hint;
		}

		public LevelType TypeOfLevel;
		public void setTypeOfLevel(LevelType levelType){
			this.TypeOfLevel = levelType;
		}
		public LevelType getTypeOfLevel(){
			return this.TypeOfLevel;
		}

		public LessonData Data;
		public void setData(LessonData data){
			this.Data = data;
		}
		public LessonData getData(){
			return this.Data;
		}

		public Lesson (){}

		public Lesson (string title, string soundPath, string hint, LevelType typeOfLevel, LessonData data)
		{
			this.setTitle(title);
			this.setSoundPath(soundPath);
			this.setHint(hint);
			this.setTypeOfLevel(typeOfLevel);
			this.setData(data);
		}
	}
}

