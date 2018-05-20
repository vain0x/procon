namespace Procon
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Text;
    using System.Threading.Tasks;

    /// <summary>
    /// Provides binary search implementations.
    /// </summary>
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
        public static int LowerBound<X>(this IReadOnlyList<X> @this, X value, int lb, int ub, IComparer<X> comparer)
        {
            if (lb > ub) throw new ArgumentOutOfRangeException(nameof(ub));

            while (lb != ub)
            {
                var m = lb + (ub - lb) / 2;
                if (comparer.Compare(@this[m], value) < 0)
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
        public static int LowerBound<X>(this IReadOnlyList<X> @this, X value, IComparer<X> comparer)
        {
            return LowerBound(@this, value, 0, @this.Count, comparer);
        }

        /// <summary>
        /// Performs binary search
        /// to find the lower bound index of the specified value in the range.
        /// </summary>
        public static int LowerBound<X>(this IReadOnlyList<X> @this, X value, int lb, int ub)
        {
            return LowerBound(@this, value, lb, ub, Comparer<X>.Default);
        }

        /// <summary>
        /// Performs binary search
        /// to find the lower bound index of the specified value.
        /// </summary>
        public static int LowerBound<X>(this IReadOnlyList<X> @this, X value)
        {
            return LowerBound(@this, value, 0, @this.Count, Comparer<X>.Default);
        }

        /// <summary>
        /// Performs binary search
        /// to find the upper bound index of the specified value in the range.
        /// </summary>
        public static int UpperBound<X>(this IReadOnlyList<X> @this, X value, int lb, int ub, IComparer<X> comparer)
        {
            if (lb > ub) throw new ArgumentOutOfRangeException(nameof(ub));

            while (lb != ub)
            {
                var m = lb + (ub - lb) / 2;
                if (comparer.Compare(@this[m], value) <= 0)
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
        public static int UpperBound<X>(this IReadOnlyList<X> @this, X value, IComparer<X> comparer)
        {
            return UpperBound(@this, value, 0, @this.Count, comparer);
        }

        /// <summary>
        /// Performs binary search
        /// to find the upper bound index of the specified value in the range.
        /// </summary>
        public static int UpperBound<X>(this IReadOnlyList<X> @this, X value, int lb, int ub)
        {
            return UpperBound(@this, value, lb, ub, Comparer<X>.Default);
        }

        /// <summary>
        /// Performs binary search
        /// to find the upper bound index of the specified value.
        /// </summary>
        public static int UpperBound<X>(this IReadOnlyList<X> @this, X value)
        {
            return UpperBound(@this, value, 0, @this.Count, Comparer<X>.Default);
        }
    }
}
