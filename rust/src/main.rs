//! Framework <https://github.com/vain0x/procon>

#![allow(non_snake_case)]
#![allow(unused_imports)]

use std::collections::*;
use std::ops::*;

struct StdinLines<R>(String, R);

impl<R: std::io::BufRead> Iterator for StdinLines<R> {
    type Item = std::str::SplitWhitespace<'static>;

    fn next(&mut self) -> Option<std::str::SplitWhitespace<'static>> {
        self.0.clear();
        std::io::BufRead::read_line(&mut self.1, &mut self.0).unwrap();
        Some(Box::leak(self.0.clone().into_boxed_str()).split_whitespace())
    }
}

pub struct Scan<'a>(std::iter::Flatten<StdinLines<std::io::StdinLock<'a>>>);

impl<'a> Scan<'a> {
    fn new(stdin: std::io::StdinLock<'a>) -> Self {
        Scan(StdinLines(String::new(), stdin).flatten())
    }

    pub fn word<T: std::str::FromStr>(&mut self) -> T {
        self.0.next().unwrap().parse().ok().unwrap()
    }

    pub fn list<T: std::str::FromStr>(&mut self, len: usize) -> Vec<T> {
        (0..len).map(|_| self.word()).collect()
    }
}

fn main() {
    let stdin = std::io::stdin();
    let mut scan = Scan::new(stdin.lock());

    let N = scan.word::<usize>();

    println!("{}", N);
}
