using System;
using System.Collections.Generic;
using System.Linq;

namespace IRMGARD.Shared
{
	public static class Alphabet
	{
		public static List<string> Letters;
        public static Random Rand = new Random();

		static Alphabet()
		{
			Letters = new List<string> { "A", "B", "C", "D", "E", "F", "G", "H", "I", "J", "K", "L", "M", "N", "O", "P", "Q", "R", "S", "T", "U", "V", "W", "X", "Y", "Z" };
		}

        public static string GetRandomLetter()
        {
            return Letters.ElementAt(Rand.Next(Letters.Count - 1));
        }	 

        public static string ToCase(this string letter, Case fontCase)
        {
            switch (fontCase)
            {                
                default:
                    return letter;
                case Case.Upper:
                    return letter.ToUpper();
                case Case.Lower:
                    return letter.ToLower();
            }
        }

        public static string ToNegativeCase(this string letter, Case fontCase)
        {
            switch (fontCase)
            {                
                default:
                    return letter;
                case Case.Upper:
                    return letter.ToLower();
                case Case.Lower:
                    return letter.ToUpper();
            }
        }

        public static Case GetCase(this string letter)
        {
            // All Alphabet letters are upper case sothat we can simply check
            // if they contain the given letter.
            return Alphabet.Letters.Contains(letter) ? Case.Upper : Case.Lower;
        }
	}        

    public enum Case { Ignore, Upper, Lower }
}