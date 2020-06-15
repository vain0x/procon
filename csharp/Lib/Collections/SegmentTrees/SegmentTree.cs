using System;
using System.Collections.Generic;
using System.Linq;

namespace Procon
{
    [System.Diagnostics.DebuggerDisplay("Count = {Count}")]
    public sealed class SegmentTree<T>
        : IReadOnlyList<T>
        , IList<T>
    {
        public T Empty { get; }
        public Func<T, T, T> Append { get; }

        private readonly int _cacheCount;
        private readonly int _itemCount;

        /// <summary>
        /// A complete binary tree.
        /// The first <see cref="_cacheCount"/> items are inner nodes
        /// whose value is cache of the query result.
        /// The next <see cref="_itemCount"/> items are leaf nodes.
        /// The rest are filled with <see cref="Empty"/>.
        /// </summary>
        private readonly T[] _nodes;

        public SegmentTree(T[] nodes, int cacheCount, int itemCount, T empty, Func<T, T, T> append)
        {
            _nodes = nodes;
            _cacheCount = cacheCount;
            _itemCount = itemCount;
            Empty = empty;
            Append = append;
        }

        public int Count => _itemCount;

        private int LeafCount => _nodes.Length - _cacheCount;

        private void SetItem(int index, T item)
        {
            var i = _cacheCount + index;

            _nodes[i] = item;

            while (i != 0)
            {
                var parentIndex = (i - 1) / 2;
                var childIndex = parentIndex * 2 + 1;
                _nodes[parentIndex] = Append(_nodes[childIndex], _nodes[childIndex + 1]);
                i = parentIndex;
            }
        }

        public T this[int index]
        {
            get
            {
                if (index < 0 || index >= Count)
                    throw new ArgumentOutOfRangeException(nameof(index));

                return _nodes[_cacheCount + index];
            }
            set
            {
                if (index < 0 || index >= Count)
                    throw new ArgumentOutOfRangeException(nameof(index));

                SetItem(index, value);
            }
        }

        /// <summary>
        /// Updates all items.
        /// </summary>
        public void CopyFrom(IReadOnlyList<T> list)
        {
            if (list.Count != _itemCount)
                throw new ArgumentException();

            for (var k = 0; k < _itemCount; k++)
            {
                _nodes[_cacheCount + k] = list[k];
            }

            for (var i = _cacheCount + _itemCount; i < _nodes.Length; i++)
            {
                _nodes[i] = Empty;
            }

            for (var i = _cacheCount - 1; i >= 0; i--)
            {
                var l = i * 2 + 1;
                var r = i * 2 + 2;
                _nodes[i] = Append(_nodes[l], _nodes[r]);
            }
        }

        private T QueryCore(int i, int nl, int nr, int ql, int qr)
        {
            if (qr <= nl || nr <= ql)
                return Empty;

            if (ql <= nl && nr <= qr)
                return _nodes[i];

            var m = nl + (nr - nl) / 2;
            var l = QueryCore(i * 2 + 1, nl, m, ql, qr);
            var r = QueryCore(i * 2 + 2, m, nr, ql, qr);
            return Append(l, r);
        }

        /// <summary>
        /// Calculates the sum of items in the specified range.
        /// </summary>
        public T Query(int index, int count)
        {
            if (index < 0 || index > _itemCount)
                throw new ArgumentOutOfRangeException(nameof(index));

            if (count < 0 || index + count > _itemCount)
                throw new ArgumentOutOfRangeException(nameof(count));

            if (count == 0)
                return Empty;

            return QueryCore(0, 0, LeafCount, index, index + count);
        }

        /// <summary>
        /// Gets the sum of items.
        /// </summary>
        public T Query() => _nodes[0];

        #region IReadOnlyList<_>, IList<_>

        private IEnumerator<T> GetEnumerator()
        {
            for (var i = 0; i < Count; i++)
            {
                yield return this[i];
            }
        }

        private void CopyTo(T[] array, int index)
        {
            for (var i = 0; i < Count; i++)
            {
                array[index + i] = this[i];
            }
        }

        private int IndexOf(T item)
        {
            for (var i = 0; i < Count; i++)
            {
                if (EqualityComparer<T>.Default.Equals(this[i], item))
                    return i;
            }
            return -1;
        }

        private bool Contains(T item) =>
            IndexOf(item) >= 0;

        IEnumerator<T> IEnumerable<T>.GetEnumerator() =>
            GetEnumerator();

        System.Collections.IEnumerator System.Collections.IEnumerable.GetEnumerator() =>
            ((IEnumerable<T>)this).GetEnumerator();

        int IReadOnlyCollection<T>.Count => Count;

        T IReadOnlyList<T>.this[int index] => this[index];

        bool ICollection<T>.IsReadOnly => false;

        int ICollection<T>.Count => Count;

        bool ICollection<T>.Contains(T item) => Contains(item);

        void ICollection<T>.CopyTo(T[] array, int arrayIndex) =>
            CopyTo(array, arrayIndex);

        void ICollection<T>.Add(T item) =>
            throw new NotSupportedException();

        bool ICollection<T>.Remove(T item) =>
            throw new NotSupportedException();

        void ICollection<T>.Clear() =>
            throw new NotSupportedException();

        T IList<T>.this[int index]
        {
            get => this[index];
            set => this[index] = value;
        }

        int IList<T>.IndexOf(T item) =>
            IndexOf(item);

        void IList<T>.Insert(int index, T item) =>
            throw new NotSupportedException();

        void IList<T>.RemoveAt(int index) =>
            throw new NotSupportedException();

        #endregion
    }

    public static class SegmentTree
    {
        private static int CalculateHeight(int count)
        {
            var h = 0;
            while ((1 << h) < count)
            {
                h++;
            }
            return h;
        }

        public static SegmentTree<T> Create<T>(IEnumerable<T> items, T empty, Func<T, T, T> append)
        {
            var buffer = items as IReadOnlyList<T> ?? items.ToList();
            var count = buffer.Count;

            if (count == 0)
                throw new NotSupportedException("Empty segment tree is not supported.");

            var height = CalculateHeight(count);
            var nodes = new T[(1 << height) * 2 - 1];
            var innerNodeCount = (1 << height) - 1;

            var tree = new SegmentTree<T>(nodes, innerNodeCount, count, empty, append);
            tree.CopyFrom(buffer);
            return tree;
        }

        public static SegmentTree<(bool ok, T value)> FromSemigroup<T>(IEnumerable<T> source, Func<T, T, T> append) =>
            Create(
                source.Select(value => (ok: true, value)),
                default,
                (opt1, opt2) => opt1.ok
                    ? (opt2.ok ?
                        (true, append(opt1.value, opt2.value))
                        : opt1)
                    : opt2
            );
    }
}
