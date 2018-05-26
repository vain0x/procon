#![allow(unused_imports)]
#![allow(non_snake_case)]

use std::cmp::{max, min, Ordering};
use std::collections::*;
use std::io::*;
use std::ops::*;
use std::*;

#[allow(unused)]
macro_rules! o {
    ($self_:expr, $($arg:expr),*) => { $self_.write_fmt(format_args!($($arg),*)) };
}

#[allow(unused)]
macro_rules! ln {
    ($self_:expr, $($arg:expr),*) => { $self_.write_fmt_ln(format_args!($($arg),*)) };
}

impl<'a> Main<'a> {
    fn entry_point(&mut self) {
        return;
    }
}

#[cfg(test)]
mod tests {
    use super::*;

    #[test]
    fn test_ok() {
        assert_eq!(7, 1 + 2 * 3);
    }
}

// -----------------------------------------------
// Framework
// -----------------------------------------------

pub fn main() {
    let (stdin, stdout) = (io::stdin(), io::stdout());
    Main {
        stdin: io::BufReader::new(stdin.lock()),
        stdout: io::BufWriter::new(stdout.lock()),
    }.entry_point();
}

#[allow(unused)]
struct Main<'a> {
    stdin: io::BufReader<io::StdinLock<'a>>,
    stdout: io::BufWriter<io::StdoutLock<'a>>,
}

#[allow(unused)]
impl<'a> Main<'a> {
    /// Reads a line.
    fn rl(&mut self) -> String {
        let mut buf = String::new();
        self.stdin.read_line(&mut buf).unwrap();
        buf.trim_right().to_owned()
    }

    /// Reads a line, splits into words, parses each of them.
    fn rw<T>(&mut self) -> Vec<T>
    where
        T: str::FromStr,
        T::Err: fmt::Debug,
    {
        let mut buf = String::new();
        self.stdin.read_line(&mut buf).unwrap();
        buf.split_whitespace()
            .map(|word| T::from_str(word).unwrap())
            .collect()
    }

    fn write_fmt(&mut self, args: std::fmt::Arguments) {
        self.stdout.write_fmt(args).unwrap();
    }

    fn write_fmt_ln(&mut self, args: fmt::Arguments) {
        self.stdout.write_fmt(args).unwrap();
        self.stdout.write(b"\n").unwrap();
    }
}

// -----------------------------------------------
// Polyfill
// -----------------------------------------------

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
