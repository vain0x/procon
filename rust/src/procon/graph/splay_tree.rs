//! FIXME: incomplete. verify and test

#[derive(Clone, Copy)]
struct SplayNode(usize);

#[derive(Clone)]
struct SplayNodeData<T> {
    value: Option<T>,
    left: Option<SplayNode>,
    right: Option<SplayNode>,
}

impl<T> SplayNodeData<T> {
    fn new() -> Self {
        SplayNodeData {
            value: None,
            left: None,
            right: None,
        }
    }
}

#[derive(Clone)]
pub struct SplayTree<T> {
    data: Vec<SplayNodeData<T>>,
    root: Option<SplayNode>,
}

impl<T> SplayTree<T> {
    pub fn new() -> Self {
        SplayTree::default()
    }

    fn value(&self, node: SplayNode) -> Option<&T> {
        self.data.get(node.0).and_then(|n| n.value.as_ref())
    }

    fn left(&self, node: SplayNode) -> Option<SplayNode> {
        self.data.get(node.0).and_then(|n| n.left)
    }

    fn left_mut(&mut self, node: SplayNode) -> Option<&mut Option<SplayNode>> {
        self.data.get_mut(node.0).map(|n| &mut n.left)
    }

    fn right(&self, node: SplayNode) -> Option<SplayNode> {
        self.data.get(node.0).and_then(|n| n.right)
    }

    fn right_mut(&mut self, node: SplayNode) -> Option<&mut Option<SplayNode>> {
        self.data.get_mut(node.0).map(|n| &mut n.right)
    }

    fn left_rotate(&mut self, node: SplayNode) -> SplayNode {
        debug_assert!(self.right(node).is_some());

        let right = self.right(node).unwrap();
        let right_left = self.left(right);
        *self.right_mut(node).unwrap() = right_left;
        *self.left_mut(right).unwrap() = Some(node);
        right
    }

    fn right_rotate(&mut self, node: SplayNode) -> SplayNode {
        debug_assert!(self.left(node).is_some());

        let left = self.left(node).unwrap();
        let left_right = self.right(left);
        *self.left_mut(node).unwrap() = left_right;
        *self.right_mut(left).unwrap() = Some(node);
        left
    }

    /// node を根とする部分木の最右ノードが根になるように回転する。node は取り除く。
    fn pop_rotate(&mut self, node: SplayNode) -> SplayNode {
        debug_assert!(self.right(node).is_some());

        let mut bottom = node;
        let mut root = node;

        while let Some(right) = self.right(root) {
            bottom = root;
            root = right;
        }

        let root = self.right(bottom).unwrap();
        let root_sibling = self.left(root);
        *self.right_mut(bottom).unwrap() = root_sibling;
        let (left, right) = (self.left(node), self.right(node));
        *self.left_mut(root).unwrap() = left;
        *self.right_mut(root).unwrap() = right;
        root
    }

