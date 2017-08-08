using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xunit;

namespace VainZero.Collections
{
    public sealed class SegmentTreeTest
    {
        X Aggregate<X>(X[] array, int index, int count, X empty, Func<X, X, X> append)
        {
            var j = index + count;
            var s = empty;
            for (var i = index; i < j; i++)
            {
                s = append(s, array[i]);
            }
            return s;
        }

        void Verify<X>(X[] array, SegmentTree<X> tree)
        {
            for (var i = 0; i < array.Length; i++)
            {
                for (var k = 0; i + k <= array.Length; k++)
                {
                    var expected = Aggregate(array, i, k, tree.Empty, tree.Append);
                    Assert.Equal(expected, tree.Query(i, k));

                    if (i == 0 && k == array.Length)
                    {
                        Assert.Equal(expected, tree.Query());
                    }
                }
            }
        }

        void VerifySmall(int empty, Func<int, int, int> append)
        {
            const int Min = -100;
            const int Max = 100;

            var random = new Random();

            for (var count = 1; count < 15; count++)
            {
                var array = Enumerable.Repeat(0, count).Select(_ => random.Next(Min, Max)).ToArray();
                var tree = SegmentTree.Create(array, empty, append);

                Assert.Equal(count, tree.Count);
                Assert.Equal(array, tree.ToArray());

                Verify(array, tree);

                for (var s = 0; s < 20; s++)
                {
                    var i = random.Next(0, count);
                    var x = random.Next(Min, Max);
                    tree[i] = x;
                    array[i] = x;

                    Verify(array, tree);
                }
            }
        }

        [Fact]
        public void Test_minimum_random()
        {
            VerifySmall(int.MaxValue, Math.Min);
        }

        [Fact]
        public void Test_mul_random()
        {
            VerifySmall(1, (x, y) => x * y);
        }

        [Fact]
        public void Test_large()
        {
            var array = new long[10000];
            for (var i = 0; i < array.Length; i++)
            {
                array[i] = i * i;
            }

            var tree = SegmentTree.Create(array, 0L, (x, y) => x + y);

            var l = 2345;
            var r = 8765;
            var expected = 220123632470L;
            Assert.Equal(expected, tree.Query(l, r - l));

            tree[5555] = 1;
            expected += 1 - 5555 * 5555;

            tree[r] = 0;

            Assert.Equal(expected, tree.Query(l, r - l));
        }

        [Fact]
        public void Test_FromSemigroup()
        {
            var tree = SegmentTree.FromSemigroup(new[] { 3, 1, 4, 2, 5 }, (x, y) => x + y);

            Assert.Equal(7, tree.Query(1, 3).Value);
            Assert.Equal(false, tree.Query(5, 0).HasValue);
        }
    }
}
