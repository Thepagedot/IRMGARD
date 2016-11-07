using System;
using System.Collections.Generic;

namespace IRMGARD.Models
{
    public class InputConcept : BaseConcept {
    }

    public class InputConceptIteration : BaseConceptIteration {

        // List of exercises for random selection
        public List<InputConceptExercise> Tasks { get; set; }
    }

    public class InputConceptExercise : BaseConceptExercise { }
}

