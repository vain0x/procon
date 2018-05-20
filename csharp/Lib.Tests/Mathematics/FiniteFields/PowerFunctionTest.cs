using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xunit;

namespace Procon
{
    public sealed class PowerFunctionTest
    {
        private const int Mod = 1000000007;
        private readonly PowerFunction _f = new PowerFunction(Mod);

        [Fact]
        public void Test_Power()
        {
            _f.Power(0, 1).Is(0);
            _f.Power(0, 0).Is(1);
            _f.Power(1, 99).Is(1);
            _f.Power(2, 4).Is(16);
            _f.Power(3, 5).Is(243);

            _f.Power(314159, 271828).Is(959898892);
            _f.Power(271828, 314159).Is(597324669);
        }

        [Theory]
        [InlineData(1L)]
        [InlineData(2L)]
        [InlineData(3L)]
        [InlineData(314159L)]
        [InlineData(1000000006L)]
        public void Test_Inverse(long value)
        {
            (_f.Inverse(value) * value % Mod).Is(1);
        }
    }
}
