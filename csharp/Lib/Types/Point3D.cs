using System;

namespace Procon
{
    public sealed class Point3D<T>
        : Tuple<T, T, T>
    {
        public T X { get { return Item1; } }
        public T Y { get { return Item2; } }
        public T Z { get { return Item3; } }

        public Point3D(T x, T y, T z)
            : base(x, y, z)
        {
        }
    }
}
