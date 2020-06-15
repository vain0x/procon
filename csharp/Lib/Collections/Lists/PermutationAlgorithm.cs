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

    public static bool NextPermutation<T>(this IList<T> list) =>
        NextPermutation(list, Comparer<T>.Default);

    public static IEnumerable<IReadOnlyList<T>> AllPermutations<T>(this IEnumerable<T> source, IComparer<T> comparer)
    {
        var array = source.ToArray();
        Array.Sort(array, comparer);

        do
        {
            yield return array;
        }
        while (NextPermutation(array, comparer));
    }

    public static IEnumerable<IReadOnlyList<T>> AllPermutations<T>(this IEnumerable<T> source) =>
        AllPermutations(source, Comparer<T>.Default);
}
