using System;

namespace VainZero
{
    public sealed class Edge<V, W>
        : Tuple<W, V, V>
    {
        public W Weight { get { return Item1; } }
        public V Source { get { return Item2; } }
        public V Dest { get { return Item3; } }

        public Edge(V source, V dest, W weight)
            : base(weight, source, dest)
        {
        }
    }
}
