from lib.array import lowerbound, upperbound

def test_lowerbound():
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
        actual = lowerbound(a, value)
        assert actual == expected

def test_upperbound():
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
        actual = upperbound(a, value)
        assert actual == expected
