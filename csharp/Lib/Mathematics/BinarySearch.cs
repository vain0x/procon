using System;
using System.Collections.Generic;

public static class BinarySearch
{
    // <https://twitter.com/meguru_comp/status/697008509376835584">
    public static int Meguru(int ok, int ng, Func<int, bool> isOk)
    {
        while (Math.Abs(ok - ng) > 1)
        {
            var mid = (ok + ng) / 2;
            if (isOk(mid))
            {
                ok = mid;
            }
            else
            {
                ng = mid;
            }
        }
        return ok;
    }

    public static long Meguru(long ok, long ng, Func<long, bool> isOk)
    {
        while (Math.Abs(ok - ng) > 1)
        {
            var mid = (ok + ng) / 2;
            if (isOk(mid))
            {
                ok = mid;
            }
            else
            {
                ng = mid;
            }
        }
        return ok;
    }

    public static int LowerBound<T>(this IReadOnlyList<T> list, T value, int lb, int ub, IComparer<T> comparer)
    {
        if (lb > ub)
            throw new ArgumentOutOfRangeException(nameof(ub));

        while (lb != ub)
        {
            var m = lb + (ub - lb) / 2;
            if (comparer.Compare(list[m], value) < 0)
            {
                lb = m + 1;
            }
            else
            {
                ub = m;
            }
        }
        return lb;
    }

    public static int LowerBound<T>(this IReadOnlyList<T> list, T value, IComparer<T> comparer) =>
        LowerBound(list, value, 0, list.Count, comparer);

    public static int LowerBound<T>(this IReadOnlyList<T> list, T value, int lb, int ub) =>
        LowerBound(list, value, lb, ub, Comparer<T>.Default);

    public static int LowerBound<T>(this IReadOnlyList<T> list, T value) =>
        LowerBound(list, value, 0, list.Count, Comparer<T>.Default);

    public static int UpperBound<T>(this IReadOnlyList<T> list, T value, int lb, int ub, IComparer<T> comparer)
    {
        if (lb > ub)
            throw new ArgumentOutOfRangeException(nameof(ub));

        while (lb != ub)
        {
            var m = lb + (ub - lb) / 2;
            if (comparer.Compare(list[m], value) <= 0)
            {
                lb = m + 1;
            }
            else
            {
                ub = m;
            }
        }
        return lb;
    }

    public static int UpperBound<T>(this IReadOnlyList<T> list, T value, IComparer<T> comparer) =>
        UpperBound(list, value, 0, list.Count, comparer);

    public static int UpperBound<T>(this IReadOnlyList<T> list, T value, int lb, int ub) =>
        UpperBound(list, value, lb, ub, Comparer<T>.Default);

    public static int UpperBound<T>(this IReadOnlyList<T> list, T value) =>
        UpperBound(list, value, 0, list.Count, Comparer<T>.Default);
}
