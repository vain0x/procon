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
    (!! ($ws:expr);
        $([ $yv:ident : $yt:ty ]),*
        $(( $($xv:ident : $xt:ty),* )),*
    ) => {
        $(let $yv : Vec<$yt> =
            $ws.map(|w| w.parse().unwrap()).collect();)*
        $($(let $xv;)* {
            let mut ws = $ws;
            $($xv = ws.next().unwrap().parse::<$xt>().unwrap();)*
        })*
    };
    (!! ($ws:expr) { $zv:ident : $zn:expr };
        $([ $yv:ident : $yt:ty ]),*
        $(( $($xv:ident : $xt:ty),* )),*
    ) => {
        let mut $zv = vec![];
        for _ in 0..$zn {
            $(scan!(!! ($ws); [ $yv : $yt ]); $zv.push($yv);)*
            $(scan!(!! ($ws); ($($xv : $xt),*)); $zv.push(($($xv),*));)*
        }
    };
    ($(
        $([ $yv:ident : $yt:ty ]),*
        $(( $($xv:ident : $xt:ty),* )),*
        $({ $zv:ident : $zn:expr }),*
    ;)+) => {
        let stdin = std::io::stdin();
        let mut stdin = std::io::BufReader::new(stdin.lock());
        let mut line = String::new();
        $(scan!{
            !! ({
                line.clear();
                stdin.read_line(&mut line).unwrap();
                line.split_whitespace()
            }) $({ $zv : $zn })*;
            $([ $yv : $yt ]),*
            $(( $($xv : $xt),* )),*
        })*
    };
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

// a : i32
// n : usize, k : i64
// x : i64 []
// u : usize, v : usize [es ; m]
// x : i32 [] [board ; h]

fn main() {
    let z = vec![1];

    scan!{
        (a: usize);
        (b: usize, c: usize);
        (s: String);
        [z: i32];
        (u: usize, v: usize) {es: a};
        [b: i32] {B: a};
    }

    println!("{} {}", a + b + c, s);
    debug!(a, b, s, z, es, B);
}

/*

2
3 4
test
9 8 7
1 2
2 3
1 2 3 4 5 6 7 8
8 7 6 5 4 3 2 1
*/
