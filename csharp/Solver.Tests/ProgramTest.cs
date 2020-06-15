using System.Collections.Generic;
using System.IO;
using System.Linq;
using Xunit;

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
    public void Test((string input, string output) pair)
    {
        var (input, output) = pair;
        var inputReader = new StringReader(input);
        var outputWriter = new StringWriter();
        new Program(inputReader, outputWriter).EntryPoint();
        Assert.Equal(output, outputWriter.ToString());
    }
}
