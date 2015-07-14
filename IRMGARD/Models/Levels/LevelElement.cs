using System;

namespace IRMGARD.Models
{
	public class LevelElement
	{
		public string ImagePath;
		public string SoundPath;

		public LevelElement () {}

		public LevelElement (string imagePath, string soundPath)
		{
			this.ImagePath = imagePath;
			this.SoundPath = soundPath;
		}
	}
}

