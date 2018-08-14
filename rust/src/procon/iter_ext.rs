use std;

trait IteratorExt2: Iterator + Sized {
    fn join(self, separator: &str) -> String
    where
        Self::Item: std::fmt::Display,
    {
        self.map(|x| format!("{}", x))
            .collect::<Vec<_>>()
            .join(separator)
    }
}

impl<T: Iterator> IteratorExt2 for T {}

#[cfg(test)]
mod tests {
    use super::IteratorExt2;

    #[test]
    fn test_join_empty() {
        assert_eq!("", Vec::<i64>::new().into_iter().join(" "));
    }

    #[test]
    fn test_join() {
        assert_eq!("0 1 2 3", (0..4).join(" "))
    }
}
