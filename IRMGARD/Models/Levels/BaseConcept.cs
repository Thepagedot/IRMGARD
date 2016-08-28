using System;
using System.Collections.Generic;

namespace IRMGARD.Models
{
    public class BaseConcept : Lesson
    {
        // Global option items
        public List<Concept> OptionItems { get; set; }

        // Do not show the wooden rack
        public bool HideRack { get; set; }

        // Idx = 0: The top margin (as dp) of the first task item row
        // Idx = 1: The top margin of each task item row (incl. the first row)
        // Idx = 2: The margin between task and option items (optional)
        // Idx = 3: The margin between option and solution items (optional)
        public int[] TopMargins { get; set; }
    }

    public abstract class BaseConceptIteration : Iteration
    {
        // The space (as dp) between each TaskItemRow
        public int TopMargin { get; set; }
    }

    public class BaseConceptExercise
    {
        // Rows of task item elements
        public List<List<Concept>> TaskItems { get; set; }

        // Task specific option items
        public List<Concept> OptionItems { get; set; }

        public virtual BaseConceptExercise DeepCopy()
        {
            BaseConceptExercise clone = (BaseConceptExercise)this.MemberwiseClone();
            if (TaskItems != null && TaskItems.Count > 0)
            {
                clone.TaskItems = new List<List<Concept>>();
                foreach (var taskItemRow in TaskItems)
                {
                    var row = new List<Concept>();
                    foreach (var concept in taskItemRow)
                    {
                        row.Add(concept.DeepCopy());
                    }
                    clone.TaskItems.Add(row);
                }
            }
            if (OptionItems != null && OptionItems.Count > 0)
            {
                clone.OptionItems = new List<Concept>();
                foreach (var concept in OptionItems)
                {
                    clone.OptionItems.Add(concept.DeepCopy());
                }
            }

            return clone;
        }
    }
}

