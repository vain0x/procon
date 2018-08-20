pub fn recurse<X, Y>(x: X, f: &Fn(X, &Fn(X) -> Y) -> Y) -> Y {
    f(x, &|x: X| recurse(x, &f))
}

pub unsafe fn recurse_mut<X, Y>(x: X, f: &mut FnMut(X, &mut FnMut(X) -> Y) -> Y) -> Y {
    use std::cell::UnsafeCell;
    use std::rc::Rc;

    let f1 = Rc::new(UnsafeCell::new(f));
    let f2 = f1.clone();
    (&mut *f1.get())(x, &mut |x: X| recurse_mut(x, &mut *f2.get()))
}

pub fn r2<F, G, H>(x: usize, mut f: F, mut g: G) -> usize
where
    F: FnMut(usize) -> Cont<H>,
    G: FnMut(usize, &mut FnMut(usize) -> usize) -> usize,
    H: FnMut(usize) -> usize,
{
    match f(x) {
        Cont::Ret(y) => y,
        Cont::Next(mut h) => {
            let fib = panic!();
            let result = g(x, fib);
            h(result)
        }
    }
}

enum Cont<H> {
    Next(H),
    Ret(usize),
}

fn f(n: usize) -> usize {
    let mut memo: Vec<Option<usize>> = vec![None; 30 + 1];
    r2(
        n,
        |n| {
            if n <= 1 {
                return Cont::Ret(1);
            }

            if let Some(result) = memo[n] {
                return Cont::Ret(result);
            }

            Cont::Next(|result| {
                memo[n] = Some(result);
                result
            })
        },
        |n, fib, cont| fib(n - 1, |x| fib(n - 2, |y| cont(x + y)),
    )
}

#[cfg(test)]
mod tests {
    use super::{recurse, recurse_mut};

    #[test]
    fn test_recurse_fib() {
        let fib_5 = recurse(5, &|n, fib| {
            if n <= 1 {
                1
            } else {
                fib(n - 1) + fib(n - 2)
            }
        });
        assert_eq!(fib_5, 8);
    }

    #[test]
    fn test_recurse_mut_memoized_fib() {
        const MAX: usize = 30;

        // let mut memo = vec![None; MAX + 1];
        {
            let mut fib = |n| super::f(n);

            assert_eq!(fib(5), 8);
            assert_eq!(fib(MAX), 1346269);
        }

        // assert!(memo[MAX].is_some());
    }
}
