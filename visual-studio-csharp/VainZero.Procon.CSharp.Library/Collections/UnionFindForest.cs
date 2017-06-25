using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace VainZero.Collections
{
    public sealed class UnionFindForest
    {
        readonly int[] parents;
        readonly int[] ranks;

        /// <summary>
        /// Constructs n-vertex UFF.
        /// </summary>
        public UnionFindForest(int n)
        {
            parents = new int[n];
            ranks = new int[n];

            for (var v = 0; v < n; v++)
            {
                parents[v] = v;
                ranks[v] = 1;
            }
        }

        /// <summary>
        /// Gets the representative of the specified vertex's group.
        /// </summary>
        public int Root(int v)
        {
            if (parents[v] == v)
            {
                return v;
            }

            var r = Root(parents[v]);
            parents[v] = r;
            return r;
        }

        /// <summary>
        /// Gets a value indicating whether two vertices belong to the same group.
        /// </summary>
        public bool Connects(int u, int v)
        {
            return Root(u) == Root(v);
        }

        static void Swap<X>(ref X l, ref X r)
        {
            var t = l;
            l = r;
            r = t;
        }

        /// <summary>
        /// Merges the specified vertices' group.
        /// </summary>
        public void Merge(int u, int v)
        {
            u = Root(u);
            v = Root(v);
            if (u == v) return;

            if (ranks[u] > ranks[v])
            {
                Swap(ref u, ref v);
            }

            parents[u] = v;
            ranks[v] += ranks[u];
        }
    }
}
