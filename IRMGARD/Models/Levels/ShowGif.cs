using System;
using System.Collections.Generic;

namespace IRMGARD.Models
{
    public class ShowGif : Lesson
    {
        public ShowGif () {}

        public ShowGif (int id, string title, bool isRecurringTask, string soundPath, string hint, LevelType typeOfLevel, List<Iteration> iterations)
            : base (id, title, isRecurringTask, soundPath, hint, typeOfLevel, iterations)
        {
        }
    }

    public class ShowGifIteration : Iteration
    {
        public List<GifTask> GifTasks { get; set; }

        public ShowGifIteration (int id, List<string> lettersToLearn, List<GifTask> gifTasks) : base (id, lettersToLearn)
        {
            this.GifTasks = gifTasks;
        }
    }

    public class GifTask
    {
        public string Path { get; set; }

        public GifTask (string path)
        {
            this.Path = path;
        }
    }
}

