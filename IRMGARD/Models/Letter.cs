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

            // Make letter empty it when is searched
            if (IsSearched)
                Letter = "";
        }

        [JsonConstructor]
        protected TaskLetter(string letter, bool isShort, bool isLong, bool isSearched) : base(letter, isShort, isLong)
        {
            IsSearched = isSearched;
            CorrectLetter = letter;

            // Make letter empty it when is searched
            if (IsSearched)
                Letter = "";
        }
    }
}