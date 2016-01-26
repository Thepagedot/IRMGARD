﻿using System;
using System.Collections.Generic;

namespace IRMGARD.Models
{
    public class Memory : Lesson
    {
        public List<MemoryOption> Options { get; set; }

        public Memory() {}

        public Memory(int id, string title, string soundPath, string hint, LevelType typeOfLevel, List<Iteration> iterations, List<MemoryOption> options)
            : base (id, title, soundPath, hint, typeOfLevel, iterations)
        {
            this.Options = options;
        }
    }

    public class MemoryIteration : Iteration
    {
        public MemoryIteration (List<string> lettersToLearn, List<MemoryOption> options) : base (lettersToLearn)
        {
        }
    }

    public class MemoryOption
    {
        public string Name { get; set; }
        public Media Media { get; set; }

        public MemoryOption () {}

        public MemoryOption (string name, Media media)
        {
            this.Name = name;
            this.Media = media;
        }
    }
}

