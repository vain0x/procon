using System;
using System.Collections.Generic;
using System.Linq;
using Xunit;

public sealed class DequeTest
{
    private Deque<int> Create(params int[] values) =>
        Deque.FromEnumerable(values);

    [Fact]
    public void Test_PushFront_PopFront()
    {
        var q = Create();

        q.Count.Is(0);

        q.PushFront(1);
        q.Count.Is(1);

        q.PushFront(0);
        q.Count.Is(2);

        q.IsSeq(new[] { 0, 1 });

        q.PopFront().Is(0);
        q.PopFront().Is(1);
    }

    [Fact]
    public void Test_PushBack_PopBack()
    {
        var q = Create();

        q.Count.Is(0);

        q.PushBack(0);
        q.Count.Is(1);

        q.PushBack(1);
        q.Count.Is(2);

        q.IsSeq(new[] { 0, 1 });

        q.PopBack().Is(1);
        q.Count.Is(1);

        q.PopBack().Is(0);
        q.Count.Is(0);
    }

    [Fact]
    public void Test_PushFront_PushBack()
    {
        var q = Create();

        q.PushFront(1);
        q.PushBack(2);
        q.PushFront(0);
        q.PushBack(3);

        q.IsSeq(new[] { 0, 1, 2, 3 });
    }

    [Fact]
    public void Test_Item()
    {
        var q = Create(0, 1, 2, 3);
        q.PopFront();

        q[0] += 10;
        q[1] += 20;
        q[2] += 30;

        q.IsSeq(new[] { 11, 22, 33 });
    }

    [Fact]
    public void Test_Large_Random()
    {
        var random = new Random();
        var q = Create();

        var n = random.Next(10000, 50000);
        var set = new HashSet<int>();

        for (var i = 0; i < n; i++)
        {
            if (random.Next(0, 2) == 0)
            {
                q.PushFront(i);
            }
            else
            {
                q.PushBack(i);
            }
            set.Add(i);

            var d = random.Next(0, 100);
            if (d == 0)
            {
                set.Remove(q.PopFront()).Is(true);
            }
            else if (d == 1)
            {
                set.Remove(q.PopBack()).Is(true);
            }
        }

        q.OrderBy(x => x).IsSeq(set.OrderBy(x => x));
    }
}
