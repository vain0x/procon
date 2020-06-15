using System;
using System.Collections.Generic;
using System.Linq;

public static class PermutationAlgorithm
{
    private static void InPlaceSwap<T>(IList<T> list, int i, int j)
    {
        var t = list[i];
        list[i] = list[j];
        list[j] = t;
    }

    private static void InPlaceReverse<T>(IList<T> list, int first, int count)
    {
        for (var i = 0; i < count / 2; i++)
        {
            InPlaceSwap(list, first + i, first + count - 1 - i);
        }
    }

    /// <summary>
    /// Rearranges items into the next lexicographically greater permutation.
    /// Returns <c>true</c> if rearranged.
    /// </summary>
    public static bool NextPermutation<T>(this IList<T> list, IComparer<T> comparer)
    {
        var count = list.Count;
        if (count <= 1)
            return false;

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

                InPlaceSwap(list, i, k);
                InPlaceReverse(list, j, count - j);
                return true;
            }

            if (i == 0)
            {
                InPlaceReverse(list, 0, count);
                return false;
            }
        }
    }

    /// <summary>
    /// Rearranges items into the next lexicographically greater permutation
    /// in the default order.
    /// Returns <c>true</c> if rearranged.
    /// </summary>
    public static bool NextPermutation<T>(this IList<T> list)
    {
        return NextPermutation(list, Comparer<T>.Default);
    }

    /// <summary>
    /// Generates all permutations.
    /// </summary>
    public static IEnumerable<IReadOnlyList<T>> Permutations<T>(this IEnumerable<T> source, IComparer<T> comparer)
    {
        var array = source.ToArray();
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
    public static IEnumerable<IReadOnlyList<T>> Permutations<T>(this IEnumerable<T> source) =>
        Permutations(source, Comparer<T>.Default);
}
