pub fn recurse<X, Y>(x: X, f: &Fn(X, &Fn(X) -> Y) -> Y) -> Y {
    f(x, &|x: X| recurse(x, &f))
}
