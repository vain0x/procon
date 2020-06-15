using Xunit;

namespace Procon
{
    public sealed class GcdExtensionTest
    {
        [Fact]
        public void Test_Gcd()
        {
            var rows = new[]
            {
                (x: 1L, y: 1L, g: 1L),
                (x: 3L, y: 5L, g: 1L),
                (x: 2L, y: 2L, g: 2L),
                (x: 4L, y: 6L, g: 2L),
                (x: 18L, y: 24L, g: 6L),
            };

            foreach (var (x, y, g) in rows)
            {
                x.Gcd(y).Is(g);
            }
        }

        // x: expected, a: actual
        private static void TestGcdExtended(long x, long y, long xG, long xS, long xT)
        {
            var (aG, aS, aT) = x.GcdExtended(y);
            (aG, aS, aT).Is((xG, xS, xT));
            (x * aS + y * aT).Is(aG);
        }

        [Fact]
        public void Test_GcdExtended()
        {
            TestGcdExtended(3, 7, xG: 1, xS: -2, xT: 1);
            TestGcdExtended(4, 6, xG: 2, xS: -1, xT: 1);
            TestGcdExtended(360 * 5 * 5 * 11, 360 * 7 * 7 * 13, xG: 360, xS: -227, xT: 98);
        }
    }
}
