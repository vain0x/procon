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
                new { x = 1L, y = 1L, g = 1L },
                new { x = 3L, y = 5L, g = 1L },
                new { x = 2L, y = 2L, g = 2L },
                new { x = 4L, y = 6L, g = 2L },
                new { x = 18L, y = 24L, g = 6L },
            };

            foreach (var a in rows)
            {
                var g = a.x.Gcd(a.y);
                a.g.Is(g);
            }
        }

        private static void TestGcdExtended(long x, long y, long g, long s, long t)
        {
            var actualG = x.GcdExtended(y, out long actualS, out long actualT);
            new { g = actualG, s = actualS, t = actualT }.Is(
                new { g = g, s = s, t = t }
            );
            (x * actualS + y * actualT).Is(actualG);
        }

        [Fact]
        public void Test_GcdExtended()
        {
            TestGcdExtended(3, 7, g: 1, s: -2, t: 1);
            TestGcdExtended(4, 6, g: 2, s: -1, t: 1);
            TestGcdExtended(360 * 5 * 5 * 11, 360 * 7 * 7 * 13, g: 360, s: -227, t: 98);
        }
    }
}
