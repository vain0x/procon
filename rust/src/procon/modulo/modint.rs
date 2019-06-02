use super::*;

use std;
use std::fmt::{Debug, Display, Formatter};
use std::ops::*;

/// Represents an element of ModInt field.
#[derive(PartialEq, Eq, PartialOrd, Ord, Clone, Copy, Default)]
struct ModInt(i64);

impl ModInt {
    fn pow(self, e: i64) -> Self {
        pow(self.0, e).into()
    }
}

impl Debug for ModInt {
    fn fmt(&self, formatter: &mut Formatter) -> std::fmt::Result {
        formatter.write_fmt(format_args!("{:?}", self.0))
    }
}

impl Display for ModInt {
    fn fmt(&self, formatter: &mut Formatter) -> std::fmt::Result {
        formatter.write_fmt(format_args!("{}", self.0))
    }
}

impl From<i64> for ModInt {
    fn from(value: i64) -> Self {
        ModInt((value % P + P) % P)
    }
}

// Generate binary operation traits.
macro_rules! impl_binary_op_for_modint {
    ($op_trait:ident, $op:ident, $assign_trait:ident, $assign:ident $(, $f:ident)*) => {
        $(impl $op_trait<ModInt> for ModInt {
            type Output = Self;

            fn $op(self, other: Self) -> Self {
                ModInt::from((self.0).$f(other.0))
            }
        })*

        impl $op_trait<i64> for ModInt {
            type Output = Self;

            fn $op(self, other: i64) -> Self {
                self.$op(ModInt::from(other))
            }
        }

        impl $assign_trait<ModInt> for ModInt {
            fn $assign(&mut self, other: Self) {
                *self = self.$op(other)
            }
        }

        impl $assign_trait<i64> for ModInt {
            fn $assign(&mut self, other: i64) {
                *self = self.$op(other)
            }
        }
    };
}

impl_binary_op_for_modint! {Add, add, AddAssign, add_assign, add}
impl_binary_op_for_modint! {Sub, sub, SubAssign, sub_assign, sub}
impl_binary_op_for_modint! {Mul, mul, MulAssign, mul_assign, mul}
impl_binary_op_for_modint! {Div, div, DivAssign, div_assign}

impl Div<ModInt> for ModInt {
    type Output = ModInt;

    fn div(self, other: Self) -> Self {
        self * other.pow(P - 2)
    }
}

#[cfg(test)]
mod tests {
    use super::*;

    #[test]
    fn test_ops() {
        let x = ModInt::from(P + 2);

        // `from` should normalize the value.
        assert_eq!(x.0, 2);

        // Operations.
        assert_eq!(x + 7, (2 + 7).into());
        assert_eq!(x - 7, (P + (2 - 7)).into());
        assert_eq!(x * 3, (2 * 3).into());
        assert_eq!((x / 11) * 11, 2.into());

        assert_eq!(x + ModInt::from(7), (2 + 7).into());
        assert_eq!(x - ModInt::from(7), (P + (2 - 7)).into());
        assert_eq!(x * ModInt::from(3), (2 * 3).into());
        assert_eq!((x / ModInt::from(11)) * 11, 2.into());

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

    #[test]
    fn test_fmt() {
        assert_eq!(format!("{:?}", ModInt::from(2)), "2");
        assert_eq!(format!("{}", ModInt::from(2)), "2");
    }
}
