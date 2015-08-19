using System;
using System.Collections.Generic;
using Android.Text;
using Android.Widget;
using Android.Text.Style;
using System.Linq;

namespace IRMGARD
{
	public static class Alphabet
	{
		public static List<string> Letters;

		static Alphabet()
		{
			Letters = new List<string> {
				"A",
				"B",
				"C",
				"D",
				"E",
				"F",
				"G",
				"H",
				"I",
				"J",
				"K",
				"L",
				"M",
				"N",
				"O",
				"P",
				"Q",
				"R",
				"S",
				"T",
				"U",
				"V",
				"W",
				"X",
				"Y",
				"Z"
			};
		}

        public static string GetRandomLetter()
        {
            var rand = new Random();
            return Letters.ElementAt(rand.Next(Letters.Count - 1));
        }

		public static SpannableString GetLettersMarked(List<string> markedLetters, bool capitalize)
		{
			var alphabet = "ABCDEFGHIJKLMNOPQRSTUVWXYZ";
			if (capitalize == false)
				alphabet = alphabet.ToLower();
			
            if (markedLetters == null)
                return new SpannableString(alphabet);

			var spannable = new SpannableString(alphabet);
			foreach (var letter in markedLetters)
			{
				var index = Letters.IndexOf(letter.ToUpper());
				spannable.SetSpan(new ForegroundColorSpan(Android.Graphics.Color.Red), index, index + 1, SpanTypes.ExclusiveExclusive);
			}

			return spannable;
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
	}        

    public enum Case { Ignore, Upper, Lower }
}

