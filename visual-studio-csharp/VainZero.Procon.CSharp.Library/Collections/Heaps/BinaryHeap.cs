using System;
using System.Collections.Generic;
using System.Linq;

namespace VainZero.Collections.Heaps
{
    public sealed class BinaryHeap<TValue>
        : IReadOnlyCollection<TValue>
    {
        readonly List<TValue> list;
        readonly Func<TValue, TValue, int> compare;

        public int Count
        {
            get { return list.Count; }
        }

        public IEnumerator<TValue> GetEnumerator()
        {
            return list.GetEnumerator();
        }

        System.Collections.IEnumerator System.Collections.IEnumerable.GetEnumerator()
        {
            return GetEnumerator();
        }

        public TValue Peek()
        {
            return list[0];
        }

        public void Enqueue(TValue value)
        {
            list.Add(value);
            var i = list.Count - 1;
            while (i > 0)
            {
                // Index of the parent.
                var p = (i - 1) >> 1;

                if (compare(list[p], value) <= 0) break;
                list[i] = list[p];
                i = p;
            }
            list[i] = value;
        }

        public TValue Dequeue()
        {
            var min = list[0];
            var x = list[list.Count - 1];
            var i = 0;
            while (true)
            {
                // Index of children.
                var l = (i << 1) + 1;
                var r = (i << 1) + 2;
                if (l >= list.Count) break;

                // Index of the smaller child.
                var c = r < list.Count && compare(list[r], list[l]) < 0 ? r : l;

                if (compare(list[c], x) >= 0) break;
                list[i] = list[c];
                i = c;
            }
            list[i] = x;
            list.RemoveAt(list.Count - 1);
            return min;
        }

        public BinaryHeap(List<TValue> list, Func<TValue, TValue, int> compare)
        {
            this.list = list;
            this.compare = compare;
        }
    }

    public static class BinaryHeap
    {
        public static BinaryHeap<X> Create<X>(Func<X, X, int> compare)
        {
            return new BinaryHeap<X>(new List<X>(), compare);
        }

        public static BinaryHeap<X> Create<X>()
        {
            return new BinaryHeap<X>(new List<X>(), Comparer<X>.Default.Compare);
        }

        public static BinaryHeap<X> FromEnumerable<X>(IEnumerable<X> xs, Func<X, X, int> compare)
        {
            var list = new List<X>(xs);
            list.Sort(new Comparison<X>(compare));
            return new BinaryHeap<X>(list, compare);
        }

        public static BinaryHeap<X> FromEnumerable<X>(IEnumerable<X> xs)
        {
            return FromEnumerable(xs, Comparer<X>.Default.Compare);
        }
    }
}
