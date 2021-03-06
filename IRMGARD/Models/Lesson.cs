﻿using System;
using System.Collections.ObjectModel;
using System.Collections.Generic;
using System.Linq;

namespace IRMGARD.Models
{
    public abstract class Lesson
    {
        public int Id { get; set; }
        public string Title { get; set; }
        public bool IsRecurringTask { get; set; }
        public string SoundPath { get; set; }
        public string Hint { get; set; }
        public LevelType TypeOfLevel { get; set; }
        public List<Iteration> Iterations { get; set; }
        public bool IsCompleted
        {
            get { return Iterations.All(i => i.Status == IterationStatus.Success); }
        }
        public bool IsDirty
        {
            get { return Iterations.Any(i => i.Status != IterationStatus.Pending); }
        }

        protected Lesson() { }

        protected Lesson(int id, string title, bool isRecurringTask, string soundPath, string hint, LevelType typeOfLevel, List<Iteration> iterations)
        {
            this.Id = id;
            this.Title = title;
            this.IsRecurringTask = isRecurringTask;
            this.SoundPath = soundPath;
            this.Hint = hint;
            this.TypeOfLevel = typeOfLevel;
            this.Iterations = iterations;
        }
    }
}