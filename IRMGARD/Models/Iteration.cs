using System;
using System.Collections.Generic;

namespace IRMGARD.Models
{
	public class Iteration
	{
		public List<string> LettersToLearn;
        public IterationStatus Status { get; set; }

	    protected Iteration() {}

	    protected Iteration (List<string> lettersToLearn)
		{
			this.LettersToLearn = lettersToLearn;
		}
	}

    public enum IterationStatus
    {
        Pending,
        Success,
        Failed
    }
}