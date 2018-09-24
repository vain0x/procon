use std::collections::BTreeMap;

/// Determines if the specified number is a prime or not.
/// O(√x) time.
pub fn is_prime(x: i64) -> bool {
    let r = |x: i64| (x as f64).sqrt() as i64 + 1;
    x >= 2 && (2..r(x)).all(|k| x % k != 0)
}

/// Performs the sieve of Eratosthenes.
/// O(n log (log n)) time.
pub fn sieve(n: usize) -> Vec<bool> {
    let mut sieve = vec![true; n];
    sieve[0] = false;
    sieve[1] = false;

    for p in 2..n {
        if !sieve[p] {
            continue;
        }

        let mut k = 2 * p;
        while k < n {
            sieve[k] = false;
            k += p;
        }
    }

    sieve
}

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
    use super::{factorize, is_prime, sieve};

    #[test]
    fn test_is_prime_edges() {
        assert_eq!(is_prime(1), false);
        assert_eq!(is_prime(2), true);
        assert_eq!(is_prime(1_000_000_007), true);
    }

    #[test]
    fn test_is_prime_small() {
        for x in 1..100 {
            let actual = is_prime(x);
            let expected = x >= 2 && (2..x).all(|d| x % d != 0);
            assert_eq!(actual, expected, "{}", x);
        }
    }

    #[test]
    fn test_sieve_small() {
        let sieve = sieve(50);
        let primes = (0..sieve.len()).filter(|&p| sieve[p]).collect::<Vec<_>>();
        let expected = vec![2, 3, 5, 7, 11, 13, 17, 19, 23, 29, 31, 37, 41, 43, 47];
        assert_eq!(primes, expected);
    }

    #[test]
    fn test_sieve_large() {
        let sieve = sieve(100_000);
        let prime_count = (0..sieve.len()).filter(|&p| sieve[p]).count();
        assert_eq!(prime_count, 9592);
    }

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
