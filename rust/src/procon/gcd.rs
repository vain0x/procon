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

/// Solves the following simultaneous equations:
///     x ≡ a1 (mod n1),
///     x ≡ a2 (mod n2)
/// to get `(x, m)`
/// where m = `lcm(n1, n2)`
/// and x is the unique solution in range `0 <= x < m`.
/// Based on Chinese Reminder Theorem.
/// To reduce, use `Some((0, 1))` as unit.
pub fn chinese_reminder(a1: i64, n1: i64, a2: i64, n2: i64) -> Option<(i64, i64)> {
    let (d, x, _) = gcd_ext(n1, n2);
    if (a2 - a1).abs() % d != 0 {
        return None;
    }

    let m = n1 * (n2 / d);
    let t = (a2 - a1) / d * x % (n2 / d);
    let u = ((a1 + n1 * t) % m + m) % m;
    Some((u, m))
}

#[cfg(test)]
mod tests {
    use super::{chinese_reminder, gcd, gcd_ext, lcm};

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

    #[test]
    fn test_chinese_reminder() {
        assert_eq!(
            vec![(2, 3), (3, 5), (2, 7)]
                .into_iter()
                .fold(Some((0, 1)), |acc, (y, m)| acc
                    .and_then(|(x, n)| chinese_reminder(x, n, y, m))),
            Some((23, 105))
        );
    }
}
