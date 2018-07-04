use std::fmt::Debug;
use std::io::*;
use std::str::{FromStr, SplitWhitespace};

#[macro_export]
macro_rules! read {
    ($r:expr; [$t:ty]; $n:expr) => {{
        let mut v = Vec::with_capacity($n);
        for _ in 0..$n {
            v.push(read!($r; [$t]));
        }
        v
    }};
    ($r:expr; $($t:ty),+; $n:expr) => {{
        let mut v = Vec::with_capacity($n);
        for _ in 0..$n {
            v.push(read!($r; $($t),+));
        }
        v
    }};
    ($r:expr; [$t:ty]) =>
        ($r.rwp::<$t>());
    ($r:expr; $t:ty) =>
        ($r.rlp::<$t>());
    ($r:expr; $($t:ty),*) => {{
        let mut w = $r.rwi();
        ($(w.next().unwrap().parse::<$t>().unwrap()),*)
    }};
}

pub struct Reader<R: Read> {
    line: String,
    inner_reader: BufReader<R>,
}

impl<R: Read> Reader<R> {
    pub fn run<T, F: FnOnce(Reader<StdinLock>) -> T>(f: F) -> T {
        let stdin = stdin();
        let r = Reader::new(stdin.lock());
        f(r)
    }

    pub fn new(reader: R) -> Self {
        Reader {
            line: String::new(),
            inner_reader: BufReader::new(reader),
        }
    }

    fn next(&mut self) -> &mut String {
        self.line.clear();
        self.inner_reader.read_line(&mut self.line).unwrap();
        &mut self.line
    }

    pub fn rls(&mut self) -> String {
        self.next().trim_right().to_owned()
    }

    pub fn rlp<T: FromStr>(&mut self) -> T
    where
        T::Err: Debug,
    {
        T::from_str(self.next().trim_right()).unwrap()
    }

    pub fn rwp<T: FromStr>(&mut self) -> Vec<T>
    where
        T::Err: Debug,
    {
        self.next()
            .trim_right()
            .split_whitespace()
            .map(|word| T::from_str(word).unwrap())
            .collect()
    }

    pub fn rwi(&mut self) -> SplitWhitespace {
        self.next().trim_right().split_whitespace()
    }
}

#[cfg(test)]
#[allow(unused_imports)]
#[allow(non_snake_case)]
mod tests {
    use super::*;

    #[test]
    fn test() {
        let source = r#"#...#.#.#..#
1000
2 3 5 7
5000000000000000 0.25
11 12 13
21 22 23
a 1
b 2
"#;

        let r = source.as_bytes();
        let mut r = Reader {
            line: String::new(),
            inner_reader: BufReader::new(r),
        };

        // Read a line to get a string. End of line is trimed.
        let line = r.rls();

        assert_eq!(&line, "#...#.#.#..#");

        // Parse a line as usize.
        let N = read!(r; usize);

        assert_eq!(N, 1_000);

        // Parse words in a line as i32 and collect them into a vec.
        let A = read![r; [usize]];

        assert_eq!(A, vec![2, 3, 5, 7]);

        // Parse constant-number words in a line as tuple.
        let (N, P) = read!(r; i64, f64);

        assert_eq!(N, 5_000_000_000_000_000);
        assert_eq!(P, 0.25);

        // Parse the specified number of lines as table of i32's.
        let board = read![r; [i32]; 2];
        assert_eq!(board, vec![vec![11, 12, 13], vec![21, 22, 23]]);

        // Parse the specified number of lines as list of tuples.
        let tuples = read![r; String, i32; 2];
        assert_eq!(tuples, vec![("a".to_string(), 1), ("b".to_string(), 2)]);
    }
}
