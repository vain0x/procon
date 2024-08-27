from functools import cache

MOD = 1000000007

@cache
def choice(n, r):
    if r == 0 or r == n:
        return 1

    return (choice(n - 1, r) + choice(n - 1, r - 1)) % MOD
