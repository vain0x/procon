# Rearranges the list to the next lexicographical order.
# Returns True if ok, False if not (i.e. xs is max.)
def next_permutation(xs):
    n = len(xs)

    # Find the suffix
    #   (min j such that xs[j] < xs[j+1] >= ... >= x[n-1])
    # e.g. 52431  (j = 1)
    #       ^~~~  (2 < 4 >= 3 >= 1)
    j = -1
    for i in range(n - 2, 0, -1):
        if xs[i] < xs[i + 1]:
            j = i
            break
    if j < 0:
        return False

    # Find the next greater element
    #   (xs[k] > xs[i] for all i in the suffix)
    # e.g. 52431
    #       ^ ^
    #       j k
    k = -1
    for i in range(n - 1, j, -1):
        if xs[j] < xs[i]:
            k = i
            break

    assert k >= 0

    # e.g. 52431 -> 53421
    #       ^ ^      ^ ^
    xs[j], xs[k] = xs[k], xs[j]

    # Reverse the descending suffix
    #   xs[j + 1:].reverse()
    # e.g. 53421 -> 53124
    #        ^j+1     ~~~ ascending
    for i in range((n - (j + 1)) // 2):
        u = j + 1 + i
        v = n - i - 1
        xs[u], xs[v] = xs[v], xs[u]
        i += 1

    return True
