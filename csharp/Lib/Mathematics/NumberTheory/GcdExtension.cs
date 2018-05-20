namespace Procon
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Text;
    using System.Threading.Tasks;

    public static class GcdExtension
    {
        public static long Gcd(this long x, long y)
        {
            return y == 0 ? x : y.Gcd(x % y);
        }

        public static long GcdExtended(this long x, long y, out long s, out long t)
        {
            if (y == 0)
            {
                s = 1;
                t = 0;
                return x;
            }
            else
            {
                var g = y.GcdExtended(x % y, out t, out s);
                t -= (x / y) * s;
                return g;
            }
        }

        public struct GcdExtendedResult
        {
            public readonly long G, X, Y;

            public GcdExtendedResult(long g, long x, long y)
            {
                G = g;
                X = x;
                Y = y;
            }
        }

        public static GcdExtendedResult GcdExtended(this long x, long y)
        {
            long s, t;
            var g = x.GcdExtended(y, out s, out t);
            return new GcdExtendedResult(g, s, t);
        }
    }
}
