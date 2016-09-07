using System;
using System.Collections.Generic;

namespace IRMGARD.Models
{
    public class SelectConcept : BaseConcept {

        // Selection of one item only 
        public bool SingleChoice { get; set; }
    }

    public class SelectConceptIteration : BaseConceptIteration {

        // List of exercises for random selection
        public List<SelectConceptExercise> Tasks { get; set; }
    }

    public class SelectConceptExercise : BaseConceptExercise { }
}

