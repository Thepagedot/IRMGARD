using System;

namespace IRMGARD.Models
{
	public class LevelElement
	{
		private string imagePath;
		public string ImagePath
		{
			get { return imagePath; }
			set { ImagePath = imagePath; }
		}

		private string soundPath;
		public string SoundPath
		{
			get { return soundPath; }
			set { SoundPath = soundPath; }
		}

		// needed for PickSyllable, HearMeAbc, Memory
		public LevelElement(string soundPath)
		{
			SoundPath = soundPath;
		}

		// needed for HearMe, FourPictures, AbcRank
		public LevelElement(string imagePath, string soundPath)
		{
			ImagePath = imagePath;
			SoundPath = soundPath;
		}
	}
}

