using System.Collections.Generic;

namespace IRMGARD.Models
{
	public class PickSyllable : Lesson
	{
        public string SyllablePath { get; set; }

        public PickSyllable () {}

        public PickSyllable (int id, string title, string soundPath, string syllablePath, string hint, LevelType typeOfLevel, List<Iteration> iterations) : base (id, title, soundPath, hint, typeOfLevel, iterations)
		{
            this.SyllablePath = syllablePath;
		}
	}

    public class PickSyllableIteration : Iteration
    {
        public List<string[]> SyllablesToLearn { get; set; }

        public PickSyllableIteration(List<string> lettersToLearn, List<string[]> syllablesToLearn) : base (lettersToLearn)
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
