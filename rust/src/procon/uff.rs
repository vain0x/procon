pub mod uff {
    use std::*;

    pub struct UnionFindForest {
        nodes: Vec<UnionFindForestNode>,
    }

    enum UnionFindForestNode {
        Root(usize),
        Child(usize),
    }

    impl UnionFindForest {
        pub fn new(size: usize) -> Self {
            UnionFindForest {
                nodes: (0..size)
                    .map(|_| UnionFindForestNode::Root(1))
                    .collect::<Vec<_>>(),
            }
        }

        pub fn root_node(&mut self, v: usize) -> (usize, usize) {
            match self.nodes[v] {
                UnionFindForestNode::Root(rank) => (v, rank),
                UnionFindForestNode::Child(u) => {
                    let (u, rank) = self.root_node(u);
                    self.nodes[v] = UnionFindForestNode::Child(u);
                    (u, rank)
                }
            }
        }

        pub fn root(&mut self, v: usize) -> usize {
            self.root_node(v).0
        }

        pub fn connects(&mut self, u: usize, v: usize) -> bool {
            self.root(u) == self.root(v)
        }

        pub fn merge(&mut self, u: usize, v: usize) {
            let (u, u_rank) = self.root_node(u);
            let (v, v_rank) = self.root_node(v);

            if u == v {
                return;
            }

            if u_rank < v_rank {
                self.merge(v, u);
                return;
            }

            self.nodes[v] = UnionFindForestNode::Child(u);
            self.nodes[u] = UnionFindForestNode::Root(u_rank + 1);
        }
    }
}
