class UnionFind:
    def __init__(self, size):
        self._len = size
        self.parent = [i for i in range(size)]
        self.u_size = [1] * size

    def __len__(self):
        return self._len

    def get(self, u):
        assert 0 <= u < self._len

        v = self.parent[u]
        if self.parent[v] == v: # v is root
            return v

        path = [u]
        while self.parent[v] != v:
            path.append(v)
            v = self.parent[v]

        # path compression
        for t in path:
            self.parent[t] = v
            self.u_size[t] = self.u_size[v]

        return v

    def __getitem__(self, i):
        return self.get(i)

    def get_size(self, u):
        return self.u_size[self.get(u)]

    def merge(self, u, v):
        assert 0 <= u < self._len
        assert 0 <= v < self._len

        if u == v: return
        u = self.get(u)
        v = self.get(v)
        if u == v: return

        if self.u_size[u] < self.u_size[v]:
            u, v = v, u

        self.parent[v] = u
        self.u_size[u] += self.u_size[v]
