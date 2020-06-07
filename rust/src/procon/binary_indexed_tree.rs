// Binary Indexed Tree (BIT) is an array structure
// implemented as a tree that looks like:
//
//      [                       8                     ] _ ..
//      [          4          ] _______________________ [ ..
//      [    2    ] ___________ [    6    ] ___________ [ ..
//      [ 1 ] _____ [ 3 ] _____ [ 5 ] _____ [ 7 ] _____ [ ..
//                                                        ..
//      < 0 > < 1 > < 2 > < 3 > < 4 > < 5 > < 6 > < 7 > < ..
//
// where each ``[ i ]`` is i-th node
// and each ``< i >`` is i-th element of the structure.

// Invariant: each node holds the sum of values of elements that covers. e.g.
//      [1] = <0>,
//      [6] = <4> + <5>,
//      [2^N] = <0> + .. + <2^N - 1>.

// Technique: for i (> 1), node [i]'s parent is [i + (i & -i)].

use std;

// 1-indexed. [0] is unused.
type BIT = Vec<i64>;

pub fn bit_new(len: usize) -> BIT {
    vec![0; len + 1]
}

pub fn bit_len(bit: &BIT) -> usize {
    bit.len() - 1
}

/// Increments an element.
pub fn bit_add(bit: &mut BIT, index: usize, value: i64) {
    let mut j = index + 1;
    while j <= bit_len(bit) {
        bit[j] += value;
        j += rightmost_bit(j);
    }
}

/// Gets sum of items in range 0..right.
pub fn bit_acc(bit: &BIT, right: usize) -> i64 {
    let mut acc = 0;
    let mut j = std::cmp::min(right, bit_len(bit));
    while j > 0 {
        acc += bit[j];
        j -= rightmost_bit(j);
    }
    acc
}

/// Gets sum of items in range left..right.
pub fn bit_sum(bit: &BIT, left: usize, right: usize) -> i64 {
    bit_acc(bit, right) - bit_acc(bit, std::cmp::min(left, right))
}

fn rightmost_bit(n: usize) -> usize {
    let s = n as isize;
    (s & -s) as usize
}

#[cfg(test)]
#[allow(unused_imports)]
mod tests {
    use super::*;
    use std::cmp::min;

    #[test]
    fn test() {
        let a = vec![3, 1, 4, 1, 5, 9];
        let n = a.len();

        let mut bit = bit_new(n);
        for (i, &x) in a.iter().enumerate() {
            bit_add(&mut bit, i, x);
        }

        assert_eq!(3 + 1 + 4, bit_acc(&bit, 3));
        assert_eq!(3 + 1 + 4 + 1 + 5 + 9, bit_acc(&bit, 6));

        assert_eq!(1 + 4 + 1, bit_sum(&bit, 1, 4));

        for l in 0..n {
            for r in 0..n {
                let actual = a[min(l, r)..r].iter().sum::<i64>();
                let expected = bit_sum(&bit, l, r);
                assert_eq!(actual, expected, "l={} r={}", l, r);
            }
        }
    }
}
