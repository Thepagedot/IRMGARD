using System;
using System.Collections.ObjectModel;

namespace IRMGARD
{
	public class Syllables
	{
		private ObservableCollection<string> syllableParts;
		public ObservableCollection<string> SyllableParts
		{
			get { return syllableParts; }
			set { SyllableParts = syllableParts; }
		}

		private string soundPath;
		public string SoundPath
		{
			get { return soundPath; }
			set { SoundPath = soundPath; }
		}


		// needed for BuildSyllables
		public Syllables (ObservableCollection<string> syllableParts, string soundPath)
		{
			SyllableParts = syllableParts;
			SoundPath = soundPath;
		}
	}
}