    fn splay<Q>(&mut self, mut node: SplayNode, value: &Q) -> SplayNode
    where
        T: std::borrow::Borrow<Q>,
        Q: PartialOrd,
    {
        match self
            .value(node)
            .and_then(|it| it.borrow().partial_cmp(value))
        {
            None | Some(std::cmp::Ordering::Equal) => return node,
            Some(std::cmp::Ordering::Greater) => {
                let left = match self.left(node) {
                    None => return node,
                    Some(left) => left,
                };

                match self
                    .value(left)
                    .and_then(|left_value| left_value.borrow().partial_cmp(value))
                {
                    None | Some(std::cmp::Ordering::Equal) => {}
                    Some(std::cmp::Ordering::Greater) => {
                        let left_left = self.left(node).map(|n| self.splay(n, value));
                        *self.left_mut(node).unwrap() = left_left;
                        node = self.right_rotate(node);
                    }
                    Some(std::cmp::Ordering::Less) => {
                        let left_right = self.right(left).map(|n| self.splay(n, value));
                        *self.right_mut(node).unwrap() = left_right;

                        if left_right.is_some() {
                            let left = self.left_rotate(left);
                            *self.left_mut(node).unwrap() = Some(left);
                        }
                    }
                }

                if self.left(node).is_none() {
                    return node;
                }

                self.right_rotate(node)
            }
            Some(std::cmp::Ordering::Less) => {
                let right = match self.right(node) {
                    None => return node,
                    Some(right) => right,
                };

                match self
                    .value(right)
                    .and_then(|right_value| right_value.borrow().partial_cmp(value))
                {
                    None | Some(std::cmp::Ordering::Equal) => {}
                    Some(std::cmp::Ordering::Greater) => {
                        let right_left = self.left(right).map(|n| self.splay(n, value));
                        *self.left_mut(right).unwrap() = right_left;

                        if right_left.is_some() {
                            let right = self.right_rotate(right);
                            *self.right_mut(node).unwrap() = Some(right);
                        }
                    }
                    Some(std::cmp::Ordering::Less) => {
                        let right_right = self.right(right).map(|n| self.splay(n, value));
                        *self.right_mut(right).unwrap() = right_right;
                        node = self.left_rotate(node);
                    }
                }

                if self.right(node).is_none() {
                    return node;
                }

                self.left_rotate(node)
            }
        }
    }

    fn find_node<Q>(&mut self, query: &Q) -> Option<SplayNode>
    where
        T: std::borrow::Borrow<Q>,
        Q: PartialOrd,
    {
        let root = self.root.map(|n| self.splay(n, query));
        self.root = root;

        match self.root.and_then(|n| self.value(n)) {
            Some(root_value) if root_value.borrow() == query => root,
            _ => None,
        }
    }

    fn create_node(&mut self) -> SplayNode {
        let node = SplayNode(self.data.len());
        self.data.push(SplayNodeData::new());
        node
    }

    /// 結果: 挿入されたノード、または入力値と同じ値を持っているノード
    fn do_insert<Q>(&mut self, node: SplayNode, query: &Q) -> SplayNode
    where
        T: std::borrow::Borrow<Q>,
        Q: PartialOrd,
    {
        match self
            .value(node)
            .and_then(|v| v.borrow().partial_cmp(&query))
        {
            None => {
                let left = self.create_node();
                *self.left_mut(node).unwrap() = Some(left);
                return left;
            }
            Some(std::cmp::Ordering::Equal) => return node,
            Some(std::cmp::Ordering::Greater) => match self.left(node) {
                Some(left) => return self.do_insert(left, query),
                None => {
                    let left = self.create_node();
                    *self.left_mut(node).unwrap() = Some(left);
                    return left;
                }
            },
            Some(std::cmp::Ordering::Less) => match self.right(node) {
                Some(right) => return self.do_insert(right, query),
                None => {
                    let right = self.create_node();
                    *self.right_mut(node).unwrap() = Some(right);
                    return right;
                }
            },
        }
    }

    pub fn insert(&mut self, value: T) -> Option<T>
    where
        T: PartialOrd,
    {
        self.root = self.root.map(|n| self.splay(n, &value));

        let node = match self.root {
            Some(root) => self.do_insert(root, &value),
            None => {
                let node = self.create_node();
                self.root = Some(node);
                node
            }
        };
        std::mem::replace(&mut self.data.get_mut(node.0).unwrap().value, Some(value))
    }

