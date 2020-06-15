using System;

namespace Procon
{
    public sealed class Edge<V, W>
        : Tuple<W, V, V>
    {
        public W Weight => Item1;
        public V Source => Item2;
        public V Dest => Item3;

        public Edge(V source, V dest, W weight)
            : base(weight, source, dest)
        {
        }
    }
}
