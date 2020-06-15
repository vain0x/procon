using System;

namespace Procon
{
    public sealed class Point3D<T>
        : Tuple<T, T, T>
    {
        public T X => Item1;
        public T Y => Item2;
        public T Z => Item3;

        public Point3D(T x, T y, T z)
            : base(x, y, z)
        {
        }
    }
}
