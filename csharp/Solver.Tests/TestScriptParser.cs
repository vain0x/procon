using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

public sealed class TestScriptParser
{
    private static string NormalizeLinebreaks(string str) =>
        str.Replace("\r\n", "\n").Replace("\r", "\n");

    private static string[] SplitByLinebreaks(string str) =>
        NormalizeLinebreaks(str).Split('\n');

    private static string Concat(IEnumerable<string> lines)
    {
        var builder = new StringBuilder();
        foreach (var line in lines)
        {
            builder.AppendLine(line);
        }
        return builder.ToString();
    }

    private static IEnumerable<IReadOnlyList<T>> SplitBy<T>(IEnumerable<T> source, T delimiter)
    {
        var chunk = new List<T>();
        foreach (var item in source)
        {
            if (EqualityComparer<T>.Default.Equals(item, delimiter))
            {
                yield return chunk;
                chunk = new List<T>();
            }
            else
            {
                chunk.Add(item);
            }
        }
        yield return chunk;
    }

    private static IEnumerable<IReadOnlyList<T>> ChunkBySize<T>(IEnumerable<T> source, int chunkSize)
    {
        if (chunkSize <= 0)
            throw new ArgumentOutOfRangeException(nameof(chunkSize));

        var chunkOrNull = default(List<T>);
        foreach (var item in source)
        {
            if (chunkOrNull == null)
            {
                chunkOrNull = new List<T>(chunkSize);
            }

            chunkOrNull.Add(item);
            if (chunkOrNull.Count == chunkSize)
            {
                yield return chunkOrNull;
                chunkOrNull = null;
            }
        }

        if (chunkOrNull != null)
        {
            yield return chunkOrNull;
        }
    }

    public IEnumerable<(string input, string output)> Parse(string source)
    {
        var lines =
            SplitByLinebreaks(source)
            .Select(line => line.Trim());
        var groupedLines =
            SplitBy(lines, "")
            .Where(list => list.Count > 0);

        foreach (var chunk in ChunkBySize(groupedLines, 2))
        {
            if (chunk.Count != 2)
                throw new Exception("Missing output for the last input.");

            yield return (Concat(chunk[0]), Concat(chunk[1]));
        }
    }
}
