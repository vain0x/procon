from lib.array import lower_bound, upper_bound

def test_lower_bound():
    a = [1, 1, 2, 3, 5]

    table = [
        (0, 0),
        (1, 0),
        (2, 2),
        (3, 3),
        (4, 4),
        (5, 4),
        (6, 5),
    ]
    for (value, expected) in table:
        actual = lower_bound(a, value)
        assert actual == expected

def test_upper_bound():
    a = [1, 1, 2, 3, 5]

    table = [
        (0, 0),
        (1, 2),
        (2, 3),
        (3, 4),
        (4, 4),
        (5, 5),
        (6, 5),
    ]
    for (value, expected) in table:
        actual = upper_bound(a, value)
        assert actual == expected
