using System;

namespace Procon
{
    public sealed class Point2D<T>
        : Tuple<T, T>
    {
        public T X { get { return Item1; } }
        public T Y { get { return Item2; } }

        public Point2D(T x, T y)
            : base(x, y)
        {
            var c = new System.Numerics.Complex(1, 2);
        }
    }
}
