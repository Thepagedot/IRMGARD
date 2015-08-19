using System;

namespace IRMGARD
{
    public class LetterBase
    {
        public string Letter { get; set; }
        public bool IsShort { get; set; }
        public bool IsLong { get; set; }

        public LetterBase() {}

        public LetterBase(string letterString)
        {
            this.Letter = letterString;
            this.IsShort = false;
            this.IsLong = false;
        }

        public LetterBase(string letterString, bool isShort, bool isLong)
        {
            this.Letter = letterString;
            this.IsShort = isShort;
            this.IsLong = isLong;
        }
    }
}