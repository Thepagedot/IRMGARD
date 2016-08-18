using System;
using System.Collections.Generic;

namespace IRMGARD.Models
{
    public class DragIntoGap : Lesson
    {
        // Global option items
        public List<Concept> OptionItems { get; set; }

        // If the solution is not correct, the correct options are highlighted
        public bool HighlightCorrectOptionsOnMistake { get; set; }

        // The pool of option items presented do not include items picked from blank fields
        public bool UseOptionItemsOnly { get; set; }

        // Do not show the wooden rack
        public bool HideRack { get; set; }
    }

    public class DragIntoGapIteration : Iteration
    {
        // List of exercises for random selection
        public List<Exercise> Tasks { get; set; }
    }

    public class Exercise
    {
        // Rows of task item elements
        public List<List<Concept>> TaskItems { get; set; }

        // Task specific option items
        public List<Concept> OptionItems { get; set; }
    }
}

