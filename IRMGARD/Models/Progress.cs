using System;

namespace IRMGARD
{
    public class Progress
    {
        public ProgressStatus Status { get; set; }

        public Progress(ProgressStatus status)
        {
            Status = status;
        }
    }

    public enum ProgressStatus
    {
        Pending,
        Success,
        Failed
    }
}