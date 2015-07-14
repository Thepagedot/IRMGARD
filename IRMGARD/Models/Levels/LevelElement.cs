using System;

namespace IRMGARD.Models
{
	public class LevelElement
	{
		public String ImagePath;
		public void setImagePath(String imagePath){
			this.ImagePath = imagePath;
		}
		public String getImagePath(){
			return this.ImagePath;
		}

		public String SoundPath;
		public void setSoundPath(String soundPath){
			this.SoundPath = soundPath;
		}
		public String getSoundPath(){
			return this.SoundPath;
		}

		public LevelElement(){}

		// needed for PickSyllable, HearMeAbc, Memory
		public LevelElement(string soundPath)
		{
			this.setSoundPath(soundPath);
		}

		// needed for HearMe, FourPictures, AbcRank
		public LevelElement(string imagePath, string soundPath)
		{
			this.setImagePath(imagePath);
			this.setSoundPath(soundPath);
		}
	}
}

