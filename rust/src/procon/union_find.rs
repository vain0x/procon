//! Verified at <https://atcoder.jp/contests/abc120/submissions/4468801>.

pub struct UnionFind {
    nodes: Vec<UnionFindNode>,
}

enum UnionFindNode {
    Root { size: usize },
    Child { parent: std::cell::Cell<usize> },
}

impl UnionFind {
    /// Create an union find with `size` vertices.
    pub fn new(size: usize) -> Self {
        UnionFind {
            nodes: (0..size)
                .map(|_| UnionFindNode::Root { size: 1 })
                .collect::<Vec<_>>(),
        }
    }

    /// Get the root index and its component size.
    fn root_node(&self, v: usize) -> (usize, usize) {
        match &self.nodes[v] {
            &UnionFindNode::Root { size } => (v, size),
            &UnionFindNode::Child { parent: ref p } => {
                let (u, size) = self.root_node(p.get());
                // Path compression.
                p.set(u);
                (u, size)
            }
        }
    }

    /// Get the root index.
    pub fn root(&self, v: usize) -> usize {
        self.root_node(v).0
    }

    /// Get the size of component.
    pub fn size(&self, v: usize) -> usize {
        self.root_node(v).1
    }

    /// Determine if two vertices are in the same component.
    pub fn connects(&self, u: usize, v: usize) -> bool {
        self.root(u) == self.root(v)
    }

    /// Merge components by adding an edge between the two vertices.
    pub fn merge(&mut self, u: usize, v: usize) {
        let (u, u_size) = self.root_node(u);
        let (v, v_size) = self.root_node(v);

        if u == v {
            return;
        }

        if u_size < v_size {
            self.merge(v, u);
            return;
        }

        self.nodes[v] = UnionFindNode::Child {
            parent: std::cell::Cell::new(u),
        };
        self.nodes[u] = UnionFindNode::Root {
            size: u_size + v_size,
        };
    }
}
