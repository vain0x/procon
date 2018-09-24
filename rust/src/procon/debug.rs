mod debug_core {
    use std::fmt;
    use std::io::{self, Write};

    // ANSI escape codes.
    // These paint succeeding characters in terminal.
    const ESC_YELLOW: &'static str = "\x1B[33m";
    const ESC_GREEN: &'static str = "\x1B[92m";
    const ESC_RESET: &'static str = "\x1B[0m";

    pub fn debug_batch<F>(f: F)
    where
        F: FnOnce(&mut io::StderrLock),
    {
        let err = io::stderr();
        let mut err = err.lock();
        f(&mut err);
        err.flush().unwrap();
    }

    pub fn debug_write<W, T>(err: &mut W, expr: &str, value: &T)
    where
        W: io::Write,
        T: fmt::Debug,
    {
        // The expr is colored by yellow.
        // Rest stdout is colored by green for vis.
        writeln!(
            err,
            "{}{}{} = {:?}{}",
            ESC_YELLOW, expr, ESC_RESET, value, ESC_GREEN
        ).unwrap();
    }
}

#[macro_export]
macro_rules! debug {
    ($($arg:expr),*) => {
        #[cfg(debug_assertions)]
        {
            use self::debug_core::*;
            debug_batch(|err| {
                $(debug_write(err, stringify!($arg), &$arg);)*
            })
        }
    };
}

#[allow(dead_code)]
fn debug_core_false_use_site() {
    // This code will report errors in body of debug!.
    debug!((), ())
}
