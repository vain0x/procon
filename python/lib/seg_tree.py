class SegTree:
    # src: array of initial values
    def __init__(self, src, empty, append):
        # w = smallest 2^n (>= size)
        # ei: index of elements
        # ni: index within node

        size = len(src)

        w = 1
        while w < size:
            w *= 2

        node = [empty] * (w * 2 - 1)

        for ei in range(size):
            node[w - 1 + ei] = src[ei]

        for ni in range(w - 2, -1, -1):
            node[ni] = append(node[ni * 2 + 1], node[ni * 2 + 2])

        self.node = node
        self.size = size
        self.width = w
        self.empty = empty
        self.append = append

    # do a[ei] = value
    def set(self, ei, value):
        assert 0 <= ei and ei < self.size

        ni = self.width - 1 + ei
        self.node[ni] = value

        while ni > 0:
            ni = (ni - 1) // 2
            self.node[ni] = (self.append)(self.node[ni * 2 + 1], self.node[ni * 2 + 2])

    # sum of range (a[ql], ..., a[qr - 1]) (qr is exclusive)
    def sum(self, ql, qr):
        assert 0 <= ql and ql <= qr and qr <= self.size

        # disjoint (ql, qr) & (0, size)
        if qr <= 0 or self.size <= ql:
            return self.empty

        return self._sum(0, 0, self.width, ql ,qr)

    # sum of range (a[l], ..., a[r - 1])
    #   where (l, r) = intersection of (el, er) (ql, qr)
    #         ni: index of node that holds sum of (a[el], ..., a[er - 1])
    def _sum(self, ni, el, er, ql, qr):
        if qr <= el or er <= ql:
            # disjoint (ql, qr) & (el, er)
            return self.empty

        if ql <= el and er <= qr:
            # covered (el, er) <= (ql, qr)
            return self.node[ni]

        mid = el + (er - el) // 2
        acc_l = self._sum(ni * 2 + 1, el, mid, ql, qr)
        acc_r = self._sum(ni * 2 + 2, mid, er, ql, qr)
        return (self.append)(acc_l, acc_r)
