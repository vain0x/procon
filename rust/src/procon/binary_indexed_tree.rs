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
    while j < bit_len(bit) {
        bit[j] += value;
        j += rightmost_bit(j);
    }
}

/// Gets sum of items in range 0..right.
pub fn bit_acc(bit: &BIT, right: usize) -> i64 {
    let mut acc = 0;
    let mut j = right;
    while 0 < j && j < bit_len(bit) {
        acc += bit[j];
        j -= rightmost_bit(j);
    }
    acc
}

/// Gets sum of items in range left..right.
pub fn bit_sum(bit: &BIT, left: usize, right: usize) -> i64 {
    let mut acc = bit_acc(bit, right);
    acc -= bit_acc(bit, left);
    acc
}

fn rightmost_bit(n: usize) -> usize {
    let s = n as isize;
    (s & -s) as usize
}

#[cfg(test)]
#[allow(unused_imports)]
mod tests {
    use super::*;

    #[test]
    fn test() {
        let mut bit = bit_new(6);
        for (i, &x) in [3, 1, 4, 1, 5, 9].into_iter().enumerate() {
            bit_add(&mut bit, i, x);
        }

        assert_eq!(1 + 4 + 1, bit_sum(&bit, 1, 4));
    }
}
