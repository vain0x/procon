#![allow(unused_imports)]
#![allow(non_snake_case)]

use std::cmp::{max, min, Ordering};
use std::collections::*;
use std::fmt::{Debug, Formatter};
use std::io::*;
use std::ops::*;
use std::*;

// -----------------------------------------------
// Framework
// -----------------------------------------------

#[allow(dead_code)]
fn rl() -> String {
    let mut buf = String::new();
    io::stdin().read_line(&mut buf).unwrap();
    buf.trim_right().to_owned()
}

#[allow(unused_macros)]
macro_rules! read {
    ([$t:ty] ; $n:expr) =>
        ((0..$n).map(|_| read!([$t])).collect::<Vec<_>>());
    ($($t:ty),+ ; $n:expr) =>
        ((0..$n).map(|_| read!($($t),+)).collect::<Vec<_>>());
    ([$t:ty]) =>
        (rl().split_whitespace().map(|w| w.parse().unwrap()).collect::<Vec<$t>>());
    ($t:ty) =>
        (rl().parse::<$t>().unwrap());
    ($($t:ty),*) => {{
        let buf = rl();
        let mut w = buf.split_whitespace();
        ($(w.next().unwrap().parse::<$t>().unwrap()),*)
    }};
}

trait IteratorExt: Iterator + Sized {
    fn vec(self) -> Vec<Self::Item> {
        self.collect()
    }
}

impl<T: Iterator> IteratorExt for T {}

#[allow(unused_macros)]
macro_rules! debug {
    ($($arg:expr),*) => {
        #[cfg(debug_assertions)]
        {
            let es = &[$((&stringify!($arg) as &Debug, &$arg as &Debug)),*];
            let mut err = std::io::stderr();
            err.write_fmt(format_args!("{:?}\n", MyDebugMap(es))).unwrap();
        }
    };
}

#[allow(dead_code)]
struct MyDebugMap<'a>(&'a [(&'a Debug, &'a Debug)]);

impl<'a> Debug for MyDebugMap<'a> {
    fn fmt(&self, fmt: &mut Formatter) -> fmt::Result {
        let mut m = fmt.debug_map();
        for &(key, value) in self.0.iter() {
            m.entry(key, value);
        }
        m.finish()
    }
}

// -----------------------------------------------
// Solution
// -----------------------------------------------

pub fn main() {
    return;
}

#[cfg(test)]
mod tests {
    use super::*;

    #[test]
    fn test_ok() {
        assert_eq!(7, 1 + 2 * 3);
    }
}
