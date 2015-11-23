using System;

namespace IRMGARD.Models
{
	public class Media
	{
		public string ImagePath;
		public string SoundPath;

		public Media () {}

		public Media (string imagePath, string soundPath)
		{
			this.ImagePath = imagePath;
			this.SoundPath = soundPath;
		}
	}
}

