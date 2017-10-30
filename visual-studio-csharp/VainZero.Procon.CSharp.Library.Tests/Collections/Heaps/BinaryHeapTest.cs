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

            Assert.Equal(0, heap.Count);
            Assert.ThrowsAny<Exception>(() => heap.Dequeue());
            Assert.ThrowsAny<Exception>(() => heap.Peek());

            heap.Enqueue(1);
            Assert.Equal(1, heap.Count);
            Assert.Equal(1, heap.Peek());

            // Test to add a value larger than min.
            {
                heap.Enqueue(2);
                Assert.Equal(2, heap.Count);
                Assert.Equal(1, heap.Peek());

                var x = heap.Dequeue();
                Assert.Equal(1, heap.Count);
                Assert.Equal(1, x);
            }

            // Test to add a value less than min.
            {
                heap.Enqueue(0);
                Assert.Equal(2, heap.Count);
                Assert.Equal(0, heap.Peek());

                var x = heap.Dequeue();
                Assert.Equal(1, heap.Count);
                Assert.Equal(0, x);
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
            Assert.Equal(xs, actual);
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
            Assert.Equal(xs, ys);
        }
    }
}
