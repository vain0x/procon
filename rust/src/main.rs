#![allow(unused_imports)]
#![allow(non_snake_case)]

use procon::*;
use std::cmp::{max, min};
use std::collections::*;
use std::ops::*;
use std::*;

pub fn main() {
    return;
}

pub mod procon {
    use std;
    use std::cmp::*;
    use std::collections::*;
    use std::io;
    use std::mem;
    use std::str::FromStr;

    pub fn read_line() -> String {
        let mut buf = String::new();
        io::stdin().read_line(&mut buf).unwrap();
        buf.trim_right().to_owned()
    }

    pub fn read_words<T>() -> Vec<T>
    where
        T: std::str::FromStr,
        T::Err: std::fmt::Debug,
    {
        let mut buf = String::new();
        io::stdin().read_line(&mut buf).unwrap();
        buf.split_whitespace()
            .map(|word| T::from_str(word).unwrap())
            .collect()
    }

    // Polyfill
    #[derive(PartialEq, Eq, Clone, Debug)]
    pub struct Rev<T>(pub T);

    impl<T: PartialOrd> PartialOrd for Rev<T> {
        fn partial_cmp(&self, other: &Rev<T>) -> Option<Ordering> {
            other.0.partial_cmp(&self.0)
        }
    }

    impl<T: Ord> Ord for Rev<T> {
        fn cmp(&self, other: &Rev<T>) -> Ordering {
            other.0.cmp(&self.0)
        }
    }
}

#[cfg(test)]
mod tests {
    use super::*;

    #[test]
    pub fn test_ok() {
        assert_eq!(7, 1 + 2 * 3);
    }
}
