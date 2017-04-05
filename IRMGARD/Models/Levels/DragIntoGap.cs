using System;
using System.Collections.Generic;

namespace IRMGARD.Models
{
    public class DragIntoGap : BaseConcept
    {
        // If the solution is not correct, the correct options are highlighted
        public bool HighlightCorrectOptionsOnMistake { get; set; }

        // The pool of option items presented do not include solution items
        public bool UseOptionItemsOnly { get; set; }
    }

    public class DragIntoGapIteration : BaseConceptIteration
    {
        // List of exercises for random selection
        public List<DragIntoGapExercise> Tasks { get; set; }
    }

    public class DragIntoGapExercise : BaseConceptExercise { }
}

