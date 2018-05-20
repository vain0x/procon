using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xunit;

namespace Procon
{
    public class ProgramTest
    {
        public static IEnumerable<object[]> TestCases()
        {
            var script = @"

";

            var parser = new TestScriptParser();
            return parser.Parse(script).Select(pair => new object[] { pair });
        }

        [Theory]
        [MemberData(nameof(TestCases))]
        public void Test(InputOutputPair pair)
        {
            var inputReader = new StringReader(pair.Input);
            var outputWriter = new StringWriter();
            new Program(inputReader, outputWriter).EntryPoint();
            Assert.Equal(pair.Output, outputWriter.ToString());
        }
    }
}
