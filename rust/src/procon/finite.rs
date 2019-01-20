use std::ops::*;

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

/// Calculates `a^(-1)` for each `a` in `1..n`.
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

/// Represents an element of finite field.
#[derive(PartialEq, Eq, PartialOrd, Ord, Clone, Copy, Debug)]
struct Finite<T>(T);

trait Normalize {
    fn normalize(&mut self);
}

impl<T> From<T> for Finite<T>
where
    Finite<T>: Normalize,
{
    fn from(value: T) -> Self {
        let mut it = Finite(value);
        it.normalize();
        it
    }
}

// Derive binary operation trait from the `FooAssign` impl.
// Define `x + y` as `{ let mut x = x.clone(); x += y; x }`.
macro_rules! impl_binary_op_for_finite {
    ($op_trait:ident, $op:ident, $assign_trait:ident, $assign:ident) => {
        impl<T: $op_trait<T, Output = T>> $op_trait<T> for Finite<T>
        where
            Finite<T>: Clone + $assign_trait<T>,
        {
            type Output = Self;

            fn $op(self, other: T) -> Self {
                let mut it = self.clone();
                it.$assign(other);
                it
            }
        }

        impl<T: $op_trait<T, Output = T>> $op_trait<Finite<T>> for Finite<T>
        where
            Finite<T>: Clone + $assign_trait<Finite<T>>,
        {
            type Output = Self;

            fn $op(self, other: Self) -> Self {
                let mut it = self.clone();
                it.$assign(other);
                it
            }
        }
    };
}

// Derive assign operation trait by unwrapping the right hand side.
macro_rules! impl_binary_op_assign_with_finite_for_finite {
    ($op_trait:ident, $op:ident) => {
        impl<T> $op_trait<Finite<T>> for Finite<T>
        where
            Finite<T>: $op_trait<T>,
        {
            fn $op(&mut self, other: Self) {
                self.$op(other.0);
            }
        }
    };
}

// Derive assign operation trait from impl for inner type.
macro_rules! impl_binary_op_assign_with_inner_for_finite {
    ($op_trait:ident, $op:ident) => {
        impl<T: $op_trait<T>> $op_trait<T> for Finite<T>
        where
            Finite<T>: Normalize,
        {
            fn $op(&mut self, other: T) {
                let mut other = Finite::from(other);
                other.normalize();
                (self.0).$op(other.0);
                self.normalize();
            }
        }
    };
}

impl_binary_op_for_finite! {Add, add, AddAssign, add_assign}
impl_binary_op_for_finite! {Sub, sub, SubAssign, sub_assign}
impl_binary_op_for_finite! {Mul, mul, MulAssign, mul_assign}
impl_binary_op_for_finite! {Div, div, DivAssign, div_assign}
impl_binary_op_assign_with_inner_for_finite! {AddAssign, add_assign}
impl_binary_op_assign_with_inner_for_finite! {SubAssign, sub_assign}
impl_binary_op_assign_with_inner_for_finite! {MulAssign, mul_assign}
impl_binary_op_assign_with_finite_for_finite! {AddAssign, add_assign}
impl_binary_op_assign_with_finite_for_finite! {SubAssign, sub_assign}
impl_binary_op_assign_with_finite_for_finite! {MulAssign, mul_assign}
impl_binary_op_assign_with_finite_for_finite! {DivAssign, div_assign}

impl Finite<i64> {
    fn pow(self, e: i64) -> Self {
        pow(self.0, e).into()
    }
}

impl Normalize for Finite<i64> {
    fn normalize(&mut self) {
        self.0 %= P;
        self.0 += P;
        self.0 %= P;
    }
}

impl DivAssign<i64> for Finite<i64> {
    fn div_assign(&mut self, other: i64) {
        *self *= Self::from(other).pow(P - 2);
    }
}

#[cfg(test)]
mod tests {
    use super::{inv_dp, pow, Finite, P};
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
    fn test_finite() {
        let x = Finite::from(P + 2);

        // `from` should normalize the value.
        assert_eq!(x.0, 2);

        // Operations.
        assert_eq!(x + 7, (2 + 7).into());
        assert_eq!(x - 7, (P + (2 - 7)).into());
        assert_eq!(x * 3, (2 * 3).into());
        assert_eq!((x / 11) * 11, 2.into());

        assert_eq!(x + Finite::from(7), (2 + 7).into());
        assert_eq!(x - Finite::from(7), (P + (2 - 7)).into());
        assert_eq!(x * Finite::from(3), (2 * 3).into());
        assert_eq!((x / Finite::from(11)) * 11, 2.into());

        let mut x = x;
        x += 7;
        assert_eq!(x, 9.into());
        x -= 5;
        assert_eq!(x, 4.into());
        x *= 6;
        assert_eq!(x, 24.into());
        x /= 3;
        assert_eq!(x, 8.into());
    }
}
