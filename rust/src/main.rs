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

#[allow(unused)]
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

#[allow(unused)]
macro_rules! debug {
    ($($arg:expr),*) => {
        #[cfg(debug_assertions)]
        {
            let entries = &[
                $((
                    &stringify!($arg) as &Debug,
                    &($arg) as &Debug,
                )),*
            ];
            eprintln!("{:?}", DebugMap(entries));
        }
    };
}

#[allow(unused)]
struct DebugMap<'a>(&'a [(&'a Debug, &'a Debug)]);

impl<'a> Debug for DebugMap<'a> {
    fn fmt(&self, fmt: &mut Formatter) -> fmt::Result {
        let mut m = fmt.debug_map();
        for &(key, value) in self.0.iter() {
            m.entry(key, value);
        }
        m.finish()
    }
}

// -----------------------------------------------
// Polyfill
// -----------------------------------------------

#[allow(unused)]
macro_rules! eprintln {
    ($($arg:expr),*) => { _eprintln(format_args!($($arg),*)) }
}

fn _eprintln(args: fmt::Arguments) {
    let err = std::io::stderr();
    let mut err = err.lock();
    err.write_fmt(args).unwrap();
    err.write(b"\n").unwrap();
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
