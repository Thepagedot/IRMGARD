using System;
using System.Collections.Generic;

namespace IRMGARD.Models
{
    public class LetterWrite : Lesson
    {
        public LetterWrite () {}

        public LetterWrite (int id, string title, bool isRecurringTask, string soundPath, string hint, LevelType typeOfLevel, List<Iteration> iterations)
            : base (id, title, isRecurringTask, soundPath, hint, typeOfLevel, iterations)
        {
        }
    }

    public class LetterWriteIteration : Iteration
    {
        public List<LetterWriteTask> TasksItems { get; set; }

        public LetterWriteIteration (int id, List<string> lettersToLearn, List<LetterWriteTask> gifTasks) : base (id, lettersToLearn)
        {
            this.TasksItems = gifTasks;
        }
    }

    public class LetterWriteTask
    {
        public string Path { get; set; }

        public LetterWriteTask (string path)
        {
            this.Path = path;
        }
    }
}

