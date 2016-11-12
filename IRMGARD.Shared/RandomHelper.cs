using System;
using System.Collections.Generic;
using System.Linq;

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

        /// <summary>
        /// Picks <paramref name="count"/> random items of this collection.
        /// </summary>
        /// <returns>
        /// A collection of random items of this collection.
        /// Using List instead of HashSet as the returned collection type,
        /// results in less type conversion work later.
        /// </returns>
        /// <param name="count">The count of items returned</param>
        public static IList<T> PickRandomItems<T>(this IEnumerable<T> src, int count)
        {
            List<int> idxItems = new List<int>(Enumerable.Range(0, src.Count()));
            var result = new List<T>();

            for (var i = 0; i < count; i++)
            {
                int idx = Rand.Next(idxItems.Count);
                result.Add(src.ElementAt(idxItems[idx]));
                idxItems.RemoveAt(idx);
            }

            return result;
        }
	}
}