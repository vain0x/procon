using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xunit;

namespace VainZero.Mathematics
{
    public sealed class BinarySearchTest
    {
        [Fact]
        public void Test_Meguru_Int32()
        {
            Assert.Equal(0, BinarySearch.Meguru(0, 5, x => x <= 0));
            Assert.Equal(3, BinarySearch.Meguru(0, 5, x => x <= 3));
            Assert.Equal(4, BinarySearch.Meguru(0, 5, x => x <= 4));
        }

        [Fact]
        public void Test_Meguru_Int64()
        {
            Assert.Equal(0, BinarySearch.Meguru(0L, 5L, x => x <= 0));
            Assert.Equal(3, BinarySearch.Meguru(0L, 5L, x => x <= 3));
            Assert.Equal(4, BinarySearch.Meguru(0L, 5L, x => x <= 4));
        }

        [Fact]
        public void Test_LowerBound_and_UpperBound()
        {
            var array = new[] { 1, 2, 2, 4 };

            var equalRange =
                new Func<int, Tuple<int, int>>(value =>
                    Tuple.Create(array.LowerBound(value), array.UpperBound(value))
                );

            Assert.Equal(Tuple.Create(0, 0), equalRange(0));
            Assert.Equal(Tuple.Create(0, 1), equalRange(1));
            Assert.Equal(Tuple.Create(1, 3), equalRange(2));
            Assert.Equal(Tuple.Create(3, 3), equalRange(3));
            Assert.Equal(Tuple.Create(3, 4), equalRange(4));
            Assert.Equal(Tuple.Create(4, 4), equalRange(5));
        }

        [Fact]
        public void Test_LowerBound_and_UpperBound_specifying_range_and_comparer()
        {
            // 3's are excluded.
            var array = new[] { 3, 3, 5, 4, 2, 2, 3, 3 };

            var lb = 2;
            var ub = 6;
            var comparer = Comparer<int>.Create((l, r) => r - l);

            var equalRange =
                new Func<int, Tuple<int, int>>(value =>
                    Tuple.Create(
                        array.LowerBound(value, lb, ub, comparer),
                        array.UpperBound(value, lb, ub, comparer)
                    ));

            Assert.Equal(Tuple.Create(2, 2), equalRange(6));
            Assert.Equal(Tuple.Create(2, 3), equalRange(5));
            Assert.Equal(Tuple.Create(3, 4), equalRange(4));
            Assert.Equal(Tuple.Create(4, 4), equalRange(3));
            Assert.Equal(Tuple.Create(4, 6), equalRange(2));
            Assert.Equal(Tuple.Create(6, 6), equalRange(1));
        }
    }
}
