namespace Procon
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Text;
    using System.Threading.Tasks;

    [System.Diagnostics.DebuggerDisplay("Count = {Count}")]
    public sealed class SegmentTree<T>
        : IReadOnlyList<T>
        , IList<T>
    {
        public readonly T Empty;
        public readonly Func<T, T, T> Append;

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

        public int Count
        {
            get { return _itemCount; }
        }

        private int LeafCount
        {
            get { return _nodes.Length - _cacheCount; }
        }

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
                if (index < 0 || index >= Count) throw new ArgumentOutOfRangeException("index");
                return _nodes[_cacheCount + index];
            }
            set
            {
                if (index < 0 || index >= Count) throw new ArgumentOutOfRangeException("index");
                SetItem(index, value);
            }
        }

        /// <summary>
        /// Updates all items.
        /// </summary>
        public void CopyFrom(IReadOnlyList<T> list)
        {
            if (list.Count != _itemCount) throw new ArgumentException();

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
            {
                return Empty;
            }
            else if (ql <= nl && nr <= qr)
            {
                return _nodes[i];
            }
            else
            {
                var m = nl + (nr - nl) / 2;
                var l = QueryCore(i * 2 + 1, nl, m, ql, qr);
                var r = QueryCore(i * 2 + 2, m, nr, ql, qr);
                return Append(l, r);
            }
        }

        /// <summary>
        /// Calculates the sum of items in the specified range.
        /// </summary>
        public T Query(int index, int count)
        {
            if (index < 0 || index > _itemCount) throw new ArgumentOutOfRangeException("index");
            if (count < 0 || index + count > _itemCount) throw new ArgumentOutOfRangeException("count");
            if (count == 0) return Empty;
            return QueryCore(0, 0, LeafCount, index, index + count);
        }

        /// <summary>
        /// Gets the sum of items.
        /// </summary>
        public T Query()
        {
            return _nodes[0];
        }

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
                {
                    return i;
                }
            }
            return -1;
        }

        private bool Contains(T item)
        {
            return IndexOf(item) >= 0;
        }

        IEnumerator<T> IEnumerable<T>.GetEnumerator()
        {
            return GetEnumerator();
        }

        System.Collections.IEnumerator System.Collections.IEnumerable.GetEnumerator()
        {
            return ((IEnumerable<T>)this).GetEnumerator();
        }

        int IReadOnlyCollection<T>.Count
        {
            get
            {
                return Count;
            }
        }

        T IReadOnlyList<T>.this[int index]
        {
            get
            {
                return this[index];
            }
        }

        bool ICollection<T>.IsReadOnly
        {
            get
            {
                return false;
            }
        }

        int ICollection<T>.Count
        {
            get
            {
                return Count;
            }
        }

        bool ICollection<T>.Contains(T item)
        {
            return Contains(item);
        }

        void ICollection<T>.CopyTo(T[] array, int arrayIndex)
        {
            CopyTo(array, arrayIndex);
        }

        void ICollection<T>.Add(T item)
        {
            throw new NotSupportedException();
        }

        bool ICollection<T>.Remove(T item)
        {
            throw new NotSupportedException();
        }

        void ICollection<T>.Clear()
        {
            throw new NotSupportedException();
        }

        T IList<T>.this[int index]
        {
            get
            {
                return this[index];
            }
            set
            {
                this[index] = value;
            }
        }

        int IList<T>.IndexOf(T item)
        {
            return IndexOf(item);
        }

        void IList<T>.Insert(int index, T item)
        {
            throw new NotSupportedException();
        }

        void IList<T>.RemoveAt(int index)
        {
            throw new NotSupportedException();
        }
        #endregion

        public SegmentTree(T[] nodes, int cacheCount, int itemCount, T empty, Func<T, T, T> append)
        {
            _nodes = nodes;
            _cacheCount = cacheCount;
            _itemCount = itemCount;
            Empty = empty;
            Append = append;
        }
    }

    public static class SegmentTree
    {
        private static int CalculateHeight(int count)
        {
            var h = 0;
            while ((1 << h) < count) h++;
            return h;
        }

        public static SegmentTree<X> Create<X>(IEnumerable<X> items, X empty, Func<X, X, X> append)
        {
            var buffer = items as IReadOnlyList<X> ?? items.ToList();
            var count = buffer.Count;

            if (count == 0)
            {
                throw new NotSupportedException("Empty segment tree is not supported.");
            }

            var height = CalculateHeight(count);
            var nodes = new X[(1 << height) * 2 - 1];
            var innerNodeCount = (1 << height) - 1;

            var tree = new SegmentTree<X>(nodes, innerNodeCount, count, empty, append);
            tree.CopyFrom(buffer);

            return tree;
        }

        public struct Option<T>
        {
            public readonly T Value;
            public readonly bool HasValue;

            public override string ToString()
            {
                return HasValue ? string.Concat(Value) : "None";
            }

            public static Option<T> None
            {
                get { return new Option<T>(); }
            }

            public Option(T value)
            {
                Value = value;
                HasValue = true;
            }
        }

        public static SegmentTree<Option<X>> FromSemigroup<X>(IEnumerable<X> items, Func<X, X, X> append)
        {
            return
                Create(
                    items.Select(x => new Option<X>(x)),
                    Option<X>.None,
                    (xo, yo) =>
                        xo.HasValue
                            ? (yo.HasValue ? new Option<X>(append(xo.Value, yo.Value)) : xo)
                            : yo
                );
        }
    }
}
