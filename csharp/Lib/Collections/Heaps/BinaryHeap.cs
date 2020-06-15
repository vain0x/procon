using System;
using System.Collections.Generic;

namespace Procon
{
    public sealed class BinaryHeap<T>
        : IReadOnlyCollection<T>
    {
        private readonly List<T> _list;
        private readonly Func<T, T, int> _compare;

        public BinaryHeap(List<T> list, Func<T, T, int> compare)
        {
            _list = list;
            _compare = compare;
        }

        public int Count
        {
            get { return _list.Count; }
        }

        public T Peek()
        {
            return _list[0];
        }

        public void Enqueue(T value)
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

        public T Dequeue()
        {
            var min = _list[0];
            var item = _list[_list.Count - 1];
            var i = 0;
            while (true)
            {
                // Index of children.
                var l = (i << 1) + 1;
                var r = (i << 1) + 2;
                if (l >= _list.Count) break;

                // Index of the smaller child.
                var c = r < _list.Count && _compare(_list[r], _list[l]) < 0 ? r : l;

                if (_compare(_list[c], item) >= 0) break;
                _list[i] = _list[c];
                i = c;
            }
            _list[i] = item;
            _list.RemoveAt(_list.Count - 1);
            return min;
        }

        public IEnumerator<T> GetEnumerator()
        {
            return _list.GetEnumerator();
        }

        System.Collections.IEnumerator System.Collections.IEnumerable.GetEnumerator()
        {
            return GetEnumerator();
        }
    }

    public static class BinaryHeap
    {
        public static BinaryHeap<T> Create<T>(Func<T, T, int> compare)
        {
            return new BinaryHeap<T>(new List<T>(), compare);
        }

        public static BinaryHeap<T> Create<T>()
        {
            return new BinaryHeap<T>(new List<T>(), Comparer<T>.Default.Compare);
        }

        public static BinaryHeap<T> FromEnumerable<T>(IEnumerable<T> source, Func<T, T, int> compare)
        {
            var list = new List<T>(source);
            list.Sort(new Comparison<T>(compare));
            return new BinaryHeap<T>(list, compare);
        }

        public static BinaryHeap<T> FromEnumerable<T>(IEnumerable<T> source)
        {
            return FromEnumerable(source, Comparer<T>.Default.Compare);
        }
    }
}
