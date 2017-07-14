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
        const int Mod = 1000000007;
        readonly PowerFunction f = new PowerFunction(Mod);

        [Fact]
        public void Test_Power()
        {
            Assert.Equal(0, f.Power(0, 1));
            Assert.Equal(1, f.Power(0, 0));
            Assert.Equal(1, f.Power(1, 99));
            Assert.Equal(16, f.Power(2, 4));
            Assert.Equal(243, f.Power(3, 5));

            Assert.Equal(959898892, f.Power(314159, 271828));
            Assert.Equal(597324669, f.Power(271828, 314159));
        }

        [Theory]
        [InlineData(1L)]
        [InlineData(2L)]
        [InlineData(3L)]
        [InlineData(314159L)]
        [InlineData(1000000006L)]
        public void Test_Inverse(long value)
        {
            Assert.Equal(1, f.Inverse(value) * value % Mod);
        }
    }
}
