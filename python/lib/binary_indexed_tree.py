# use atcoder.fenwicktree.FenwickTree
class BinaryIndexedTree:
    # size: number of elements
    def __init__(self, size):
        self.size = size
        # (1-indexed)
        self.data = [0] * (size + 1)

    # do a[i] += value
    def add(self, i, value):
        assert 0 <= i and i < self.size
        i += 1
        while i <= self.size:
            self.data[i] += value
            i += i & (-i)

    # sum of range (a[l], ..., a[r - 1]) (r is exclusive)
    def sum(self, l, r):
        assert 0 <= l and l <= r and r <= self.size
        return self._prefix_sum(r) - self._prefix_sum(l)

    # sum of prefix range (a[0], ..., a[r - 1]) (r is exclusive)
    def _prefix_sum(self, r):
        i = r
        total = 0
        while i > 0:
            total += self.data[i]
            i -= i & (-i)
        return total
