using System;
using System.Collections.Generic;
using Android.Text;
using Android.Widget;
using Android.Text.Style;

namespace IRMGARD
{
	public static class Alphabet
	{
		static List<string> letters;

		static Alphabet()
		{
			letters = new List<string> {
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

		public static SpannableString GetLettersMarked(List<string> markedLetters, bool capitalize)
		{
			var alphabet = "ABCDEFGHIJKLMNOPQRSTUVWXYZ";
			if (capitalize == false)
				alphabet = alphabet.ToLower();
				

			var spannable = new SpannableString(alphabet);
			foreach (var letter in markedLetters)
			{
				var index = letters.IndexOf(letter.ToUpper());
				spannable.SetSpan(new ForegroundColorSpan(Android.Graphics.Color.Red), index, index + 1, SpanTypes.ExclusiveExclusive);
			}

			return spannable;
		}
	}
}

