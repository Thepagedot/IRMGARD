using System;
using System.Collections.Generic;
using System.Linq;

using IRMGARD.Models;
using IRMGARD.Shared;

namespace IRMGARD
{
    public class SpeakerSelectConceptFragment : SelectConceptFragment
    {
        protected override void TransformTaskItems()
        {
            exercise = exercise.DeepCopy();

            var concepts = new List<Concept>();
            concepts.AddRange(exercise.TaskItems[0]);
            concepts.AddRange(Lesson.OptionItems.PickRandomItems(4).Select(item => { item.IsOption = true; return item; }));
            concepts.Shuffle();

            exercise.TaskItems = new List<List<Concept>>();
            exercise.TaskItems.Add(concepts.GetRange(0, 3));
            exercise.TaskItems.Add(concepts.GetRange(3, 3));
            exercise.TaskItems.Add(concepts.GetRange(6, 3));
        }
    }
}