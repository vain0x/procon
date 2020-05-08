/// Finds the lower bound of `y`, i.e. the end of `< y` half.
/// Given slice must be sorted in the ascending order.
pub fn lower_bound<T: Ord>(xs: &[T], y: &T) -> usize {
    let mut l = 0;
    let mut r = xs.len() + 1;

    while r - l > 1 {
        let m = l + (r - l) / 2;
        if &xs[m - 1] < y {
            l = m;
        } else {
            r = m;
        }
    }

    l
}

/// Finds the upper bound of `y`, i.e. the end of `<= y` half.
/// Given slice must be sorted in the ascending order.
pub fn upper_bound<T: Ord>(xs: &[T], y: &T) -> usize {
    let mut l = 0;
    let mut r = xs.len() + 1;

    while r - l > 1 {
        let m = l + (r - l) / 2;
        if &xs[m - 1] <= y {
            l = m;
        } else {
            r = m;
        }
    }

    l
}

#[cfg(test)]
mod tests {
    use super::{lower_bound, upper_bound};

    #[test]
    fn test_lb_ub() {
        let xs = vec![1, 2, 2, 4];
        let case = |x: i32, (xl, xr): (usize, usize)| {
            let l = lower_bound(&xs, &x);
            let r = upper_bound(&xs, &x);
            assert_eq!((x, l, r), (x, xl, xr));
        };

        case(0, (0, 0));
        case(1, (0, 1));
        case(2, (1, 3));
        case(3, (3, 3));
        case(4, (3, 4));
        case(5, (4, 4));
    }
}
