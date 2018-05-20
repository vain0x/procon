using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xunit;

namespace VainZero.Collections.Heaps
{
    public sealed class BinaryHeapTest
    {
        private static IEnumerable<int> UnorderedSeq(int n)
        {
            return Enumerable.Range(0, n).Select(i => (i * i * i) % 17);
        }

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

                var x = heap.Dequeue();
                heap.Count.Is(1);
                x.Is(1);
            }

            // Test to add a value less than min.
            {
                heap.Enqueue(0);
                heap.Count.Is(2);
                heap.Peek().Is(0);

                var x = heap.Dequeue();
                heap.Count.Is(1);
                x.Is(0);
            }
        }

        [Fact]
        public void Test_GetEnumerator()
        {
            var n = 100;

            var heap = BinaryHeap.Create<int>();
            var xs = UnorderedSeq(n).ToArray();

            foreach (var x in xs)
            {
                heap.Enqueue(x);
            }

            var actual = heap.OrderBy(x => x);

            Array.Sort(xs);
            actual.IsSeq(xs);
        }

        [Fact]
        public void Test_FromEnumerable()
        {
            var n = 10;

            var xs = UnorderedSeq(n).ToArray();
            var heap = BinaryHeap.FromEnumerable(xs);

            var ys = new List<int>();
            while (heap.Count > 0)
            {
                ys.Add(heap.Dequeue());
            }

            Array.Sort(xs);
            ys.IsSeq(xs);
        }
    }
}
