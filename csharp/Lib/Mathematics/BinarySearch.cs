using System;
using System.Collections.Generic;

namespace Procon
{
    public static class BinarySearch
    {
        /// <summary>
        /// Performs Meguru-style binary search.
        /// <seealso cref="https://twitter.com/meguru_comp/status/697008509376835584"/>
        /// </summary>
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

        /// <summary>
        /// Performs Meguru-style binary search.
        /// </summary>
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

        /// <summary>
        /// Performs binary search
        /// to find the lower bound index of the specified value in the range.
        /// </summary>
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

        /// <summary>
        /// Performs binary search
        /// to find the lower bound index of the specified value.
        /// </summary>
        public static int LowerBound<T>(this IReadOnlyList<T> list, T value, IComparer<T> comparer) =>
            LowerBound(list, value, 0, list.Count, comparer);

        /// <summary>
        /// Performs binary search
        /// to find the lower bound index of the specified value in the range.
        /// </summary>
        public static int LowerBound<T>(this IReadOnlyList<T> list, T value, int lb, int ub) =>
            LowerBound(list, value, lb, ub, Comparer<T>.Default);

        /// <summary>
        /// Performs binary search
        /// to find the lower bound index of the specified value.
        /// </summary>
        public static int LowerBound<T>(this IReadOnlyList<T> list, T value) =>
            LowerBound(list, value, 0, list.Count, Comparer<T>.Default);

        /// <summary>
        /// Performs binary search
        /// to find the upper bound index of the specified value in the range.
        /// </summary>
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

        /// <summary>
        /// Performs binary search
        /// to find the upper bound index of the specified value.
        /// </summary>
        public static int UpperBound<T>(this IReadOnlyList<T> list, T value, IComparer<T> comparer) =>
            UpperBound(list, value, 0, list.Count, comparer);

        /// <summary>
        /// Performs binary search
        /// to find the upper bound index of the specified value in the range.
        /// </summary>
        public static int UpperBound<T>(this IReadOnlyList<T> list, T value, int lb, int ub) =>
            UpperBound(list, value, lb, ub, Comparer<T>.Default);

        /// <summary>
        /// Performs binary search
        /// to find the upper bound index of the specified value.
        /// </summary>
        public static int UpperBound<T>(this IReadOnlyList<T> list, T value) =>
            UpperBound(list, value, 0, list.Count, Comparer<T>.Default);
    }
}
