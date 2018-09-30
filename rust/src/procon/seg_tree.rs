use std;
use std::marker::PhantomData;

pub trait MonoidOp {
    type T;

    fn empty() -> Self::T;
    fn append(l: Self::T, r: Self::T) -> Self::T;
}

#[derive(PartialEq, Clone, Debug)]
struct Interval<T> {
    l: T,
    r: T,
}

impl<T: Ord> Interval<T> {
    fn new(l: T, r: T) -> Interval<T> {
        Interval { l: l, r: r }
    }

    fn disjoint(&self, other: &Self) -> bool {
        self.r <= other.l || other.r <= self.l
    }

    fn covers(&self, other: &Self) -> bool {
        self.l <= other.l && other.r <= self.r
    }
}

#[derive(Debug)]
pub struct SegTree<T, M> {
    len: usize,

    /// Number of leaf nodes.
    width: usize,

    /// len = `2w-1` for `w-1` inners and `w` leaves,
    /// where `w` is the smallest `2^p` (`>= len`).
    node: Vec<T>,

    monoid_op: PhantomData<M>,
}

impl<T: Clone, M: MonoidOp<T = T>> SegTree<T, M> {
    pub fn new(items: Vec<T>) -> SegTree<T, M> {
        let len = items.len();

        let mut w = 1;
        while w < len {
            w *= 2;
        }
        debug_assert!(w >= len);

        let mut node = vec![M::empty(); 2 * w - 1];

        for (ei, item) in items.into_iter().enumerate() {
            node[w - 1 + ei] = item;
        }

        for ni in (0..w - 1).rev() {
            node[ni] = M::append(node[2 * ni + 1].clone(), node[2 * ni + 2].clone());
        }

        SegTree {
            len: len,
            width: w,
            node: node,
            monoid_op: std::marker::PhantomData,
        }
    }

    pub fn len(&self) -> usize {
        self.len
    }

    pub fn set(&mut self, ei: usize, value: T) {
        let mut ni = self.width - 1 + ei;
        self.node[ni] = value;

        while ni > 0 {
            ni = (ni - 1) / 2;
            self.node[ni] = M::append(self.node[2 * ni + 1].clone(), self.node[2 * ni + 2].clone());
        }
    }

    pub fn sum(&self, ql: usize, qr: usize) -> T {
        let q = Interval::new(ql, qr);
        if q.disjoint(&Interval::new(0, self.len())) {
            M::empty()
        } else {
            self.sum_core(0, Interval::new(0, self.width), &q)
        }
    }

    fn sum_core(&self, ni: usize, e: Interval<usize>, q: &Interval<usize>) -> T {
        if e.disjoint(&q) {
            M::empty()
        } else if q.covers(&e) {
            self.node[ni].clone()
        } else {
            let m = (e.l + e.r) / 2;
            let vl = self.sum_core(2 * ni + 1, Interval::new(e.l, m), q);
            let vr = self.sum_core(2 * ni + 2, Interval::new(m, e.r), q);
            M::append(vl.clone(), vr.clone())
        }
    }
}

#[cfg(test)]
mod tests {
    use super::{MonoidOp, SegTree};
    use std;
    use std::cmp::min;
    use std::marker::PhantomData;

    #[derive(Debug)]
    struct Min<T>(PhantomData<T>);

    impl MonoidOp for Min<i64> {
        type T = i64;

        fn empty() -> Self::T {
            std::i64::MAX
        }

        fn append(l: Self::T, r: Self::T) -> Self::T {
            min(l, r)
        }
    }

    #[test]
    fn test_segtree_min() {
        let mut tree = SegTree::<_, Min<_>>::new(vec![10; 10]);

        assert_eq!(tree.sum(0, 10), 10);
        assert_eq!(tree.sum(10, 10), std::i64::MAX);

        tree.set(5, 5);
        assert_eq!(tree.sum(0, 5), 10);
        assert_eq!(tree.sum(5, 6), 5);
        assert_eq!(tree.sum(6, 10), 10);
        assert_eq!(tree.sum(4, 8), 5);
        assert_eq!(tree.sum(0, 10), 5);

        tree.set(3, 3);
        assert_eq!(tree.sum(0, 3), 10);
        assert_eq!(tree.sum(3, 5), 3);
        assert_eq!(tree.sum(5, 10), 5);
        assert_eq!(tree.sum(0, 10), 3);
    }
}
