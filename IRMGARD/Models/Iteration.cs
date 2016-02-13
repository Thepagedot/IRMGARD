using System;
using System.Collections.Generic;

namespace IRMGARD.Models
{
	public class Iteration
	{
        public int Id { get; set; }
        public List<string> LettersToLearn { get; set; }
        public IterationStatus Status { get; set; }

	    protected Iteration() {}

	    protected Iteration (int id, List<string> lettersToLearn)
		{
            this.Id = id;
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