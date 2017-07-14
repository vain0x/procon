using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace VainZero.Mathematics.FiniteFields
{
    public sealed class CombinationFunction
    {
        readonly int mod;
        readonly long[] factorial;
        readonly long[] factorialInverse;

        public long Combination(int n, int k)
        {
            if (k == 0 || n == k) return 1;
            if (k < 0 || k > n) return 0;

            var c = factorial[n];
            c = (c * factorialInverse[n - k]) % mod;
            c = (c * factorialInverse[k]) % mod;
            return c;
        }

        public CombinationFunction(int n, int mod)
        {
            this.mod = mod;

            factorial = new long[n + 1];
            factorial[0] = 1;
            factorial[1] = 1;

            factorialInverse = new long[n + 1];
            factorialInverse[0] = 1;
            factorialInverse[1] = 1;

            var inverse = new long[n + 1];
            inverse[1] = 1;

            for (var i = 2; i <= n; i++)
            {
                factorial[i] = (factorial[i - 1] * i) % mod;
                inverse[i] = ((-inverse[mod % i] * (mod / i)) % mod + mod) % mod;
                factorialInverse[i] = (factorialInverse[i - 1] * inverse[i]) % mod;
            }
        }
    }
}
