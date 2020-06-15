// Framework <https://github.com/vain0x/procon>

using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Text;

public sealed partial class Program
{
    private long Solve()
    {
        return 0;
    }

    public void EntryPoint()
    {
        var I = _scanner;

        WriteLine(Solve());
    }
}

// ###############################################

public static class TemplateExtension
{
    public static T[] MakeArray<T>(this int count, Func<int, T> selector)
    {
        var array = new T[count];
        for (var i = 0; i < count; i++)
        {
            array[i] = selector(i);
        }
        return array;
    }

    public static int[] Range(this int count, int start = 0) =>
        count.MakeArray(i => i + start);

    public static string Intercalate<T>(this IEnumerable<T> source, string separator) =>
        string.Join(separator, source);

    public static IEnumerable<(T item, int index)> Indexed<T>(this IEnumerable<T> source) =>
        source.Select((item, i) => (item, i));
}

public sealed class Scanner
{
    private readonly TextReader _reader;
    private readonly StringBuilder _sb = new StringBuilder();

    public string Word()
    {
        _sb.Clear();

        while (true)
        {
            var r = _reader.Read();

            if (r == ' ' || r == '\r' || r == '\n')
            {
                if (r == '\r' && _reader.Peek() == '\n')
                {
                    _reader.Read();
                }

                // Ignore leading spaces.
                if (_sb.Length == 0)
                    continue;

                break;
            }

            if (r == -1)
                break;

            _sb.Append((char)r);
        }

        return _sb.ToString();
    }

    /// <summary>
    /// Reads next word as <see cref="int"/>.
    /// </summary>
    public int N() => int.Parse(Word().Trim());

    /// <summary>
    /// Reads next word as <see cref="long"/>.
    /// </summary>
    public long L() => long.Parse(Word());

    /// <summary>
    /// Reads next word as <see cref="double"/>.
    /// </summary>
    public double F() => double.Parse(Word());

    /// <summary>
    /// Reads next line and splits it by spaces.
    /// </summary>
    public T[] Words<T>(Func<string, T> selector) =>
        _reader.ReadLine().Split(' ').Select(selector).ToArray();

    public Scanner(TextReader reader) =>
        _reader = reader;
}

public partial class Program
{
    private readonly TextReader _input;
    private readonly TextWriter _output;
    private readonly Scanner _scanner;

    private void WriteLine(int value) =>
        _output.WriteLine(value);

    private void WriteLine(long value) =>
        _output.WriteLine(value);

    private void WriteLine(double value) =>
        _output.WriteLine(value.ToString(CultureInfo.InvariantCulture));

    private void WriteLine(char value) =>
        _output.WriteLine(value);

    private void WriteLine(string value) =>
        _output.WriteLine(value);

    public Program(TextReader input, TextWriter output)
    {
        _input = input;
        _output = output;
        _scanner = new Scanner(input);
    }

    public static void Main(string[] args) =>
        new Program(Console.In, Console.Out).EntryPoint();
}
