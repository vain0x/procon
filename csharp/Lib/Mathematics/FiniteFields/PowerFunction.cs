namespace Procon
{
    public sealed class PowerFunction
    {
        private readonly int _mod;

        public long Power(long x, int n)
        {
            var y = 1L;
            while (n > 0)
            {
                if (n % 2 == 0)
                {
                    x = (x * x) % _mod;
                    n /= 2;
                }
                else
                {
                    y = (y * x) % _mod;
                    n--;
                }
            }
            return y;
        }

        public long Inverse(long x)
        {
            return Power(x, _mod - 2);
        }

        public PowerFunction(int mod)
        {
            _mod = mod;
        }
    }
}
