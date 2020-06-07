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

pub struct ChooseFn {
    fact: Vec<i64>,
    fact_inv: Vec<i64>,
}

impl ChooseFn {
    pub fn new(m: usize) -> Self {
        let mut fact = vec![0; m];
        fact[0] = 1;
        fact[1] = 1;
        for i in 2..m {
            fact[i] = (i as i64) * fact[i - 1] % P;
        }

        let mut fact_inv = vec![0; m];
        fact_inv[m - 1] = pow(fact[m - 1], P - 2);
        for i in (1..m).rev() {
            fact_inv[i - 1] = (i as i64) * fact_inv[i] % P;
        }

        ChooseFn {
            fact: fact,
            fact_inv: fact_inv,
        }
    }

    pub fn call(&self, n: usize, r: usize) -> i64 {
        if n < r {
            return 0;
        }

        let mut t = self.fact[n];
        t *= self.fact_inv[n - r];
        t %= P;
        t *= self.fact_inv[r];
        t % P
    }
}

#[cfg(test)]
mod tests {
    use super::{inv_dp, pow, ChooseFn, P};
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

    #[test]
    fn test_choose() {
        let table = vec![
            vec![1],
            vec![1, 1],
            vec![1, 2, 1],
            vec![1, 3, 3, 1],
            vec![1, 4, 6, 4, 1],
            vec![1, 5, 10, 10, 5, 1],
        ];

        let m = table.len();
        let choose_fn = ChooseFn::new(m + 1);
        for n in 0..m {
            for r in 0..n + 1 {
                assert_eq!(choose_fn.call(n, r), table[n][r]);
            }
        }
    }
}
