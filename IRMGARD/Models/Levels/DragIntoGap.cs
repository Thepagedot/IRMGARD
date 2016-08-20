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

        // Idx = 0: The top margin (as dp) of the first task item row
        // Idx = 1: The top margin of each task item row (incl. the first row)
        // Idx = 2: The margin between task and option items (optional)
        // Idx = 3: The margin between option and solution items (optional)
        public int[] TopMargins { get; set; }
    }

    public class DragIntoGapIteration : Iteration
    {
        // List of exercises for random selection
        public List<Exercise> Tasks { get; set; }

        // The space (as dp) between each TaskItemRow
        public int TopMargin { get; set; }
    }

    public class Exercise
    {
        // Rows of task item elements
        public List<List<Concept>> TaskItems { get; set; }

        // Task specific option items
        public List<Concept> OptionItems { get; set; }
    }
}

