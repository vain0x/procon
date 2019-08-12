use std;
use std::collections::VecDeque;

/// (O(|V||E|) 時間)
/// `dist[t]`: 頂点 s から t への長さ最大 k の経路の重みの最小値。
/// `neg`: グラフのどこかに負閉路が存在する。経路上とは限らない。
/// Verify: https://atcoder.jp/contests/abc137/tasks/abc137_e
pub fn bellman_ford(s: usize, g: &[Vec<(usize, i64)>], dist: &mut [i64], neg: &mut bool) {
    let n = g.len();

    for i in 0..n {
        dist[i] = if i == s { 0 } else { std::i64::MAX };
    }
    *neg = false;

    for k in 0..n {
        for u in 0..n {
            for i in 0..g[u].len() {
                let (v, w) = g[u][i];
                if dist[u] == std::i64::MAX || dist[v] <= dist[u] + w {
                    continue;
                }

                dist[v] = dist[u] + w;
                if k == n - 1 {
                    *neg = true;
                }
            }
        }
    }
}

/// 各頂点が頂点 s から到達可能か否かを判定する。(BFS)
fn reachable<W>(s: usize, g: &[Vec<(usize, W)>]) -> Vec<bool> {
    let n = g.len();

    let mut done = vec![false; n];
    let mut q = VecDeque::new();
    q.push_back(s);

    while let Some(u) = q.pop_front() {
        if done[u] {
            continue;
        }

        done[u] = true;

        for i in 0..g[u].len() {
            let &(v, _) = &g[u][i];
            q.push_back(v);
        }
    }

    done
}

/// 頂点 s から t への経路上に存在しない頂点を削除する。
pub fn prune<W: Clone>(s: usize, t: usize, g: &mut [Vec<(usize, W)>]) {
    let n = g.len();

    let done = reachable(s, g);
    for es in g.iter_mut() {
        es.retain(|&(v, _)| done[v]);
    }

    let mut dual = vec![vec![]; n];
    for u in 0..n {
        for i in 0..g[u].len() {
            let (v, w) = g[u][i].clone();
            dual[v].push((u, w));
        }
    }

    let done = reachable(t, &dual);
    for es in g.iter_mut() {
        es.retain(|&(v, _)| done[v]);
    }
}
