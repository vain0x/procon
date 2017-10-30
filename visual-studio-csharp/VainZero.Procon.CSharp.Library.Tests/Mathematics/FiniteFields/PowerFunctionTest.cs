using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xunit;

namespace VainZero.Mathematics.FiniteFields
{
    public sealed class PowerFunctionTest
    {
        private const int Mod = 1000000007;
        private readonly PowerFunction _f = new PowerFunction(Mod);

        [Fact]
        public void Test_Power()
        {
            Assert.Equal(0, _f.Power(0, 1));
            Assert.Equal(1, _f.Power(0, 0));
            Assert.Equal(1, _f.Power(1, 99));
            Assert.Equal(16, _f.Power(2, 4));
            Assert.Equal(243, _f.Power(3, 5));

            Assert.Equal(959898892, _f.Power(314159, 271828));
            Assert.Equal(597324669, _f.Power(271828, 314159));
        }

        [Theory]
        [InlineData(1L)]
        [InlineData(2L)]
        [InlineData(3L)]
        [InlineData(314159L)]
        [InlineData(1000000006L)]
        public void Test_Inverse(long value)
        {
            Assert.Equal(1, _f.Inverse(value) * value % Mod);
        }
    }
}
