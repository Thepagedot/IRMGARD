using System.Collections.Generic;

namespace IRMGARD.Models
{
	public class PickSyllable : Lesson
	{
        public string SyllablePath { get; set; }

        public PickSyllable () {}

        public PickSyllable (int id, string title, bool isRecurringTask, string soundPath, string syllablePath, string hint, LevelType typeOfLevel, List<Iteration> iterations)
            : base (id, title, isRecurringTask, soundPath, hint, typeOfLevel, iterations)
		{
            this.SyllablePath = syllablePath;
		}
	}

    public class PickSyllableIteration : Iteration
    {
        public List<string[]> SyllablesToLearn { get; set; }

        public PickSyllableIteration(int id, List<string> lettersToLearn, List<string[]> syllablesToLearn) : base (id, lettersToLearn)
        {
            SyllablesToLearn = syllablesToLearn;
        }
    }

	public class PickSyllableOption
    {
		public bool IsCorrect { get; set; }
		public Media Media  { get; set; }

		public PickSyllableOption () {}

		public PickSyllableOption (bool isCorrect, Media media)
		{
			this.IsCorrect = isCorrect;
			this.Media = media;
		}
	}
}
