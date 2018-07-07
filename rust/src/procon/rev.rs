use std::cmp::Ordering;

#[derive(PartialEq, Eq, Clone, Debug)]
pub struct Rev<T>(pub T);

impl<T: PartialOrd> PartialOrd for Rev<T> {
    fn partial_cmp(&self, other: &Rev<T>) -> Option<Ordering> {
        other.0.partial_cmp(&self.0)
    }
}

impl<T: Ord> Ord for Rev<T> {
    fn cmp(&self, other: &Rev<T>) -> Ordering {
        other.0.cmp(&self.0)
    }
}

#[cfg(test)]
mod tests {
    use super::Rev;

    #[test]
    fn test_rev() {
        let mut xs = (0..5).map(|i| Rev((i * i * i % 13, i))).collect::<Vec<_>>();

        xs.sort();

        let xs = xs.into_iter().map(|Rev(t)| t).collect::<Vec<_>>();

        assert_eq!(xs, vec![(12, 4), (8, 2), (1, 3), (1, 1), (0, 0)]);
    }
}
