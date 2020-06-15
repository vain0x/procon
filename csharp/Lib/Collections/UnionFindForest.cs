namespace Procon
{
    public sealed class UnionFindForest
    {
        private readonly int[] _parents;
        private readonly int[] _ranks;

        /// <summary>
        /// Constructs n-vertex UFF.
        /// </summary>
        public UnionFindForest(int n)
        {
            _parents = new int[n];
            _ranks = new int[n];

            for (var v = 0; v < n; v++)
            {
                _parents[v] = v;
                _ranks[v] = 1;
            }
        }

        /// <summary>
        /// Gets the representative of the specified vertex's group.
        /// </summary>
        public int Root(int v)
        {
            if (_parents[v] == v)
                return v;

            var r = Root(_parents[v]);
            _parents[v] = r;
            return r;
        }

        /// <summary>
        /// Gets a value indicating whether two vertices belong to the same group.
        /// </summary>
        public bool Connects(int u, int v) =>
            Root(u) == Root(v);

        /// <summary>
        /// Merges the specified vertices' group.
        /// </summary>
        public void Merge(int u, int v)
        {
            u = Root(u);
            v = Root(v);
            if (u == v)
                return;

            if (_ranks[u] > _ranks[v])
            {
                (u, v) = (v, u);
            }

            _parents[u] = v;
            _ranks[v] += _ranks[u];
        }
    }
}
