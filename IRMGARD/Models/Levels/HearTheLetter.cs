using System;
using System.Collections.Generic;

namespace IRMGARD.Models
{
    public class HearTheLetter : Lesson
    {
        public HearTheLetter () {}

        public HearTheLetter(int id, string title, string soundPath, string hint, LevelType typeOfLevel, List<Iteration> iterations)
            : base (id, title, soundPath, hint, typeOfLevel, iterations) {}
    }

    public class HearTheLetterIteration : Iteration
    {
        public List<HearTheLetterOption> LetterLocations { get; set; }

        public HearTheLetterIteration(List<string> lettersToLearn, List<HearTheLetterOption> letterLocations) : base(lettersToLearn)
        {
            this.LetterLocations = letterLocations;
        }
    }

    public class HearTheLetterOption
    {
        public int Location { get; set; }
        public string SoundPath { get; set; }

        public HearTheLetterOption () {}

        public HearTheLetterOption(int location, string soundPath)
        {
            this.Location = location;
            this.SoundPath = soundPath;
        }
    }
}
