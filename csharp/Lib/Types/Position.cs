namespace Procon
{
    using System;

    public sealed class Position<T>
        : Tuple<T, T>
    {
        public T Y { get { return Item1; } }
        public T X { get { return Item2; } }

        public Position(T y, T x)
            : base(y, x)
        {
        }
    }
}
