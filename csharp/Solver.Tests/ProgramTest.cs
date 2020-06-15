using System.Collections.Generic;
using System.IO;
using System.Linq;
using Xunit;

namespace Procon
{
    public class ProgramTest
    {
        // Write test cases here:
        private static string Script { get; } = @"



";

        public static IEnumerable<object[]> TestCases() =>
            new TestScriptParser()
            .Parse(Script)
            .Select(pair => new object[] { pair });

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
