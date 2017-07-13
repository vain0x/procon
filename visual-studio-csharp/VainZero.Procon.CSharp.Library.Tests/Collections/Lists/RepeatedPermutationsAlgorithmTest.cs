using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xunit;

namespace VainZero.Collections.Lists
{
    public sealed class RepeatedPermutationsAlgorithmTest
    {
        public sealed class Test_RepeatedPermutations
        {
            [Fact]
            public void Test_edge_cases()
            {
                Assert.Equal(new[] { new int[0] }, new[] { 0, 1 }.RepeatedPermutations(0));
                Assert.Equal(new int[0][], new int[0].RepeatedPermutations(1));
            }

            [Fact]
            public void Test_source_3_count_2()
            {
                var expected =
                    new[]
                    {
                        new[] { 0, 0 },
                        new[] { 0, 1 },
                        new[] { 0, 2 },
                        new[] { 1, 0 },
                        new[] { 1, 1 },
                        new[] { 1, 2 },
                        new[] { 2, 0 },
                        new[] { 2, 1 },
                        new[] { 2, 2 },
                    };

                var actual = new[] { 0, 1, 2 }.RepeatedPermutations(2);
                Assert.Equal(expected, actual);
            }

            [Fact]
            public void Test_source_2_count_3()
            {
                var expected =
                    new[]
                    {
                        new[] { 0, 0, 0 },
                        new[] { 0, 0, 1 },
                        new[] { 0, 1, 0 },
                        new[] { 0, 1, 1 },
                        new[] { 1, 0, 0 },
                        new[] { 1, 0, 1 },
                        new[] { 1, 1, 0 },
                        new[] { 1, 1, 1 },
                    };

                var actual = new[] { 0, 1 }.RepeatedPermutations(3);
                Assert.Equal(expected, actual);
            }
        }
    }
}
