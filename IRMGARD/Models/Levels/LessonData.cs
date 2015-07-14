using System;

namespace IRMGARD
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

		public LessonData (){}
		public LessonData (String letterToLearn)
		{
			setLetterToLearn (letterToLearn);
		}
	}
}

