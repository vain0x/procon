namespace Procon
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Text;
    using System.Threading.Tasks;

    public static class RepeatedPermutationAlgorithm
    {
        private static IEnumerable<IReadOnlyList<X>> RepeatedPermutationsIterator<X>(int i, X[] source, X[] buffer)
        {
            if (i == buffer.Length)
            {
                yield return buffer;
            }
            else
            {
                foreach (var x in source)
                {
                    buffer[i] = x;

                    foreach (var xs in RepeatedPermutationsIterator(i + 1, source, buffer))
                    {
                        yield return xs;
                    }
                }
            }
        }

        /// <summary>
        /// Enumerates repeated permutations with the specified length.
        /// </summary>
        public static IEnumerable<IReadOnlyList<X>> RepeatedPermutations<X>(this IEnumerable<X> @this, int count)
        {
            return RepeatedPermutationsIterator(0, @this.ToArray(), new X[count]);
        }
    }
}
