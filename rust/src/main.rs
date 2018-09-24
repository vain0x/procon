#![allow(unused_imports)]
#![allow(non_snake_case)]

use std::cell::RefCell;
use std::cmp::{max, min, Ordering};
use std::collections::*;
use std::fmt::{Debug, Formatter, Write as FmtWrite};
use std::io::{stderr, stdin, BufRead, Write};
use std::mem::{replace, swap};
use std::ops::*;
use std::rc::Rc;

// -----------------------------------------------
// Framework <https://github.com/vain0x/procon>
// -----------------------------------------------

#[cfg(debug_assertions)]
include!{"./procon/debug.rs"}

#[cfg(not(debug_assertions))]
macro_rules! debug {
    ($($arg:expr),*) => {};
}

#[allow(unused_macros)]
macro_rules! scan {
    (@w $ws:expr; $v:ident : $t:ty) => {
        let $v : $t = $ws.next().unwrap().parse().unwrap();
    };
    (@l; $($v:ident : $t:ty),+;) => {
        let stdin = std::io::stdin();
        let mut line = String::new();
        stdin.read_line(&mut line).unwrap();
        let mut ws = line.split_whitespace();
        $(scan!{@w ws; $v : $t})*
    };
    ($($($v:ident : $t:ty),+);+ $(;)*) => {
        $(scan!{@l; $($v : $t),*;})*
    };

    // ([$t:ty] ; $n:expr) =>
    //     ((0..$n).map(|_| read!([$t])).collect::<Vec<_>>());
    // ($($t:ty),+ ; $n:expr) =>
    //     ((0..$n).map(|_| read!($($t),+)).collect::<Vec<_>>());
    // ([$t:ty]) =>
    //     (rl().split_whitespace().map(|w| w.parse().unwrap()).collect::<Vec<$t>>());
    // ($t:ty) =>
    //     (rl().parse::<$t>().unwrap());
    // ($($t:ty),*) => {{
    //     let buf = rl();
    //     let mut w = buf.split_whitespace();
    //     ($(w.next().unwrap().parse::<$t>().unwrap()),*)
    // }};
}

#[allow(dead_code)]
fn rl() -> String {
    let mut buf = String::new();
    stdin().read_line(&mut buf).unwrap();
    buf.trim_right().to_owned()
}

trait IteratorExt: Iterator + Sized {
    fn vec(self) -> Vec<Self::Item> {
        self.collect()
    }
}

impl<T: Iterator> IteratorExt for T {}

// -----------------------------------------------
// Solution
// -----------------------------------------------

fn main() {
    scan!{
        a: i32;
        b: i32, c: i32;
        s: String;
    }

    println!("{} {}", a + b + c, s);
}
