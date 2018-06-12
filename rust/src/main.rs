#![allow(unused_imports)]
#![allow(non_snake_case)]

use std::cell::*;
use std::cmp::{max, min, Ordering};
use std::collections::*;
use std::io::*;
use std::marker::PhantomData;
use std::mem::*;
use std::ops::*;
use std::rc::*;
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

#[allow(unused)]
fn rw<T>() -> Vec<T>
where
    T: std::str::FromStr,
    T::Err: std::fmt::Debug,
{
    let mut buf = String::new();
    io::stdin().read_line(&mut buf).unwrap();
    buf.split_whitespace()
        .map(|word| T::from_str(word).unwrap())
        .collect()
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
                    &stringify!($arg).to_string() as &fmt::Debug,
                    &($arg) as &fmt::Debug,
                )),*
            ];
            eprintln!("{:?}", DebugMap(entries));
        }
    };
}

#[allow(unused)]
struct DebugMap<'a>(&'a [(&'a fmt::Debug, &'a fmt::Debug)]);

impl<'a> std::fmt::Debug for DebugMap<'a> {
    fn fmt(&self, fmt: &mut fmt::Formatter) -> fmt::Result {
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

pub trait FnMutRec<X, Y> {
    fn call(&mut self, x: X) -> Y;
}

pub struct ClosureFnMutRec<F>(F);

impl<X, Y, F> FnMutRec<X, Y> for ClosureFnMutRec<F>
where
    F: FnMut(&mut FnMut(X) -> Y, X) -> Y,
{
    fn call(&mut self, x: X) -> Y {
        // Duplicate mutable reference.
        let f = unsafe { &mut *(&mut self.0 as *mut F) };

        f(&mut |x: X| self.call(x), x)
    }
}

// FIXME: Replace the result type with impl FnMut in Rust 1.26+.
pub fn recursive<'a, X: 'a, Y: 'a, F>(f: F) -> Box<FnMut(X) -> Y + 'a>
where
    F: FnMut(&mut FnMut(X) -> Y, X) -> Y + 'a,
{
    let mut f = ClosureFnMutRec(f);
    Box::new(move |x: X| f.call(x))
}

pub fn recurse_with<X, Y, F>(x: X, f: F) -> Y
where
    F: FnMut(&mut FnMut(X) -> Y, X) -> Y,
{
    ClosureFnMutRec(f).call(x)
}

pub fn main() {
    let N = 7;
    let mut A = vec![vec![]; N];

    for &(u, v) in &[(1, 3), (3, 2), (3, 4), (5, 6)] {
        A[u].push(v);
        A[v].push(u);
    }

    let mut root = vec![None; N];

    for v in 0..N {
        recurse_with((v, v), |dfs, (v, r)| {
            if root[v].is_some() {
                return;
            }

            // It can borrow variables out of the closure.
            root[v] = Some(r);

            for &w in A[v].iter() {
                // Recursive call!
                dfs((w, r));
            }
        });

        /*
        let mut dfs = Y(
            |dfs: &mut FnMut((usize, usize)) -> (), (v, r): (usize, usize)| {
                if root[v].is_some() {
                    return;
                }

                // It can borrow variables out of the closure.
                root[v] = Some(r);

                for &w in A[v].iter() {
                    // Recursive call!
                    dfs((w, r));
                }
            },
        );
        dfs.apply((v, v));
        */
    }

    debug!(root);
    return;
}

#[cfg(test)]
mod tests {
    use super::*;

    #[test]
    fn test_ok() {
        assert_eq!(7, 1 + 2 * 3);
    }

    #[test]
    fn test_fact() {
        let f7 = recurse_with(7, |fact, n| if n == 0 { 1 } else { fact(n - 1) * n });
        assert_eq!(f7, 1 * 2 * 3 * 4 * 5 * 6 * 7);
    }

    #[test]
    fn test_memoized_fibonacci() {
        let mut memo = HashMap::new();
        let mut fib = {
            recursive(|fib, n: i32| {
                let e = memo.entry(n).or_insert_with(|| {
                    if n <= 1 {
                        1
                    } else {
                        fib(n - 1) + fib(n - 2)
                    }
                });
                *e
            })
        };
        assert_eq!(fib(0), 1);
        assert_eq!(fib(4), 5);
        assert_eq!(fib(5), 8);
        assert_eq!(fib(10), 89);
        assert_eq!(fib(20), 10946);
    }
}
