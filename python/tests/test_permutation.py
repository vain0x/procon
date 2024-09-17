from lib.permutation import next_permutation

def test_next_permutation():
    src = [5, 3, 2, 4, 1]

    def text():
        return "".join(map(str, src))

    assert next_permutation(src)
    assert text() == "53412"

    assert next_permutation(src)
    assert text() == "53421"

    for i in range(6):
        assert next_permutation(src)

    assert text() == "54321"
    assert not next_permutation(src)
