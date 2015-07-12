using System;
using System.Collections.ObjectModel;

namespace IRMGARD.Models
{
	public class BuildSyllable : Lesson
	{
		private ObservableCollection<Syllables> syllablesList;
		public ObservableCollection<Syllables> SyllablesList
		{
			get { return syllablesList; }
			set { SyllablesList = syllablesList; }
		}

		private ObservableCollection<string> options;
		public ObservableCollection<string> Options
		{
			get { return options; }
			set { Options = options; }
		} 

		public BuildSyllable(){}

		public BuildSyllable(ObservableCollection<Syllables> syllableList, ObservableCollection<string> options)
		{
			SyllablesList = syllableList;
			Options = options;
		}
	}
}

