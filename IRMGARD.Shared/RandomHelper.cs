using System;
using System.Collections.Generic;

namespace IRMGARD.Shared
{
	public static class RandomHelper
	{
		private static readonly Random Rand = new Random();  

		public static void Shuffle<T>(this IList<T> list)  
		{  
			var n = list.Count;
			while (n > 1)
            {  
				n--;
				var k = Rand.Next(n + 1);
				T value = list[k];

				list[k] = list[n];
				list[n] = value;
			}  
		}
	}
}