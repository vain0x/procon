from random import Random
from lib.treap import Treap

def test_treap_basic():
    rng = Random()
    tr = Treap(rng = rng)

    t = tr.EMPTY
    t = tr.insert(t, 0, 31); _verify_invariants(tr, t)
    t = tr.insert(t, 1, 14); _verify_invariants(tr, t)
    t = tr.insert(t, 2, 15); _verify_invariants(tr, t)

    assert tr.to_list(t) == [31, 14, 15]
    assert tr.get(t, 0) == 31
    assert tr.get(t, 1) == 14
    assert tr.get(t, 2) == 15

def test_treap_sorted():
    rng = Random()
    tr = Treap(rng)

    # manual
    t = 0
    t = tr.add_sorted(t, 3)
    t = tr.add_sorted(t, 1)
    assert tr.to_list(t) == [1, 3]
    t = tr.add_sorted(t, 4)
    assert tr.to_list(t) == [1, 3, 4]
    t = tr.add_sorted(t, 1)
    assert tr.to_list(t) == [1, 1, 3, 4]

    # random
    N = 100
    for _ in range(8):
        t = 0
        expected = []

        for _ in range(3):
            for _ in range(N // 3):
                x = rng.randint(1, 30)
                t = tr.add_sorted(t, x)
                expected.append(x)
                expected.sort()
                assert tr.to_list(t) == expected

            for _ in range(3):
                i = rng.randint(0, len(expected) - 1)
                x = expected[i]
                _, t = tr.remove_sorted(t, x)
                del expected[i]
                assert tr.to_list(t) == expected

def test_treap_random():
    rng = Random()
    tr = Treap(rng)
    N = 100
    for _ in range(8):
        values = [i + 10 for i in range(N)]
        rng.shuffle(values)

        t = tr.EMPTY
        expected = []

        for i in range(len(values)):
            if i >= 1:
                k = rng.randint(0, i - 1)
            else:
                k = 0

            t = tr.insert(t, k, values[i])
            expected.insert(k, values[i])

            assert tr.size[t] == i + 1
            assert tr.to_list(t) == expected
            _verify_invariants(tr, t)

        # remove
        for _ in range(3):
            k = rng.randint(0, len(expected) - 1)
            t = tr.del_at(t, k)
            del expected[k]
            assert tr.to_list(t) == expected
            _verify_invariants(tr, t)

def _verify_invariants(tr: Treap, t: int):
    assert 0 <= t < len(tr.data)

    def dfs(t: int):
        if t == tr.EMPTY:
            return 0

        for i in range(2):
            c = tr.ch[t][i]
            if c == 0: continue
            assert tr.pri[t] > tr.pri[c]

        size = 0
        for i in range(2):
            size += dfs(tr.ch[t][i])

        size += 1
        assert tr.size[t] == size
        return size

    dfs(t)
