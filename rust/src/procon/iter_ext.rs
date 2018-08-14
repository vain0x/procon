use std;

pub struct Pairwise<I, T> {
    iter: I,
    prev: Option<T>,
}

impl<I: Iterator> Iterator for Pairwise<I, I::Item>
where
    I::Item: Clone,
{
    type Item = (I::Item, I::Item);

    fn next(&mut self) -> Option<Self::Item> {
        if self.prev.is_none() {
            self.prev = self.iter.next();
            if self.prev.is_none() {
                return None;
            }
        }

        let next = self.iter.next();
        if next.is_none() {
            return None;
        }

        use std::mem::replace;
        let pair = (
            replace(&mut self.prev, next.clone()).unwrap(),
            next.unwrap(),
        );
        Some(pair)
    }
}

trait IteratorExt2: Iterator + Sized {
    fn pairwise(self) -> Pairwise<Self, Self::Item>
    where
        Self::Item: Clone,
    {
        Pairwise {
            iter: self,
            prev: None,
        }
    }

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

    #[test]
    fn test_pairwise() {
        assert_eq!(
            vec![(0, 1), (1, 2), (2, 3)],
            (0..4).pairwise().collect::<Vec<_>>()
        )
    }

    #[test]
    fn test_pairwise_empty() {
        assert_eq!(0, (0..0).pairwise().count());
        assert_eq!(0, (0..1).pairwise().count());
    }
}
