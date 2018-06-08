use std::ops::{AddAssign, SubAssign};

// `inner` is 1-indexed. [0] is unused.
pub struct BinaryIndexedTree<T> {
    inner: Vec<T>,
}

impl<T> BinaryIndexedTree<T>
where
    T: Clone + Default + AddAssign,
{
    pub fn new(len: usize) -> BinaryIndexedTree<T> {
        BinaryIndexedTree {
            inner: vec![T::default(); len + 1],
        }
    }

    pub fn len(&self) -> usize {
        self.inner.len() - 1
    }

    /// Increments an element.
    pub fn add(&mut self, index: usize, value: T) {
        let mut j = index + 1;
        while j < self.len() {
            self.inner[j] += value.clone();
            j += rightmost_bit(j);
        }
    }

    /// Gets sum of items in range 0..right.
    pub fn acc(&self, right: usize) -> T {
        let mut acc = T::default();
        let mut j = right;
        while 0 < j && j < self.inner.len() {
            acc += self.inner[j].clone();
            j -= rightmost_bit(j);
        }
        acc
    }
}

impl<T> BinaryIndexedTree<T>
where
    T: Clone + Default + AddAssign + SubAssign,
{
    pub fn sum(&self, left: usize, right: usize) -> T {
        let mut acc = self.acc(right);
        acc -= self.acc(left);
        acc
    }
}

fn rightmost_bit(n: usize) -> usize {
    let s = n as isize;
    (s & -s) as usize
}

#[cfg(test)]
mod tests {
    use super::BinaryIndexedTree;

    #[test]
    fn test() {
        let mut bit = BinaryIndexedTree::<i32>::new(6);
        for (i, &x) in [3, 1, 4, 1, 5, 9].into_iter().enumerate() {
            bit.add(i, x);
        }

        assert_eq!(1 + 4 + 1, bit.sum(1, 4));
    }
}
