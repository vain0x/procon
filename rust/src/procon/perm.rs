/// Rearranges elements in the slice
/// to the next lexicographically larger permutation.
pub fn next_perm<T: Ord>(xs: &mut [T]) -> bool {
    // `xs[i + 1..]` : desc but
    // `xs[i..]` : not desc.
    let i = match (0..xs.len())
        .rev()
        .filter(|&i| i + 1 < xs.len() && xs[i] < xs[i + 1])
        .next()
    {
        None => return false,
        Some(i) => i,
    };

    // `xs[k]` : The next greater elem in `xs[i..]`.
    let k = (i + 1..xs.len())
        .rev()
        .filter(|&k| xs[i] < xs[k])
        .next()
        .unwrap();

    // E.g. 2431 -> 3421 -> 3124 (where i = 0, k = 2).
    xs.swap(i, k);
    xs[i + 1..].reverse();

    true
}

#[cfg(test)]
mod tests {
    use super::next_perm;

    #[test]
    fn test_next_perm_corners() {
        let mut zero: Vec<usize> = vec![];
        assert_eq!(next_perm(&mut zero), false);

        let mut one: Vec<usize> = vec![0];
        assert_eq!(next_perm(&mut one), false);
    }

    #[test]
    fn test_next_perm_order_3() {
        let mut xs: Vec<usize> = vec![0, 1, 2];
        let mut perms: Vec<Vec<usize>> = vec![];

        loop {
            perms.push(xs.iter().cloned().collect());
            if !next_perm(&mut xs) {
                break;
            }
        }

        assert_eq!(
            perms,
            vec![
                vec![0, 1, 2],
                vec![0, 2, 1],
                vec![1, 0, 2],
                vec![1, 2, 0],
                vec![2, 0, 1],
                vec![2, 1, 0],
            ]
        );
    }

    #[test]
    fn test_next_perm_count() {
        let mut xs: Vec<usize> = (0..5).collect();
        let mut count: usize = 0;
        loop {
            count += 1;
            if !next_perm(&mut xs) {
                break;
            }
        }
        assert_eq!(count, 1 * 2 * 3 * 4 * 5);
    }
}
