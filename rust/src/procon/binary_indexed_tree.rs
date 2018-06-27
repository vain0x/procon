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
