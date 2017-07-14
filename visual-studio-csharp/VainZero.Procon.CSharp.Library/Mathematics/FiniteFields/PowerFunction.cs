using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace VainZero.Mathematics.FiniteFields
{
    public sealed class PowerFunction
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
}
