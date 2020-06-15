using System;
using System.Collections.Generic;
using System.Linq;
using Xunit;

namespace Procon
{
    public sealed class BinaryHeapTest
    {
        private static IEnumerable<int> UnorderedSeq(int n) =>
            Enumerable.Range(0, n).Select(i => (i * i * i) % 17);

        [Fact]
        public void Test_Enqueue_Dequeue()
        {
            var heap = BinaryHeap.Create<int>();

            heap.Count.Is(0);
            Assert.ThrowsAny<Exception>(() => heap.Dequeue());
            Assert.ThrowsAny<Exception>(() => heap.Peek());

            heap.Enqueue(1);
            heap.Count.Is(1);
            heap.Peek().Is(1);

            // Test to add a value larger than min.
            {
                heap.Enqueue(2);
                heap.Count.Is(2);
                heap.Peek().Is(1);

                var item = heap.Dequeue();
                heap.Count.Is(1);
                item.Is(1);
            }

            // Test to add a value less than min.
            {
                heap.Enqueue(0);
                heap.Count.Is(2);
                heap.Peek().Is(0);

                var item = heap.Dequeue();
                heap.Count.Is(1);
                item.Is(0);
            }
        }

        [Fact]
        public void Test_GetEnumerator()
        {
            var n = 100;

            var heap = BinaryHeap.Create<int>();
            var array = UnorderedSeq(n).ToArray();

            foreach (var item in array)
            {
                heap.Enqueue(item);
            }

            var actual = heap.OrderBy(x => x);

            Array.Sort(array);
            actual.IsSeq(array);
        }

        [Fact]
        public void Test_FromEnumerable()
        {
            var n = 10;

            var array = UnorderedSeq(n).ToArray();
            var heap = BinaryHeap.FromEnumerable(array);

            var list = new List<int>();
            while (heap.Count > 0)
            {
                list.Add(heap.Dequeue());
            }

            Array.Sort(array);
            list.IsSeq(array);
        }
    }
}
