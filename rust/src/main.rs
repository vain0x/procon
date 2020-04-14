//! Framework <https://github.com/vain0x/procon>

#![allow(non_snake_case)]
#![allow(unused_imports)]

use std::cmp::{max, min};
use std::collections::*;
use std::ops::*;

fn read_line(reader: &mut impl std::io::BufRead, buf: &mut String) {
    buf.clear();
    reader.read_line(buf).unwrap();
}

fn parse<T: std::str::FromStr>(word: &str) -> T {
    T::from_str(word).unwrap_or_else(|_| std::process::exit(66))
}

// ###############################################

fn main() {
    let stdin = std::io::stdin();
    let mut stdin = stdin.lock();
    let mut buf = String::new();

    read_line(&mut stdin, &mut buf);
    let mut words = buf.split_whitespace();
    let N: usize = parse(words.next().unwrap_or(""));

    println!("{}", N);
}
