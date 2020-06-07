//! 最大流 (Ford-Fulkerson)

use std;

/// `adjacent_fn(u) = k`: 頂点 u から出る辺の個数 k
/// `edge_fn(u, i) = (v, capacity)`: 頂点 u から出る i 番目の辺の先の頂点が v 、許容量が capacity
pub fn ford_fulkerson<
    AdjacentFn: Fn(usize) -> usize,
    EdgeFn: Fn(usize, usize) -> (usize, isize),
>(
    s: usize,
    t: usize,
    n: usize,
    adjacent_fn: AdjacentFn,
    edge_fn: EdgeFn,
) -> isize {
    type VertexId = usize;
    type EdgeId = usize;
    type Flow = isize;

    struct EdgeData {
        v: VertexId,
        capacity: Flow,
        rev: EdgeId,
    }

    struct FordFulkerson {
        t: VertexId,
        g: Vec<Vec<EdgeId>>,
        edges: Vec<EdgeData>,
        done: Vec<bool>,
    }

    const INF: Flow = 1 << 60;

    fn dfs(u: VertexId, flow: &mut Flow, ff: &mut FordFulkerson) -> bool {
        if u == ff.t {
            return true;
        }

        ff.done[u] = true;

        for i in 0..ff.g[u].len() {
            let ei = ff.g[u][i];
            let EdgeData {
                v, capacity, rev, ..
            } = ff.edges[ei];

            if ff.done[v] || capacity <= 0 {
                continue;
            }

            let mut d = std::cmp::min(*flow, capacity);
            if !dfs(v, &mut d, ff) {
                continue;
            }

            ff.edges[ei].capacity -= d;
            ff.edges[rev].capacity += d;
            *flow = d;
            return true;
        }

        false
    }

    let mut ff = FordFulkerson {
        t: t,
        g: vec![vec![]; n],
        edges: vec![],
        done: vec![],
    };

    // グラフを構築する。
    for u in 0..n {
        let k = adjacent_fn(u);
        for i in 0..k {
            let (v, capacity) = edge_fn(u, i);

            let ei = ff.edges.len();
            ff.edges.push(EdgeData {
                v: v,
                capacity: capacity,
                rev: std::usize::MAX,
            });

            let rev = ff.edges.len();
            ff.edges.push(EdgeData {
                v: u,
                capacity: 0,
                rev: ei,
            });

            ff.edges[ei].rev = rev;

            ff.g[u].push(ei);
            ff.g[v].push(rev);
        }
    }

    // DFS.
    let mut total_flow = 0;
    loop {
        ff.done.clear();
        ff.done.resize(n, false);

        let mut flow = INF;
        if dfs(s, &mut flow, &mut ff) {
            total_flow += flow;
            continue;
        }

        return total_flow;
    }
}

#[cfg(test)]
mod tests {
    use super::ford_fulkerson;

    #[test]
    fn test_ford_fulkerson() {
        const N: usize = 6;

        let mut g = vec![vec![]; N];
        for &(u, v, capacity) in &[
            (0, 1, 1),
            (0, 2, 9),
            (0, 3, 3),
            (0, 4, 3),
            (1, 3, 9),
            (2, 4, 3),
            (3, 1, 9),
            (3, 5, 9),
            (4, 5, 1),
        ] {
            g[u].push((v, capacity));
        }

        // Flow:
        // 0 -> 1 -> 3 -> 5 (+1)
        // 0 -> 2 -> 4 -> 5 (+1)
        // 0 -> 3 -> 5 (+3)

        const S: usize = 0;
        const T: usize = 5;
        let flow = ford_fulkerson(S, T, N, |u| g[u].len(), |u, i| g[u][i]);
        assert_eq!(flow, 5);
    }
}
