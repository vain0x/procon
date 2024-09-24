from math import inf
from random import Random
from typing import Generic, Iterable, Self, TypeVar

T = TypeVar("T")

class Treap(Generic[T]):
    # Node 0 is always empty
    EMPTY = 0

    def __init__(self, rng: Random | None = None) -> None:
        self.rng = rng if rng is not None else Random()

        self.data = [None] # node values ([0] is invalid)
        self.pri = [-inf] # node priorities
        self.ch = [(0, 0)] # children of node. ([0] is tuple, others list)
        self.size = [0] # node size

    def _create_node(self, value: T, priority: float | None = None) -> int:
        t = len(self.data)
        self.data.append(value)
        self.pri.append(priority)
        self.ch.append([0, 0])
        self.size.append(1)
        return t

    def merge(self, l: int, r: int) -> int:
        "Merge two trees"

        if l == 0 or r == 0:
            return l if r == 0 else r

        assert 0 < l < len(self.data)
        assert 0 < r < len(self.data)

        if self.pri[l] > self.pri[r]:
            self.ch[l][1] = self.merge(self.ch[l][1], r)
            return self._update(l)
        else:
            self.ch[r][0] = self.merge(l, self.ch[r][0])
            return self._update(r)

    def split(self, t: int, k: int) -> tuple[int, int]:
        "Split node t at index k to two trees"
        # split t => (l, r)
        #   t: [0, n) => l: [0, k), r: [k, n)

        assert 0 <= t < len(self.data)
        assert 0 <= k <= self.size[t]

        if t == 0:
            return 0, 0

        l_size = self.size[self.ch[t][0]]
        if k <= l_size:
            sl, sr = self.split(self.ch[t][0], k)
            self.ch[t][0] = sr
            t = self._update(t)
            return sl, t
        else:
            k -= l_size + 1
            sl, sr = self.split(self.ch[t][1], k)
            self.ch[t][1] = sl
            t = self._update(t)
            return t, sr

    def insert(self, t: int, k: int, value: T, priority: float | None = None) -> int:
        "Create new node and insert to node t at index k"

        assert 0 <= t < len(self.data)
        assert 0 <= k <= self.size[t]

        if priority is None:
            priority = self.rng.random()

        if t != 0 and priority < self.pri[t]:
            l_size = self.size[self.ch[t][0]]
            if k <= l_size:
                self.ch[t][0] = self.insert(self.ch[t][0], k, value, priority)
                return self._update(t)
            else:
                k -= l_size + 1
                self.ch[t][1] = self.insert(self.ch[t][1], k, value, priority)
                return self._update(t)

        u = self._create_node(value, priority)

        if t == 0:
            return u
        else:
            sl, sr = self.split(t, k)
            self.ch[u][0] = sl
            self.ch[u][1] = sr
            return self._update(u)

    def del_at(self, t: int, k: int) -> int:
        "Delete from node t at index k"
        assert 0 < t < len(self.data)
        assert 0 <= k < self.size[t]

        sl, sr = self.split(t, k)
        _, srr = self.split(sr, 1)
        return self.merge(sl, srr)

    def _update(self, t: int):
        "Called after children of node t changed"

        assert 0 <= t < len(self.data)
        l, r = self.ch[t]
        self.size[t] = self.size[l] + self.size[r] + 1
        return t

    def get_node(self, t: int, k: int) -> int:
        "Get k'th node in node t"
        assert 0 < t < len(self.data)
        assert 0 <= k < self.size[t]

        while True:
            l_size = self.size[self.ch[t][0]]
            if l_size == k:
                return t
            elif k < l_size:
                t = self.ch[t][0]
            else:
                k -= l_size + 1
                t = self.ch[t][1]

    def get(self, t: int, k: int) -> T:
        "Get k'th item in node t"
        return self.data[self.get_node(t, k)]

    def bisect_left(self, t: int, value: T) -> int:
        "Find lower bound of value in node t (if sorted)"
        k = 0
        while t != 0:
            assert 0 <= t < len(self.data)
            if not (self.data[t] < value):
                t = self.ch[t][0]
            else:
                k += self.size[self.ch[t][0]] + 1
                t = self.ch[t][1]
        return k

    def bisect_right(self, t: int, value: T) -> int:
        "Find upper bound of value in node t (if sorted)"
        k = 0
        while t != 0:
            assert 0 <= t < len(self.data)
            if value < self.data[t]:
                t = self.ch[t][0]
            else:
                k += self.size[self.ch[t][0]] + 1
                t = self.ch[t][1]
        return k

    def add_sorted(self, t: int, value: T, priority: float | None = None) -> int:
        k = self.bisect_right(t, value)
        return self.insert(t, k, value, priority)

    def remove_sorted(self, t: int, value: T) -> tuple[bool, int]:
        k = self.bisect_right(t, value)
        if k >= 1 and self.data[self.get_node(t, k - 1)] == value:
            return True, self.del_at(t, k - 1)
        else:
            return False, t

    # traversal

    def to_list(self, t: int) -> list[T]:
        assert 0 <= t < len(self.data)
        output = []
        def dfs(t: int):
            if t != 0:
                dfs(self.ch[t][0])
                output.append(self.data[t])
                dfs(self.ch[t][1])
        dfs(t)
        return output

    def create(self, iter: Iterable[T]):
        t = 0
        for x in iter:
            t = self.insert(t, x)
        return TreapNode(self, t)



class TreapNode(Generic[T]):
    def __init__(self, tr: Treap[T], t: int) -> None:
        self.owner = tr
        self.t = t

    def with_node(self, t: int):
        return TreapNode(self.owner, t)

    def __len__(self) -> int:
        return self.owner.size[self.t]

    def __getitem__(self, k: int | slice) -> T:
        if isinstance(k, slice): raise NotImplementedError()
        if not(0 <= k < len(self)): raise IndexError()
        s = self.owner.get_node(self.t, k)
        return self.owner.data[s]

    def __setitem__(self, k: int | slice, value: T):
        if isinstance(k, slice): raise NotImplementedError()
        if not(0 <= k < len(self)): raise IndexError()
        s = self.owner.get_node(self.t, k)
        self.owner.data[s] = value

    def __delitem__(self, k: int | slice):
        if isinstance(k, slice()): raise NotImplementedError()
        if not(0 <= k < len(self)): raise IndexError()
        self.t = self.owner.del_at(self.t, k)

    def insert(self, k: int, value: T):
        self.t = self.owner.insert(self.t, k, value)

    def merge(self, other: Self):
        assert self.owner == other.owner
        assert self.t != other.t
        self.t = self.owner.merge(self.t, other.t)

    def split(self, k: int):
        sl, sr = self.owner.split(self.t, k)
        return TreapNode(self.owner, sl), TreapNode(self.owner, sr)



class SortedList(Generic[T]):
    def __init__(self, init: list[T], rng: Random | None = None):
        tr = Treap(rng)
        t = 0
        for x in init:
            t = tr.add_sorted(t, x)

        self.tr = tr
        self.t = t

    def __contains__(self, value: T) -> bool:
        k = self.tr.bisect_left(self.t, value)
        if k >= self.tr.size[self.t]:
            return False

        s = self.tr.get_node(self.t, k)
        return self.tr.data[s] == value

    def add(self, value: T):
        self.t = self.tr.add_sorted(self.t, value)

    def remove(self, value: T) -> bool:
        removed, t = self.tr.remove_sorted(self.t, value)
        self.t = t
        return removed
