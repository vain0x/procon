using System;
using System.Collections.Generic;
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

    private int N { get; }
    private List<Edge>[] G { get; }

    public MaxFlowByFordFulkerson(int n)
    {
        N = n;
        G = new List<Edge>[n];

        for (var u = 0; u < n; u++)
        {
            G[u] = new List<Edge>();
        }
    }

    public void Insert(int source, int dest, int capacity)
    {
        var e1 = new Edge() { Source = source, Dest = dest, Capacity = capacity };
        var e2 = new Edge() { Source = dest, Dest = source, Capacity = 0 };
        e1.Dual = e2;
        e2.Dual = e1;
        G[source].Add(e1);
        G[dest].Add(e2);
    }

    private bool Dfs(int v, int t, bool[] done, ref Flow flow)
    {
        if (v == t)
            return true;

        done[v] = true;
        foreach (var e in G[v])
        {
            if (done[e.Dest] || e.Capacity <= 0)
                continue;

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

    public Flow MaxFlow(int source, int sink)
    {
        const Flow INF = 1 << 28;
        var totalFlow = (Flow)0;
        while (true)
        {
            var done = new bool[N];
            var flow = INF;

            if (!Dfs(source, sink, done, ref flow))
                return totalFlow;

            totalFlow += flow;
        }
    }
}
