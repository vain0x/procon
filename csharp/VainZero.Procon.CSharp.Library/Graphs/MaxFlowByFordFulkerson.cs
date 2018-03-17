using System;
using System.Collections.Generic;

namespace VainZero.Graphs
{
    using Flow = System.Int64; // Or System.Double

    public sealed class MaxFlowByFordFulkerson
    {
        public sealed class Edge
        {
            public int Source;
            public int Dest;
            public Flow Capacity;
            public Edge Dual;
        }

        private readonly int N;
        private readonly List<Edge>[] G;

        public MaxFlowByFordFulkerson(int n)
        {
            N = n;
            G = new List<Edge>[n];

            for (var u = 0; u < n; u++)
            {
                G[u] = new List<Edge>();
            }
        }

        /// <summary>
        /// Inserts an edge from u to v with the specified capacity.
        /// </summary>
        public void Insert(int u, int v, int capacity)
        {
            var e1 = new Edge() { Source = u, Dest = v, Capacity = capacity };
            var e2 = new Edge() { Source = v, Dest = u, Capacity = 0 };
            e1.Dual = e2;
            e2.Dual = e1;
            G[u].Add(e1);
            G[v].Add(e2);
        }

        private bool Dfs(int v, int t, bool[] done, ref Flow flow)
        {
            if (v == t)
            {
                return true;
            }

            done[v] = true;
            foreach (var e in G[v])
            {
                if (done[e.Dest] || e.Capacity <= 0)
                {
                    continue;
                }

                var d = Math.Min(flow, e.Capacity);

                if (Dfs(e.Dest, t, done, ref d))
                {
                    e.Capacity -= d;
                    e.Dual.Capacity += d;
                    flow = d;
                    return true;
                }
            }

            return false;
        }

        /// <summary>
        /// Calculates max flow from vertex s to t.
        /// </summary>
        public Flow MaxFlow(int s, int t)
        {
            const Flow INF = 1 << 28;
            var totalFlow = (Flow)0;
            while (true)
            {
                var done = new bool[N];
                var flow = INF;

                if (!Dfs(s, t, done, ref flow))
                {
                    return totalFlow;
                }

                totalFlow += flow;
            }
        }
    }
}
