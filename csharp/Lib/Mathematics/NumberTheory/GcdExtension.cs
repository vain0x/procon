public static class GcdExtension
{
    public static long Gcd(this long x, long y) =>
        y == 0 ? x : y.Gcd(x % y);

    public static (long g, long s, long t) GcdExtended(this long x, long y)
    {
        if (y == 0)
            return (g: x, s: 1, t: 0);

        var (g, t, s) = y.GcdExtended(x % y);
        t -= (x / y) * s;
        return (g, s, t);
    }
}
