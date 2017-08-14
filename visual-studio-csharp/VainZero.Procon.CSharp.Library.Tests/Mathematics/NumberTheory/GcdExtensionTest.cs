using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xunit;

namespace VainZero.Mathematics
{
    public sealed class GcdExtensionTest
    {
        [Fact]
        public void Test_Gcd()
        {
            var rows =
                new[]
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
                Assert.Equal(g, a.g);
            }
        }

        static void TestGcdExtended(long x, long y, long g, long s, long t)
        {
            long actualS, actualT;
            var actualG = x.GcdExtended(y, out actualS, out actualT);
            Assert.Equal(
                new { g = g, s = s, t = t },
                new { g = actualG, s = actualS, t = actualT }
            );
            Assert.Equal(actualG, x * actualS + y * actualT);
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
