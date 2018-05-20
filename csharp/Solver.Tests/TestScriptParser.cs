using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Procon
{
    public sealed class InputOutputPair
    {
        public string Input { get; }
        public string Output { get; }

        public InputOutputPair(string input, string output)
        {
            Input = input;
            Output = output;
        }
    }

    public sealed class TestScriptParser
    {
        private static string NormalizeLinebreaks(string str)
        {
            return str.Replace("\r\n", "\n").Replace("\r", "\n");
        }

        private static string[] SplitByLinebreaks(string str)
        {
            return NormalizeLinebreaks(str).Split('\n');
        }

        private static string Concat(IEnumerable<string> lines)
        {
            var builder = new StringBuilder();
            foreach (var line in lines)
            {
                builder.AppendLine(line);
            }
            return builder.ToString();
        }

        private static IEnumerable<IReadOnlyList<X>> SplitBy<X>(IEnumerable<X> xs, X delimiter)
        {
            var chunk = new List<X>();
            foreach (var x in xs)
            {
                if (EqualityComparer<X>.Default.Equals(x, delimiter))
                {
                    yield return chunk;
                    chunk = new List<X>();
                }
                else
                {
                    chunk.Add(x);
                }
            }
            yield return chunk;
        }

        private static IEnumerable<IReadOnlyList<X>> ChunkBySize<X>(IEnumerable<X> xs, int chunkSize)
        {
            if (chunkSize <= 0) throw new ArgumentOutOfRangeException(nameof(chunkSize));
            var chunkOrNull = default(List<X>);
            foreach (var x in xs)
            {
                if (chunkOrNull == null) chunkOrNull = new List<X>(chunkSize);

                chunkOrNull.Add(x);
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

        public IEnumerable<InputOutputPair> Parse(string source)
        {
            var lines =
                SplitByLinebreaks(source).Select(line => line.Trim());
            var groupedLines =
                SplitBy(lines, "").Where(list => list.Count > 0);

            foreach (var chunk in ChunkBySize(groupedLines, 2))
            {
                if (chunk.Count != 2)
                {
                    throw new Exception("Missing output for the last input.");
                }

                yield return new InputOutputPair(Concat(chunk[0]), Concat(chunk[1]));
            }
        }
    }
}
