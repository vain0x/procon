# Rust

- Language: Rust

## Install

Install rustup first.

For AtCoder, set the default toolchain to the supported version, [1.15.1](https://github.com/rust-lang/rust/blob/master/RELEASES.md#version-1151-2017-02-09):

```sh
rustup install 1.15.1
rustup override set 1.15.1
```

Note that if `rust-toolchain` file exists, it seems Rust Language Server (which works only with `nightly` toolchain) doesn't work.

## Run

```sh
cargo run
```
