using System;
using System.Runtime.CompilerServices;

namespace IRMGARD
{
    public class Progress
    {
        public ProgressStatus Status { get; set; }
        public bool IsCurrent { get; set; }

        public Progress(ProgressStatus status)
        {
            Status = status;
            IsCurrent = false;
        }
    }

    public enum ProgressStatus
    {
        Pending,
        Success,
        Failed
    }
}