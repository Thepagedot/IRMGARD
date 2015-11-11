using System;
using Newtonsoft.Json;

namespace IRMGARD
{
    public class LetterBase
    {
        public string Letter { get; set; }
        public bool IsShort { get; set; }
        public bool IsLong { get; set; }

        public LetterBase(string letter)
        {
            Letter = letter;
            IsShort = false;
            IsLong = false;
        }

        [JsonConstructor]
        public LetterBase(string letter, bool isShort, bool isLong)
        {
            Letter = letter;
            IsShort = isShort;
            IsLong = isLong;
        }
    }

    public class TaskLetter : LetterBase
    {        
        public string CorrectLetter { get; set; }
        public bool IsCorrect { get; set; }

        public TaskLetter(string letterString) : base(letterString)
        {
            CorrectLetter = letterString;
        }

        [JsonConstructor]
        protected TaskLetter(string letter, bool isShort, bool isLong) : base(letter, isShort, isLong)
        {
            CorrectLetter = letter;
        }
    }
}