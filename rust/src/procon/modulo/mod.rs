pub mod modint;

const P: i64 = 1_000_000_007;

/// Calculates `x^n`. O(log n) time.
/// By Fermat's little theorem, `x^(-1) = pow(x, P - 2)`.
pub fn pow(x: i64, n: i64) -> i64 {
    let (mut x, mut y, mut n) = (x % P, 1_i64, n);
    while n > 0 {
        if n % 2 != 0 {
            y = (y * x) % P;
            n -= 1;
        }

        x = (x * x) % P;
        n /= 2;
    }
    y
}

/// Calculates `1/a` for each `a` in `1..n`.
/// Use `P = floor(P / k) * k + P % k` for proof.
pub fn inv_dp(n: usize) -> Vec<i64> {
    let mut dp = vec![0; n];
    if n >= 2 {
        dp[1] = 1;
        for i in 2..n {
            let mut z = P - dp[(P % i as i64) as usize];
            z %= P;
            z *= P / i as i64;
            z %= P;
            dp[i] = z;
        }
    }
    dp
}

#[cfg(test)]
mod tests {
    use super::{inv_dp, pow, P};
    use std;

    #[test]
    fn test_pow_edges() {
        assert_eq!(pow(0, 0), 1);
        assert_eq!(pow(2, 0), 1);
        assert_eq!(pow(3, 1), 3);
        assert_eq!(pow(5, 6), (5 * 5 * 5) * (5 * 5 * 5));
        assert_eq!(pow(std::i64::MAX % P, std::i64::MAX), 856225998);
    }

    #[test]
    fn test_pow_small() {
        for x in 0..100_i64 {
            for n in 0..100_i64 {
                let actual = pow(x, n);
                let expected = (0..n).fold(1_i64, |acc, _| (acc * x) % P);
                assert_eq!(actual, expected, "{}^{}", x, n);
            }
        }
    }

    #[test]
    fn test_inv_dp() {
        let n = 10000;
        let dp = inv_dp(n);
        for i in 1..n {
            let mut z = dp[i] * i as i64;
            z %= P;
            z += P;
            z %= P;
            assert_eq!(z, 1);
        }
    }
}
