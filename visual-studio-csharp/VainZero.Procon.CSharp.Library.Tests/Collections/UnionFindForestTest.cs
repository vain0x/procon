using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xunit;

namespace VainZero.Collections
{
    public sealed class UnionFindForestTest
    {
        [Fact]
        public void Test_initial_state()
        {
            var n = 10;
            var uff = new UnionFindForest(n);
            for (var i = 0; i < n; i++)
            {
                Assert.Equal(i, uff.Root(i));
            }
        }

        [Fact]
        public void Test_single_group_case()
        {
            var n = 10;

            var uff = new UnionFindForest(n);
            for (var i = 0; i + 1 < n; i++)
            {
                uff.Merge(i, i + 1);
            }

            var r = uff.Root(0);

            for (var i = 1; i < n; i++)
            {
                Assert.Equal(r, uff.Root(i));
                Assert.True(uff.Connects(r, uff.Root(i)));
            }
        }

        [Fact]
        public void Test_two_group_case()
        {
            var n = 10;

            var uff = new UnionFindForest(n);
            for (var i = 0; i < n; i++)
            {
                uff.Merge(i, i % 2);
            }

            Assert.Equal(0, uff.Root(0) % 2);
            Assert.Equal(1, uff.Root(1) % 2);

            for (var i = 0; i < n; i++)
            {
                Assert.Equal(uff.Root(i % 2), uff.Root(i));
                Assert.True(uff.Connects(i % 2, i));
            }
        }
    }
}
