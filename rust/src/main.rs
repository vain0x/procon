//! Framework <https://github.com/vain0x/procon>

#![allow(non_snake_case)]
#![allow(unused_imports)]

use std::collections::*;
use std::ops::*;

fn main() {
    let stdin = std::io::stdin();
    let mut stdin = stdin.lock();
    let mut buf = String::new();
    let mut words = std::iter::repeat_with(|| {
        buf.clear();
        std::io::BufRead::read_line(&mut stdin, &mut buf).unwrap();
        Box::leak(buf.clone().into_boxed_str()).split_whitespace()
    })
    .flatten();

    let N = words.next().unwrap().parse::<usize>().unwrap();

    println!("{}", N);
}
