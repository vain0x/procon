# use atcoder.segtree.SegTree
class SegTree:
    # src: array of initial values
    def __init__(self, append, empty, src):
        # ei: index of elements
        # ni: index within node

        size = len(src)
        node = [empty] * (size * 2)

        for ei in range(size):
            node[size + ei] = src[ei]

        for ni in range(size - 1, 0, -1):
            node[ni] = append(node[ni * 2], node[ni * 2 + 1])

        self.node = node
        self.size = size
        self.empty = empty
        self.append = append

    # do a[ei] = value
    def set(self, ei, value):
        assert 0 <= ei and ei < self.size

        node = self.node
        append = self.append

        ni = self.size + ei
        node[ni] = value

        while ni > 1:
            ni //= 2
            node[ni] = append(node[ni * 2], node[ni * 2 + 1])

    # sum of range (a[ql], ..., a[qr - 1]) (qr is exclusive)
    def sum(self, ql, qr):
        assert 0 <= ql and ql <= qr and qr <= self.size

        if ql == 0 and qr == self.size:
            return self.node[1]

        node = self.node
        append = self.append

        acc_l = self.empty
        acc_r = self.empty
        nl = self.size + ql
        nr = self.size + qr

        while nl < nr:
            if nl & 1:
                acc_l = append(acc_l, node[nl])
                nl += 1

            if nr & 1:
                nr -= 1
                acc_r = append(node[nr], acc_r)

            nl >>= 1
            nr >>= 1

        return append(acc_l, acc_r)
