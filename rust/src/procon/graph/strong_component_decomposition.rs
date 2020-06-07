// Verified: http://judge.u-aizu.ac.jp/onlinejudge/review.jsp?rid=3429517#1

#![allow(non_snake_case)]

use std;

pub struct StrongComponentDecomposition<'a> {
    /// Number of vertices.
    N: usize,
    /// The graph.
    G: &'a [Vec<usize>],
    /// Dual of the graph.
    H: Vec<Vec<usize>>,
    /// gray[u] = true if the vertex u is visited.
    gray: Vec<bool>,
    /// Topological order index for each vertex.
    top_ord: Vec<usize>,
    /// List of vertices in topological order.
    top_seq: Vec<usize>,
    /// Next index of topological order.
    next_ord: usize,
    /// Output of the process.
    components: Vec<Vec<usize>>,
}

impl<'a> StrongComponentDecomposition<'a> {
    pub fn run(G: &'a [Vec<usize>]) -> Vec<Vec<usize>> {
        let N = G.len();

        let mut H = vec![vec![]; N];
        for u in 0..N {
            for &v in &G[u] {
                H[v].push(u);
            }
        }

        let it = StrongComponentDecomposition {
            N: N,
            G: G,
            H: H,
            gray: vec![],
            top_ord: vec![N; N],
            top_seq: vec![],
            next_ord: 0,
            components: vec![],
        };
        it.start()
    }

    fn start(mut self) -> Vec<Vec<usize>> {
        self.gray = vec![false; self.N];
        for u in 0..self.N {
            self.top(u);
        }

        self.gray = vec![false; self.N];
        let mut bot_seq = std::mem::replace(&mut self.top_seq, vec![]);
        bot_seq.reverse();
        for u in bot_seq {
            if self.gray[u] {
                continue;
            }

            // Here u is the last vertex in topological order.

            let mut component = vec![];
            self.bot(&mut component, u);
            self.components.push(component);
        }

        self.components
    }

    /// Topological sort.
    fn top(&mut self, u: usize) {
        if self.gray[u] {
            return;
        }
        self.gray[u] = true;

        for i in 0..self.G[u].len() {
            let v = self.G[u][i];
            self.top(v);
        }

        self.top_seq.push(u);
        self.top_ord[u] = self.next_ord;
        self.next_ord += 1;
    }

    /// DFS over the dual.
    fn bot(&mut self, c: &mut Vec<usize>, u: usize) {
        if self.gray[u] {
            return;
        }

        self.gray[u] = true;
        c.push(u);

        for i in 0..self.H[u].len() {
            let v = self.H[u][i];
            self.bot(c, v);
        }
    }
}
