pub fn gcd(x: i64, y: i64) -> i64 {
    if y == 0 {
        x.abs()
    } else {
        gcd(y, x % y)
    }
}

/// Finds a solution of Bézout's identity ``s * x + t * y = d``
/// where `d` is the greatest common divisor of `x`, `y`.
pub fn gcd_ext(x: i64, y: i64) -> (i64, i64, i64) {
    if y == 0 {
        (x.abs(), x.signum(), 0)
    } else {
        let (d, s, t) = gcd_ext(y, x % y);
        (d, t, s - x / y * t)
    }
}

pub fn lcm(x: i64, y: i64) -> i64 {
    x / gcd(x, y) * y
}

#[cfg(test)]
mod tests {
    use super::{gcd, gcd_ext, lcm};

    #[test]
    fn test_gcd() {
        fn case(x: i64, y: i64, d: i64) {
            assert_eq!(gcd(x, y), d, "{:?}", (x, y, d));
            assert_eq!(gcd(y, x), d, "{:?}", (y, x, d));
        }

        case(1, 1, 1);
        case(3, 5, 1);
        case(2, 2, 2);
        case(4, 6, 2);
        case(18, 24, 6);
        case(-1, -1, 1);
        case(4, -6, 2);
    }

    #[test]
    fn test_gcd_ext() {
        fn case(x: i64, y: i64, (d, s, t): (i64, i64, i64)) {
            assert_eq!(gcd_ext(x, y), (d, s, t), "{:?}", (x, y));
            assert_eq!(s * x + t * y, d);
        }

        case(3, 7, (1, -2, 1));
        case(4, 6, (2, -1, 1));
        case(360 * 5 * 5 * 11, 360 * 7 * 7 * 13, (360, -227, 98));
        case(4, -6, (2, -1, -1));
        case(-9, -12, (3, 1, -1));
    }

    #[test]
    fn test_lcm() {
        assert_eq!(lcm(1, 1), 1);
        assert_eq!(lcm(12, 18), 36);
    }
}
