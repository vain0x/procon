using System;
using System.Collections.Generic;
using Xunit;

public sealed class BinarySearchTest
{
    [Fact]
    public void Test_Meguru_Int32()
    {
        BinarySearch.Meguru(0, 5, x => x <= 0).Is(0);
        BinarySearch.Meguru(0, 5, x => x <= 3).Is(3);
        BinarySearch.Meguru(0, 5, x => x <= 4).Is(4);
    }

    [Fact]
    public void Test_Meguru_Int64()
    {
        BinarySearch.Meguru(0L, 5L, x => x <= 0).Is(0);
        BinarySearch.Meguru(0L, 5L, x => x <= 3).Is(3);
        BinarySearch.Meguru(0L, 5L, x => x <= 4).Is(4);
    }

    [Fact]
    public void Test_LowerBound_and_UpperBound()
    {
        var array = new[] { 1, 2, 2, 4 };

        var equalRange = new Func<int, (int lb, int ub)>(value =>
            (array.LowerBound(value), array.UpperBound(value))
        );

        equalRange(0).Is((0, 0));
        equalRange(1).Is((0, 1));
        equalRange(2).Is((1, 3));
        equalRange(3).Is((3, 3));
        equalRange(4).Is((3, 4));
        equalRange(5).Is((4, 4));
    }

    [Fact]
    public void Test_LowerBound_and_UpperBound_specifying_range_and_comparer()
    {
        // 3's are excluded.
        var array = new[] { 3, 3, 5, 4, 2, 2, 3, 3 };

        var lb = 2;
        var ub = 6;
        var comparer = Comparer<int>.Create((l, r) => r - l);

        var equalRange = new Func<int, (int lb, int ub)>(value => (
            array.LowerBound(value, lb, ub, comparer),
            array.UpperBound(value, lb, ub, comparer)
        ));

        equalRange(6).Is((2, 2));
        equalRange(5).Is((2, 3));
        equalRange(4).Is((3, 4));
        equalRange(3).Is((4, 4));
        equalRange(2).Is((4, 6));
        equalRange(1).Is((6, 6));
    }
}
