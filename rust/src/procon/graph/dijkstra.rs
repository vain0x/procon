/// Evaluates minimal path cost for each vertex u from `start`.
/// g[u][_] = (u, v, w): an edge from u to v with cost w.
/// dist[v] = cost of minimal-cost path from `start` to `v`.
pub fn dijkstra(g: &Vec<Vec<(usize, usize, i64)>>, start: usize) -> Vec<i64> {
    use std::cmp::Ordering;
    use std::collections::BinaryHeap;

    #[derive(PartialEq, Eq, Ord, Clone, Debug)]
    struct Route {
        cost: i64,
        vertex: usize,
    };

    impl PartialOrd for Route {
        fn partial_cmp(&self, other: &Self) -> Option<Ordering> {
            other.cost.partial_cmp(&self.cost)
        }
    }

    let n = g.len();
    let mut dist = vec![std::i64::MAX; n];
    dist[start] = 0;

    let mut heap = BinaryHeap::new();
    heap.push(Route {
        cost: 0,
        vertex: start,
    });

    while let Some(Route { cost: d, vertex: u }) = heap.pop() {
        if d > dist[u] {
            continue;
        }

        for &(_, v, cost) in g[u].iter() {
            let d = d + cost;

            if d < dist[v] {
                dist[v] = d;
                heap.push(Route { cost: d, vertex: v });
            }
        }
    }

    dist
}

#[cfg(test)]
mod tests {
    use super::dijkstra;

    #[test]
    fn test() {
        //       1
        //   [0] --> [1]*
        //      \     |
        //     3  \   | 5
        //          \ |
        //   [2] --> [3]
        //       7

        let n = 4;
        let edges = vec![(0, 1, 1), (0, 3, 3), (1, 3, 5), (2, 3, 7)];
        let mut g = vec![vec![]; n];
        for (u, v, w) in edges {
            g[u].push((u, v, w));
            g[v].push((v, u, w));
        }

        let dist = dijkstra(&g, 1);
        assert_eq!(dist, vec![1, 0, 1 + 3 + 7, 1 + 3]);
    }
}
