using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace VainZero.Mathematics.FiniteFields
{
    public sealed class CombinationFunction
    {
        struct PowerFunction
        {
            readonly int mod;

            public long Power(long x, int n)
            {
                var y = 1L;
                while (n > 0)
                {
                    if (n % 2 == 0)
                    {
                        x = (x * x) % mod;
                        n /= 2;
                    }
                    else
                    {
                        y = (y * x) % mod;
                        n--;
                    }
                }
                return y;
            }

            public long Inverse(long x)
            {
                return Power(x, mod - 2);
            }

            public PowerFunction(int mod)
            {
                this.mod = mod;
            }
        }

        struct FactorialFunction
        {
            readonly long[] dp;

            public long Factorial(int n)
            {
                return dp[n];
            }

            public FactorialFunction(int m, int mod)
            {
                dp = new long[m + 1];

                for (var i = 0; i <= m; i++)
                {
                    dp[i] = (i == 0 ? 1 : (dp[i - 1] * i) % mod);
                }
            }
        }

        readonly int mod;
        readonly FactorialFunction factorial;
        readonly long[] factorialInverse;

        public long Combination(int n, int k)
        {
            if (k == 0 || n == k) return 1;
            if (k < 0 || k > n) return 0;

            var c = factorial.Factorial(n);
            c = (c * factorialInverse[n - k]) % mod;
            c = (c * factorialInverse[k]) % mod;
            return c;
        }

        public CombinationFunction(int n, int mod)
        {
            this.mod = mod;
            factorial = new FactorialFunction(n, mod);

            var power = new PowerFunction(mod);
            factorialInverse = new long[n + 1];
            for (var i = 0; i < factorialInverse.Length; i++)
            {
                factorialInverse[i] = power.Inverse(factorial.Factorial(i));
            }
        }
    }
}
