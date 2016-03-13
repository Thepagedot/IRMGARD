using System;
using System.Collections.Generic;

namespace IRMGARD.Models
{
    public class HearMeAbc : Lesson
    {
        public string SoundPathABC { get; set; }

        public HearMeAbc() { }

        public HearMeAbc(int id, string title, bool isRecurringTask, string soundPath, string soundPathABC, string hint, LevelType typeOfLevel, List<Iteration> iterations)
            : base (id, title, isRecurringTask, soundPath, hint, typeOfLevel, iterations)
        {
            SoundPathABC = soundPathABC;
        }
    }

    public class HearMeAbcIteration : Iteration
    {
        public string SoundPath { get; set; }
        public List<HearMeAbcLetter> Letters { get; set; }

        public HearMeAbcIteration(int id, List<string> lettersToLearn, string soundPath, List<HearMeAbcLetter> letters) : base(id, lettersToLearn)
        {
            SoundPath = soundPath;
            Letters = letters;
        }
    }

    public class HearMeAbcLetter
    {
        public string Letter { get; set; }
        public string Prepend { get; set; }
        public string Append { get; set; }
        public Media Media { get; set; }

        public HearMeAbcLetter(int id, string letter, string prepend, string append, Media media)
        {
            Letter = letter;
            Prepend = prepend;
            Append = append;
            Media = media;
        }
    }
}
