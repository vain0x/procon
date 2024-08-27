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

    fn sort_by_key<K, F>(self, f: F) -> std::vec::IntoIter<Self::Item>
    where
        K: Ord,
        F: Fn(&Self::Item) -> K,
    {
        let mut v = self.collect::<Vec<_>>();
        v.sort_by_key(f);
        v.into_iter()
    }

    fn group_by<K, V, KF, VF>(
        self,
        mut key_fn: KF,
        mut value_fn: VF,
    ) -> std::vec::IntoIter<(K, Vec<V>)>
    where
        K: Clone + Eq + std::hash::Hash,
        KF: FnMut(&Self::Item) -> K,
        VF: FnMut(Self::Item) -> V,
    {
        use std::collections::HashMap;

        let mut groups = Vec::<(K, Vec<V>)>::new();
        let mut group_indices = HashMap::<K, usize>::new();

        for item in self {
            let key = key_fn(&item);
            let value = value_fn(item);

            match group_indices.get(&key) {
                Some(&index) => {
                    let (_, ref mut items) = groups[index];
                    items.push(value);
                }
                None => {
                    let index = groups.len();
                    groups.push((key.clone(), vec![value]));
                    group_indices.insert(key, index);
                }
            }
        }

        groups.into_iter()
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

    #[test]
    fn test_group_by() {
        let kvs = vec![
            ("Japan", "Tokyo"),
            ("Japan", "Osaka"),
            ("USA", "New York"),
            ("China", "Shanghai"),
            ("USA", "Los Angeles"),
        ];
        let expected = vec![
            ("Japan", vec!["Tokyo", "Osaka"]),
            ("USA", vec!["New York", "Los Angeles"]),
            ("China", vec!["Shanghai"]),
        ];

        let actual = kvs
            .into_iter()
            .group_by(|&(country, _)| country, |(_, city)| city);

        for ((ak, av), (xk, xv)) in actual.into_iter().zip(expected) {
            assert_eq!(xk, ak);
            for (a, x) in av.into_iter().zip(xv) {
                assert_eq!(a, x);
            }
        }
    }
}
