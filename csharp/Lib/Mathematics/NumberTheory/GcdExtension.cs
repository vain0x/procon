namespace Procon
{
    public static class GcdExtension
    {
        public static long Gcd(this long x, long y) =>
            y == 0 ? x : y.Gcd(x % y);

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

        public static GcdExtendedResult GcdExtended(this long x, long y) =>
            new GcdExtendedResult(
                x.GcdExtended(y, out var s, out var t),
                s,
                t
            );
    }
}
