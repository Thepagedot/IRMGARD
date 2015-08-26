using System;

namespace IRMGARD
{
    public class LetterBase
    {
        public string Letter { get; set; }
        public bool IsShort { get; set; }
        public bool IsLong { get; set; }

        public LetterBase(string letterString)
        {
            Letter = letterString;
            IsShort = false;
            IsLong = false;
        }

        public LetterBase(string letterString, bool isShort, bool isLong)
        {
            Letter = letterString;
            IsShort = isShort;
            IsLong = isLong;
        }
    }

    public abstract class TaskLetter : LetterBase
    {
        public bool IsSearched { get; set; }
        public string CorrectLetter { get; set; }
        public bool IsCorrect { get; set; }

        protected TaskLetter(string letterString) : base (letterString)
        {
            IsSearched = false;
            CorrectLetter = letterString;
        }

        protected TaskLetter(string letterString, bool isSearched, string correctLetter) : base (letterString)
        {
            IsSearched = isSearched;
            CorrectLetter = correctLetter;
        }

        protected TaskLetter(string letterString, bool isShort, bool isLong, bool isSearched, string correctLetter) : base(letterString, isShort, isLong)
        {
            IsSearched = isSearched;
            CorrectLetter = correctLetter;
        }
    }
}