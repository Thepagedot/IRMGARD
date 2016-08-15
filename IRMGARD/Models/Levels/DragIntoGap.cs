using System;
using System.Collections.Generic;

namespace IRMGARD.Models
{
    public class DragIntoGap : Lesson
    {
        public List<Concept> OptionItems { get; set; }
        public bool HighlightCorrectOptionsOnMistake { get; set; }
    }

    public class DragIntoGapIteration : Iteration
    {
        public List<List<Concept>> TaskItems { get; set; }
        public List<Concept> OptionItems { get; set; }
        public int NumberOfTaskItemsToShow { get; set; }
    }
}

