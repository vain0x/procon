namespace Procon
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Text;
    using System.Threading.Tasks;

    public sealed class CombinationFunction
    {
        private readonly int _mod;
        private readonly long[] _factorial;
        private readonly long[] _factorialInverse;

        public long Combination(int n, int k)
        {
            if (k == 0 || n == k) return 1;
            if (k < 0 || k > n) return 0;

            var c = _factorial[n];
            c = (c * _factorialInverse[n - k]) % _mod;
            c = (c * _factorialInverse[k]) % _mod;
            return c;
        }

        public CombinationFunction(int n, int mod)
        {
            _mod = mod;

            _factorial = new long[n + 1];
            _factorial[0] = 1;
            _factorial[1] = 1;

            _factorialInverse = new long[n + 1];
            _factorialInverse[0] = 1;
            _factorialInverse[1] = 1;

            var inverse = new long[n + 1];
            inverse[1] = 1;

            for (var i = 2; i <= n; i++)
            {
                _factorial[i] = (_factorial[i - 1] * i) % mod;
                inverse[i] = ((-inverse[mod % i] * (mod / i)) % mod + mod) % mod;
                _factorialInverse[i] = (_factorialInverse[i - 1] * inverse[i]) % mod;
            }
        }
    }
}
