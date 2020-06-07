//! Framework <https://github.com/vain0x/procon>

#![allow(non_snake_case)]
#![allow(unused_imports)]

use std::cmp::{max, min};
use std::collections::*;
use std::ops::*;

fn main() {
    let stdin = std::io::stdin();
    let mut stdin = stdin.lock();
    let mut buf = String::new();

    let mut words = {
        buf.clear();
        std::io::BufRead::read_line(&mut stdin, &mut buf).unwrap();
        buf.split_whitespace()
    };
    let N: usize = words.next().unwrap().parse().unwrap();

    println!("{}", N);
}
