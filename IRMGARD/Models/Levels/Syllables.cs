using System;
using System.Collections.ObjectModel;

namespace IRMGARD
{
	public class Syllables
	{
		public ObservableCollection<String> SyllableParts;
		public void setSyllableParts(ObservableCollection<String> syllableParts){
			this.SyllableParts = syllableParts;
		}
		public ObservableCollection<String> getSyllableParts(){
			return SyllableParts;
		}

		public String SoundPath;
		public void setSoundPath(String soundPath){
			this.SoundPath = soundPath;
		}
		public String getSoundPath(){
			return SoundPath;
		}

		public Syllables(){}

		// needed for BuildSyllables
		public Syllables (ObservableCollection<string> syllableParts, string soundPath)
		{
			this.setSyllableParts(syllableParts);
			this.setSoundPath(soundPath);
		}
	}
}

