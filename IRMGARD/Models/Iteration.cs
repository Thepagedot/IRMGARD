using System;
using System.Collections.Generic;

namespace IRMGARD.Models
{
	public class Iteration
	{
		public List<string> LettersToLearn;

	    protected Iteration() {}

	    protected Iteration (List<string> lettersToLearn)
		{
			this.LettersToLearn = lettersToLearn;
		}
	}
}