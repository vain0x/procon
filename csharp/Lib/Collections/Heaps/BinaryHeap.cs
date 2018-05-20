namespace Procon
{
    using System;
    using System.Collections.Generic;
    using System.Linq;

    public sealed class BinaryHeap<TValue>
        : IReadOnlyCollection<TValue>
    {
        private readonly List<TValue> _list;
        private readonly Func<TValue, TValue, int> _compare;

        public int Count
        {
            get { return _list.Count; }
        }

        public IEnumerator<TValue> GetEnumerator()
        {
            return _list.GetEnumerator();
        }

        System.Collections.IEnumerator System.Collections.IEnumerable.GetEnumerator()
        {
            return GetEnumerator();
        }

        public TValue Peek()
        {
            return _list[0];
        }

        public void Enqueue(TValue value)
        {
            _list.Add(value);
            var i = _list.Count - 1;
            while (i > 0)
            {
                // Index of the parent.
                var p = (i - 1) >> 1;

                if (_compare(_list[p], value) <= 0) break;
                _list[i] = _list[p];
                i = p;
            }
            _list[i] = value;
        }

        public TValue Dequeue()
        {
            var min = _list[0];
            var x = _list[_list.Count - 1];
            var i = 0;
            while (true)
            {
                // Index of children.
                var l = (i << 1) + 1;
                var r = (i << 1) + 2;
                if (l >= _list.Count) break;

                // Index of the smaller child.
                var c = r < _list.Count && _compare(_list[r], _list[l]) < 0 ? r : l;

                if (_compare(_list[c], x) >= 0) break;
                _list[i] = _list[c];
                i = c;
            }
            _list[i] = x;
            _list.RemoveAt(_list.Count - 1);
            return min;
        }

        public BinaryHeap(List<TValue> list, Func<TValue, TValue, int> compare)
        {
            _list = list;
            _compare = compare;
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
