#![allow(unused_imports)]
#![allow(non_snake_case)]

use std::cell::*;
use std::cmp::{max, min, Ordering};
use std::collections::*;
use std::io::*;
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

fn fixpoint<'a, X, Y, F>(mut f: F) -> Box<FnMut(X) -> Y + 'a>
where
    X: 'a,
    Y: 'a,
    F: FnMut(&mut (FnMut(X) -> Y + 'a), X) -> Y + 'a,
{
    let rg: Rc<UnsafeCell<Option<*mut (FnMut(X) -> Y + 'a)>>> = Rc::new(UnsafeCell::new(None));

    let mut g: Box<FnMut(X) -> Y + 'a> = {
        let rg = rg.clone();
        let g = Box::new(move |x: X| {
            let ref_g = unsafe { &mut **(*rg.get()).as_mut().unwrap() };
            f(ref_g, x)
        });
        g
    };

    unsafe { *rg.get() = Some(g.as_mut()) };

    g
}

fn recurse<'a, X, Y, G>(x: X, g: G) -> Y
where
    X: 'a,
    Y: 'a,
    G: FnMut(&mut (FnMut(X) -> Y + 'a), X) -> Y + 'a,
{
    let mut f = fixpoint(g);
    f(x)
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
        recurse((v, v), |dfs, (v, r)| {
            if root[v].is_some() {
                return;
            }

            root[v] = Some(r);

            for &w in A[v].iter() {
                dfs((w, r));
            }
        });
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
}
