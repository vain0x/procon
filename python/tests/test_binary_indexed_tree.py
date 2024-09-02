from lib.binary_indexed_tree import BinaryIndexedTree

def test_binary_indexed_tree():
    src = [3, 1, 4, 1, 5, 9, 2]
    changes = [1, 1, 2, 3, 5]

    bit = BinaryIndexedTree(len(src))
    for i in range(bit.size):
        bit.add(i, src[i])

    for r in range(bit.size + 1):
        assert bit.sum(0, r) == sum(src[:r])

    for x in changes:
        bit.add(x, x)
        src[x] += x
        print(f"{bit.sum(0, x + 1)} == {sum(src[:x + 1])}")
        assert bit.sum(0, x + 1) == sum(src[:x + 1])
