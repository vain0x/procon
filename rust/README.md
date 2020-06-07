# Competitive Programming with Rust

## Install

Follow the instruction written in the official page <https://rust-lang.org/>.

### Install: (Optional) AtCoder-compatible Toolchain

For AtCoder, set the default toolchain to the supported version:

```sh
rustup install 1.42.0
rustup override set 1.42.0
```

## Usage

### Usage: Run

```sh
cargo run
```

### Usage: main.rs

Write your solution in `main.rs`.

There are some standard IO helpers. See the following parsing script and acceptable input.

```rust
pub fn main() {
    // Read a line to get a string. End of line is trimed.
    let line = rl();

    assert_eq!(&line, "#...#.#.#..#");

    // Parse a line as usize.
    let N = read!(usize);

    assert_eq!(N, 1_000);

    // Parse words in a line as i32 and collect them into a vec.
    let A = read![[usize]];

    assert_eq!(A, vec![2, 3, 5, 7]);

    // Parse constant-number words in a line as tuple.
    let (N, P) = read!(i64, f64);

    assert_eq!(N, 5_000_000_000_000_000);
    assert_eq!(P, 0.25);

    // Parse the specified number of lines as table of i32's.
    let board = read![[i32]; 2];
    assert_eq!(board, vec![vec![11, 12, 13], vec![21, 22, 23]]);

    // Parse the specified number of lines as list of tuples.
    let tuples = read![String, i32; 2];
    assert_eq!(tuples, vec![("a".to_string(), 1), ("b".to_string(), 2)]);
}
```

```
#...#.#.#..#
1000
2 3 5 7
5000000000000000 0.25
11 12 13
21 22 23
a 1
b 2
```
