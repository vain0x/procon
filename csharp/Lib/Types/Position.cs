using System;

namespace Procon
{
    public sealed class Position<T>
        : Tuple<T, T>
    {
        public T Y => Item1;
        public T X => Item2;

        public Position(T y, T x)
            : base(y, x)
        {
        }
    }
}