    /// node を根とする部分木から値が value に等しいノードを削除する。
    /// 結果: (この部分木の新しい根, 削除されたノード)
    fn do_remove<Q>(
        &mut self,
        node: SplayNode,
        query: &Q,
        removed: &mut Option<SplayNode>,
    ) -> Option<SplayNode>
    where
        T: std::borrow::Borrow<Q>,
        Q: PartialOrd,
    {
        match self.value(node).and_then(|v| v.borrow().partial_cmp(query)) {
            None => {}
            Some(std::cmp::Ordering::Equal) => {
                *removed = Some(node);
                match (self.left(node), self.right(node)) {
                    (None, None) => return None,
                    (None, Some(root)) | (Some(root), None) => return Some(root),
                    (Some(_), Some(_)) => return Some(self.pop_rotate(node)),
                }
            }
            Some(std::cmp::Ordering::Greater) => match self.left(node) {
                Some(left) => {
                    let left = self.do_remove(left, query, removed);
                    *self.left_mut(node).unwrap() = left;
                    return Some(node);
                }
                None => {}
            },
            Some(std::cmp::Ordering::Less) => match self.right(node) {
                Some(right) => {
                    let right = self.do_remove(right, query, removed);
                    *self.right_mut(node).unwrap() = right;
                    return Some(node);
                }
                None => {}
            },
        }

        // 削除するノードがない。
        debug_assert!(removed.is_none());
        Some(node)
    }

    pub fn remove<Q>(&mut self, query: &Q) -> Option<T>
    where
        T: std::borrow::Borrow<Q>,
        Q: PartialOrd,
    {
        let root = self.find_node(query)?;

        let mut removed = None;
        self.root = self.do_remove(root, query, &mut removed);

        removed.and_then(|n| std::mem::take(&mut self.data[n.0].value))
    }

    fn do_dump<W: std::io::Write>(
        &self,
        node: SplayNode,
        indent: usize,
        out: &mut W,
    ) -> std::io::Result<()>
    where
        T: std::fmt::Debug,
    {
        if indent >= 1 {
            write!(out, "\n")?;
            for _ in 0..indent {
                write!(out, "  ")?;
            }
        }

        match self.value(node) {
            Some(value) => write!(out, "{:?}", value)?,
            None => write!(out, "???")?,
        }

        match (self.left(node), self.right(node)) {
            (None, None) => Ok(()),
            (Some(left), None) => {
                self.do_dump(left, indent + 1, out)?;
                write!(out, "\n")?;
                for _ in 0..(indent + 1) {
                    write!(out, "  ")?;
                }
                write!(out, "right = null")
            }
            (None, Some(right)) => {
                write!(out, "\n")?;
                for _ in 0..(indent + 1) {
                    write!(out, "  ")?;
                }
                write!(out, "left = null")?;

                self.do_dump(right, indent + 1, out)
            }
            (Some(left), Some(right)) => {
                self.do_dump(left, indent + 1, out)?;
                self.do_dump(right, indent + 1, out)
            }
        }
    }

    fn dump<W: std::io::Write>(&self, out: &mut W) -> std::io::Result<()>
    where
        T: std::fmt::Debug,
    {
        match self.root {
            Some(root) => self.do_dump(root, 0, out),
            None => write!(out, "root = null\n"),
        }
    }
}

impl<T: std::fmt::Debug> std::fmt::Debug for SplayTree<T> {
    fn fmt(&self, f: &mut std::fmt::Formatter) -> std::fmt::Result {
        let mut s = Vec::new();
        self.dump(&mut s).ok();
        write!(f, "{}", String::from_utf8_lossy(&s))
    }
}

impl<T> Default for SplayTree<T> {
    fn default() -> Self {
        SplayTree {
            data: vec![],
            root: None,
        }
    }
}

#[cfg(test)]
mod tests {
    use super::SplayTree;

    #[test]
    fn test() {
        // FIXME: assert!

        let mut t = SplayTree::new();

        for i in 0..10 {
            let j = t.insert(i);
            eprintln!("insert i = {} (prev = {:?}), tree:\n{:?}", i, j, t);
        }

        for i in 0..10 {
            let j = t.insert(i);
            eprintln!("insert i = {} (prev = {:?}), tree:\n{:?}", i, j, t);
        }

        for i in 0..10 {
            let j = t.remove(&i);
            eprintln!("remove i = {} (obtain {:?}), tree\n{:?}", i, j, t);
        }

        // panic!()
    }
}
