# Procon / Rust

- Language: Rust
- Runtime: Native

## Install

Install rustup first.

For AtCoder, set the default toolchain to the supported version, [1.15.1](https://github.com/rust-lang/rust/blob/master/RELEASES.md#version-1151-2017-02-09):

```sh
rustup install 1.15.1
rustup override set 1.15.1
```

You could override the toolchain by adding `rust-toolchain` file, however, it seems Rust Language Server (which works only with `nightly` toolchain) doesn't work.

## Run

```sh
cargo run
```

## Usage

Write your solution in `main.rs`.

There are some standard IO helpers. For example, the following code:

```rust
pub fn main() {
    // Read a line to get a string. End of line is trimed.
    let line = rl();

    assert_eq!(&line, "#...#.#.#..#");

    // Read a line and parse the whole contents as a non-negative integer (usize).
    let N = rl().parse::<usize>().unwrap();

    assert_eq!(N, 1_000);

    // Read a line, split it by spaces and parse each word as an integer (32-bit signed).
    let A = rw::<i32>();

    assert_eq!(A, vec![2, 3, 5, 7]);

    // Read a line, split it by spaces and parse two of them as different types.
    let (N, P) = {
        let words = rw::<String>();
        let N = words[0].parse::<i64>().unwrap();
        let P = words[1].parse::<f64>().unwrap();
        (N, P)
    };

    assert_eq!(N, 5_000_000_000_000_000);
    assert_eq!(P, 0.25);
}
```

will parse the following input:

```
#...#.#.#..#
1000
2 3 5 7
5000000000000000 0.25
```
