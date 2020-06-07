// BFS のサンプル。

#[allow(dead_code)]
fn bfs() {
    use std::collections::VecDeque;

    const INF: usize = 1 << 60;

    let n = 0;
    let g = vec![vec![]; n];
    let s = 0;

    let mut dist = vec![INF; n];
    dist[s] = 0;

    let mut q = VecDeque::new();
    q.push_back(s);

    while let Some(u) = q.pop_front() {
        for &v in &g[u] {
            if dist[v] != INF {
                continue;
            }

            q.push_back(v);
            dist[v] = dist[u] + 1;
        }
    }
}
