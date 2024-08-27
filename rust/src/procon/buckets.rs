//! Provides basic bucket structure of square root decomposition.

#[derive(Clone, Debug)]
pub struct BucketVec<T, F> {
    node: Vec<T>,
    bucket: Vec<(usize, usize, T)>,
    bucket_size: usize,
    mempty: T,
    mappend: F,
}

impl<T, F> BucketVec<T, F>
where
    T: Clone,
    F: Fn(T, T) -> T,
{
    pub fn new(node: Vec<T>, mempty: T, mappend: F) -> BucketVec<T, F> {
        let bucket_size = (node.len() as f64).sqrt() as usize + 1;

        // Chunkify nodes into buckets. Bucket sums are inconsistent yet.
        let mut bucket = vec![];
        let mut l = 0;
        let mut r = 1;
        while r <= node.len() {
            if r == node.len() || r % bucket_size == 0 {
                bucket.push((l, r, mempty.clone()));
                l = r;
            }
            r += 1;
        }

        let mut it = BucketVec {
            node,
            bucket,
            bucket_size,
            mempty,
            mappend,
        };
        for i in 0..it.bucket.len() {
            it.refresh(i * bucket_size);
        }

        it
    }

    fn mempty(&self) -> T {
        self.mempty.clone()
    }

    /// Does `*l (mpappend)= r` in an efficient way.
    fn mappend_mut(&self, l: &mut T, r: T) {
        let t = std::mem::replace(l, self.mempty());
        *l = (self.mappend)(t, r);
    }

    /// Recalculates the bucket sum of node `i`.
    fn refresh(&mut self, i: usize) {
        let bi = i / self.bucket_size;
        let (bl, br, _) = self.bucket[bi];
        let mut sum = self.mempty();
        for i in bl..br {
            self.mappend_mut(&mut sum, self.node[i].clone());
        }
        self.bucket[bi] = (bl, br, sum);
    }

    pub fn set(&mut self, i: usize, item: T) {
        self.node[i] = item;
        self.refresh(i);
    }

    /// Calculates sum of nodes `ql..qr`. O(√N).
    pub fn sum(&self, ql: usize, qr: usize) -> T {
        // Range of buckets that involves in the query.
        let lbi = ql / self.bucket_size;
        let rbi = (qr + self.bucket_size - 1) / self.bucket_size;

        let mut sum = self.mempty();
        for bi in lbi..rbi {
            let &(bl, br, ref acc) = &self.bucket[bi];

            let l = if bl <= ql && ql < br { ql } else { bl };
            let r = if bl < qr && qr <= br { qr } else { br };

            if l == bl && r == br {
                self.mappend_mut(&mut sum, acc.clone());
            } else {
                // If l or r isn't at boundary of buckets,
                // we need to sum up partially-covered nodes.

                let mut acc = self.mempty();
                for i in l..r {
                    self.mappend_mut(&mut acc, self.node[i].clone());
                }
                self.mappend_mut(&mut sum, acc);
            }
        }
        sum
    }
}

#[cfg(test)]
mod test {
    use super::BucketVec;

    #[derive(PartialEq, Clone, Copy, Debug)]
    struct Linear(i64, i64);

    #[test]
    fn test() {
        let id = Linear(1, 0);
        let mut v = BucketVec::new(vec![id; 8], id, |Linear(la, lb), Linear(ra, rb)| {
            Linear(ra * la, rb + ra * lb)
        });

        for i in 0..8 {
            let x = 1 + i as i64;
            v.set(i, Linear(x, x));
        }

        assert_eq!(v.sum(0, 8).0, 1 * 2 * 3 * 4 * 5 * 6 * 7 * 8);
        assert_eq!(v.sum(2, 7), Linear(3 * 4 * 5 * 6 * 7, 3619));
        assert_eq!(v.sum(3, 6), Linear(4 * 5 * 6, 156));
    }

    #[test]
    fn test_buckets_with_initial() {
        let mut v = BucketVec::new((0..5).collect(), 0, |l, r| l + r);
        let mut s = 0 + 1 + 2 + 3 + 4;
        assert_eq!(v.sum(0, 5), s);

        for i in 0..5 {
            v.set(i, i + 10);
            s += 10;
            assert_eq!(v.sum(0, 5), s);
        }
    }
}
