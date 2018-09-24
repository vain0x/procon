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
        let $v = <$t as Scan>::scan($ws.next().unwrap());
    };
    (@l; $v:ident : $t:ty [ ]) => {
        let stdin = std::io::stdin();
        let mut line = String::new();
        stdin.read_line(&mut line).unwrap();
        let mut ws = line.split_whitespace();
        let $v : Vec<$t> = ws.map(|x| <$t as Parse>::parse(x)).collect();
    };
    (@l; $($v:ident : $t:ty),+) => {
        let stdin = std::io::stdin();
        let mut line = String::new();
        stdin.read_line(&mut line).unwrap();
        let mut ws = line.split_whitespace();
        $(scan!{@w ws; $v : $t})*
    };
    ($($($v:ident : $t:ty),+);+ $(;)*) => {
        $(scan!{@l; $($v : $t),*})*
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

struct Scanner;

trait Scan {
    type Output;

    fn scan(s: &str) -> Self::Output;
}

macro_rules! impl_scan {
    ($($t:ty),*) => {
        $(impl Scan for $t {
            type Output = $t;

            fn scan(s: &str) -> Self::Output {
                s.parse::<Self::Output>().unwrap()
            }
        })*
    };
}

impl_scan!{char, String, usize, i32, i64, f64}

impl<T: Scan> Scan for Vec<T> {
    type Output = Vec<T::Output>;

    fn scan(source: &str) -> Self::Output {
        source
            .split_whitespace()
            .map(|w| <T as Scan>::scan(w))
            .collect()
    }
}

// -----------------------------------------------
// Solution
// -----------------------------------------------

fn main() {
    scan!{
        a: i32;
        b: i32, c: i32;
        s: String;
        z: i32[];
    }

    println!("{} {}", a + b + c, s);
    debug!(z);
}
