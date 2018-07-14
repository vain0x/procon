#[macro_export]
macro_rules! debug {
    ($($arg:expr),*) => {
        #[cfg(debug_assertions)]
        {
            use std::io::*;
            $(writeln!(stderr(), "{} = {:?}", stringify!($arg), $arg).unwrap();)*
        }
    };
}
