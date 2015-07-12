using System;

namespace IRMGARD
{
	public class LessonData
	{
		private string letterToLearn;
		public string LetterToLearn
		{
			get { return letterToLearn; }
			set { LetterToLearn = letterToLearn; }
		}

		public LessonData (){}
		public LessonData (string letterToLearn)
		{
			LetterToLearn = letterToLearn;
		}
	}
}

