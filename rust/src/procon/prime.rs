use std::collections::BTreeMap;

/// Performs prime factorization in O(√x) time.
pub fn factorize(mut x: i64) -> BTreeMap<i64, i64> {
    let mut ms = BTreeMap::new();
    let r = (x as f64).sqrt() as i64 + 1;

    for p in 2..r as i64 {
        let mut m = 0;

        while x >= p && x % p == 0 {
            x /= p;
            m += 1;
        }

        if m > 0 {
            ms.insert(p, m);
        }
    }

    // `x` can have a prime factor larger than √x at most one.
    if x > 1 {
        ms.insert(x, 1);
    }

    ms
}

#[cfg(test)]
mod tests {
    use super::factorize;

    #[test]
    fn test_factorize() {
        let x = 2 * 2 * 2 * 5 * 5 * 7 * 97;
        let m = factorize(x);
        let v = m.into_iter().collect::<Vec<_>>();
        assert_eq!(v, vec![(2, 3), (5, 2), (7, 1), (97, 1)])
    }

    #[test]
    fn test_factorize_edges() {
        assert_eq!(factorize(0).len(), 0);
    }
}
