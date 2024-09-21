from lib.seg_tree import SegTree

def test_seg_tree():
    INF = 1e9 + 7
    src = [2, 2, 3, 6, 0, 6, 7, 9]
    st = SegTree(empty = INF, append = min, src = src)

    assert st.size == len(src)

    for l in range(st.size):
        for r in range(l + 1, st.size + 1):
            assert st.sum(l, r) == min(src[l:r])

    for x in [3, 1, 4, 1, 5]:
        src[x] = x
        st.set(x, x)
        assert st.sum(x, x + 1) == x

        l = x // 2
        r = min(x + 1, st.size)
        assert st.sum(l, r) == min(src[l:r])
