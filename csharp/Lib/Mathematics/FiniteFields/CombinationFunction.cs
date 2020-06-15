public sealed class CombinationFunction
{
    private readonly int _mod;
    private readonly long[] _factorial;
    private readonly long[] _factorialInverse;

    public long Combination(int m, int k)
    {
        if (k == 0 || m == k)
            return 1;

        if (k < 0 || k > m)
            return 0;

        var c = _factorial[m];
        c = (c * _factorialInverse[m - k]) % _mod;
        c = (c * _factorialInverse[k]) % _mod;
        return c;
    }

    public CombinationFunction(int m, int mod)
    {
        _mod = mod;

        _factorial = new long[m + 1];
        _factorial[0] = 1;
        _factorial[1] = 1;

        _factorialInverse = new long[m + 1];
        _factorialInverse[0] = 1;
        _factorialInverse[1] = 1;

        var inverse = new long[m + 1];
        inverse[1] = 1;

        for (var i = 2; i <= m; i++)
        {
            _factorial[i] = (_factorial[i - 1] * i) % mod;
            inverse[i] = ((-inverse[mod % i] * (mod / i)) % mod + mod) % mod;
            _factorialInverse[i] = (_factorialInverse[i - 1] * inverse[i]) % mod;
        }
    }
}
