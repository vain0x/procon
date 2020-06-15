using System.Collections.Generic;
using System.Linq;

namespace Procon
{
    public static class RepeatedPermutationAlgorithm
    {
        private static IEnumerable<IReadOnlyList<T>> RepeatedPermutationsIterator<T>(int i, T[] source, T[] buffer)
        {
            if (i == buffer.Length)
            {
                yield return buffer;
            }
            else
            {
                foreach (var item in source)
                {
                    buffer[i] = item;

                    foreach (var list in RepeatedPermutationsIterator(i + 1, source, buffer))
                    {
                        yield return list;
                    }
                }
            }
        }

        /// <summary>
        /// Enumerates repeated permutations with the specified length.
        /// </summary>
        public static IEnumerable<IReadOnlyList<T>> RepeatedPermutations<T>(this IEnumerable<T> source, int count)
        {
            return RepeatedPermutationsIterator(0, source.ToArray(), new T[count]);
        }
    }
}
