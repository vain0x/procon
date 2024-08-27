def prefix_sum(src, key = None) -> list[int]:
    src_len = len(src)
    sum = [0] * (src_len + 1)

    for i in range(src_len):
        if key is not None:
            value = key(src[i])
        else:
            value = src[i]

        sum[i + 1] = sum[i] + value

    return sum

def lowerbound(src, value) -> int:
    src_len = len(src)

    if src_len == 0 or value > src[src_len - 1]:
        index = src_len
    else:
        ok = src_len - 1
        ng = -1
        while abs(ok - ng) > 1:
            mid = (ok + ng) // 2
            if src[mid] >= value:
                ok = mid
            else:
                ng = mid
        index = ok

        assert(0 <= index and index < src_len)
        assert(value <= src[index])
        assert(index == 0 or value > src[index - 1])
    return index
