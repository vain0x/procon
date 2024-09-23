from lib.union_find import UnionFind

def test_union_find():
    uf = UnionFind(10)

    uf.merge(0, 1)
    assert uf.get(0) == uf.get(1)
    assert uf.get_size(0) == 2

    uf.merge(2, 3)
    assert uf.get(0) != uf.get(3)
    assert uf.get_size(3) == 2

    uf.merge(1, 2)
    assert uf.get(0) == uf.get(3)
    for i in range(0, 4):
        assert uf.get_size(i) == 4

    for i in range(0, 10):
        uf.merge(9, i)

    for i in range(0, 10 - 1):
        assert uf.get(i) == uf.get(i + 1)
    assert uf.get_size(0) == 10
