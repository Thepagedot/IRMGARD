using System;
using System.Runtime.CompilerServices;
using IRMGARD.Models;

namespace IRMGARD
{
    public class Progress
    {
        public IterationStatus Status { get; set; }
        public bool IsCurrent { get; set; }

        public Progress(IterationStatus status)
        {
            Status = status;
            IsCurrent = false;
        }
    }
}