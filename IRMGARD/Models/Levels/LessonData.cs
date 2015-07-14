using System;
using System.Collections.ObjectModel;

namespace IRMGARD.Models
{
	public class LessonData
	{
		public String LetterToLearn;
		public void setLetterToLearn(String letterToLearn){
			this.LetterToLearn = letterToLearn;
		}
		public String getLetterToLearn(){
			return this.LetterToLearn;
		}

		public LevelOption Option;
		public void setOptions(LevelOption option){
			this.Option = option;
		}
		public LevelOption getOption(){
			return this.Option;
		}

		public ObservableCollection<LevelOption> OptionList;
		public void setOptionList(ObservableCollection<LevelOption> optionList){
			this.OptionList = optionList;
		}
		public ObservableCollection<LevelOption> getOptionList(){
			return this.OptionList;
		}

		public LessonData (){}
		public LessonData (String letterToLearn)
		{
			setLetterToLearn (letterToLearn);
		}

		public LessonData(String letterToLearn, LevelOption option)
		{
			this.setLetterToLearn (letterToLearn);
			this.setOptions (option);
		}

		public LessonData(String letterToLearn, ObservableCollection<LevelOption> optionList)
		{
			this.setLetterToLearn (letterToLearn);
			this.setOptionList (optionList);
		}
	}
}

