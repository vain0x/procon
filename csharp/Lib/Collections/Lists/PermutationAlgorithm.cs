using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace VainZero.Collections.Lists
{
    public static class PermutationAlgorithm
    {
        private static void InplaceSwap<X>(IList<X> list, int i, int j)
        {
            var t = list[i];
            list[i] = list[j];
            list[j] = t;
        }

        private static void InplaceReverse<X>(IList<X> list, int first, int count)
        {
            for (var i = 0; i < count / 2; i++)
            {
                InplaceSwap(list, first + i, first + count - 1 - i);
            }
        }

        /// <summary>
        /// Rearranges items into the next lexicographically greater permutation.
        /// Returns <c>true</c> if rearranged.
        /// </summary>
        public static bool NextPermutation<X>(this IList<X> list, IComparer<X> comparer)
        {
            var count = list.Count;
            if (count <= 1) return false;

            var i = count - 1;
            while (true)
            {
                var j = i;

                i--;

                if (comparer.Compare(list[i], list[j]) < 0)
                {
                    var k = count - 1;
                    while (comparer.Compare(list[i], list[k]) >= 0)
                    {
                        k--;
                    }

                    InplaceSwap(list, i, k);
                    InplaceReverse(list, j, count - j);
                    return true;
                }

                if (i == 0)
                {
                    InplaceReverse(list, 0, count);
                    return false;
                }
            }
        }

        /// <summary>
        /// Rearranges items into the next lexicographically greater permutation
        /// in the default order.
        /// Returns <c>true</c> if rearranged.
        /// </summary>
        public static bool NextPermutation<X>(this IList<X> list)
        {
            return NextPermutation(list, Comparer<X>.Default);
        }

        /// <summary>
        /// Generates all permutations.
        /// </summary>
        public static IEnumerable<IReadOnlyList<X>> Permutations<X>(this IEnumerable<X> xs, IComparer<X> comparer)
        {
            var array = xs.ToArray();
            Array.Sort(array, comparer);

            do
            {
                yield return array;
            }
            while (NextPermutation(array, comparer));
        }

        /// <summary>
        /// Generates all permutations.
        /// </summary>
        public static IEnumerable<IReadOnlyList<X>> Permutations<X>(this IEnumerable<X> xs)
        {
            return Permutations(xs, Comparer<X>.Default);
        }
    }
}
