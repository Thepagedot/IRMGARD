using System;
using System.Collections.Generic;

namespace IRMGARD.Models
{
    public class BaseConcept : Lesson
    {
        // Do not show the wooden rack
        public bool HideRack { get; set; }

        // Use two columns (value set width (in dpi) for the first column)
        public int TwoColumns { get; set; }

        // Idx = 0: The top margin (as dp) of the first task item row
        // Idx = 1: The top margin of each task item row (incl. the first row)
        // Idx = 2: The margin between task and option items (optional)
        // Idx = 3: The margin between option and solution items (optional)
        public int[] TopMargins { get; set; }

        // Align task row items to the left rather than centered
        public bool[] LeftAlignItems { get; set; }

        // Global option items
        public List<Concept> OptionItems { get; set; }

        // Global concept items
        public Dictionary<string, List<Concept>> NamedConcepts { get; set; }

        public virtual BaseConcept DeepCopy()
        {
            BaseConcept clone = (BaseConcept)this.MemberwiseClone();
            clone.HideRack = HideRack;
            if (TopMargins != null && TopMargins.Length > 0)
            {
                clone.TopMargins = new int[TopMargins.Length];
                Array.Copy(TopMargins, clone.TopMargins, TopMargins.Length);
            }
            if (OptionItems != null && OptionItems.Count > 0)
            {
                clone.OptionItems = new List<Concept>();
                foreach (var concept in OptionItems)
                {
                    clone.OptionItems.Add(concept.DeepCopy());
                }
            }
            if (NamedConcepts != null && NamedConcepts.Count > 0)
            {
                clone.NamedConcepts = new Dictionary<string, List<Concept>>();
                foreach (var pair in NamedConcepts)
                {
                    clone.NamedConcepts[pair.Key] = new List<Concept>();
                    foreach (var concept in pair.Value)
                    {
                        clone.NamedConcepts[pair.Key].Add(concept.DeepCopy());
                    }
                }
            }

            return clone;
        }
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

