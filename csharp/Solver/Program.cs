// Framework <https://github.com/vain0x/procon>

using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Text;

public static class TemplateExtension
{
    public static T[] MakeArray<T>(this int count, Func<int, T> func)
    {
        var array = new T[count];
        for (var i = 0; i < count; i++)
        {
            array[i] = func(i);
        }
        return array;
    }

    public static int[] Range(this int count, int start = 0) =>
        count.MakeArray(i => i + start);

    public static string Intercalate<T>(this IEnumerable<T> source, string separator) =>
        string.Join(separator, source);

    public sealed class ValueIndexPair<T>
        : Tuple<T, int>
    {
        public T Value => Item1;
        public int Index => Item2;

        public ValueIndexPair(T value, int index)
            : base(value, index)
        {
        }
    }

    public static IEnumerable<ValueIndexPair<T>> Indexed<T>(this IEnumerable<T> source)
    {
        var i = 0;
        foreach (var item in source)
        {
            yield return new ValueIndexPair<T>(item, i);
            i++;
        }
    }
}

public sealed class Scanner
{
    private readonly TextReader _reader;
    private readonly StringBuilder _sb = new StringBuilder();

    /// <summary>
    /// Reads next word separated by spaces.
    /// </summary>
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
    public T[] Words<T>(Func<string, T> func) =>
        _reader.ReadLine().Split(' ').Select(func).ToArray();

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

// ###############################################

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
