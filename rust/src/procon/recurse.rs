pub fn recurse<X, Y>(x: X, f: &Fn(X, &Fn(X) -> Y) -> Y) -> Y {
    f(x, &|x: X| recurse(x, &f))
}

#[cfg(test)]
mod tests {
    use super::recurse;

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
}
