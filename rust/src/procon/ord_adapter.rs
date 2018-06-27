use std::cmp::*;

/// Wraps a partial-ord value to impl Ord.
#[derive(PartialEq, PartialOrd, Clone, Copy, Debug)]
pub struct OrdAdapter<T>(pub T);

impl<T: PartialEq> Eq for OrdAdapter<T> {}

impl<T: PartialOrd> Ord for OrdAdapter<T> {
    fn cmp(&self, other: &Self) -> Ordering {
        self.partial_cmp(other).unwrap()
    }
}

#[cfg(test)]
#[allow(unused_imports)]
mod tests {
    use super::*;
    use std::f64::consts::{E, PI};

    #[test]
    fn test_ord() {
        let e = OrdAdapter(E);
        let pi = OrdAdapter(PI);
        assert_eq!(e.cmp(&pi), Ordering::Less);
    }
}
