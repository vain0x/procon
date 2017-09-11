using System;
using System.Collections;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

public static class TemplateExtension
{
    public static X[] MakeArray<X>(this int count, Func<int, X> func)
    {
        var xs = new X[count];
        for (var i = 0; i < count; i++)
        {
            xs[i] = func(i);
        }
        return xs;
    }

    public static int[] Range(this int count, int start = 0)
    {
        return count.MakeArray(i => i + start);
    }

    public static string Intercalate<X>(this IEnumerable<X> @this, string separator)
    {
        return string.Join(separator, @this);
    }

    public sealed class ValueIndexPair<T>
        : Tuple<T, int>
    {
        public T Value { get { return Item1; } }
        public int Index { get { return Item2; } }

        public ValueIndexPair(T value, int index)
            : base(value, index)
        {
        }
    }

    public static IEnumerable<ValueIndexPair<X>> Indexed<X>(this IEnumerable<X> @this)
    {
        var i = 0;
        foreach (var x in @this)
        {
            yield return new ValueIndexPair<X>(x, i);
            i++;
        }
    }
}

public sealed class Scanner
{
    readonly TextReader reader;
    readonly StringBuilder sb = new StringBuilder();

    /// <summary>
    /// Reads next word separated by spaces.
    /// </summary>
    public string Word()
    {
        sb.Clear();

        while (true)
        {
            var r = reader.Read();

            if (r == '\r')
            {
                if (reader.Peek() == '\n') reader.Read();
                break;
            }
            else if (r == -1 || r == ' ' || r == '\n')
            {
                break;
            }
            else
            {
                sb.Append((char)r);
            }
        }

        return sb.ToString();
    }

    /// <summary>
    /// Reads next word as <see cref="int"/>.
    /// </summary>
    public int N()
    {
        return int.Parse(Word());
    }

    /// <summary>
    /// Reads next word as <see cref="long"/>.
    /// </summary>
    public long L()
    {
        return long.Parse(Word());
    }

    /// <summary>
    /// Reads next word as <see cref="double"/>.
    /// </summary>
    public double F()
    {
        return double.Parse(Word());
    }

    public int[] Ns(int count)
    {
        return count.MakeArray(_ => N());
    }

    public long[] Ls(int count)
    {
        return count.MakeArray(_ => L());
    }

    public double[] Fs(int count)
    {
        return count.MakeArray(_ => F());
    }

    /// <summary>
    /// Reads next line and splits it by spaces.
    /// </summary>
    public X[] Words<X>(Func<string, X> func)
    {
        return reader.ReadLine().Split(' ').Select(func).ToArray();
    }

    public Scanner(TextReader reader)
    {
        this.reader = reader;
    }
}

public partial class Program
{
    readonly TextReader input;
    readonly TextWriter output;
    readonly Scanner scanner;

    void WriteLine(int value)
    {
        output.WriteLine(value);
    }

    void WriteLine(long value)
    {
        output.WriteLine(value);
    }

    void WriteLine(double value)
    {
        output.WriteLine(value);
    }

    void WriteLine(char value)
    {
        output.WriteLine(value);
    }

    void WriteLine(string value)
    {
        output.WriteLine(value);
    }

    public Program(TextReader input, TextWriter output)
    {
        this.input = input;
        this.output = output;
        scanner = new Scanner(input);
    }

    public static void Main(string[] args)
    {
#if DEBUG
        using (var writer = new VainZero.IO.DebugTextWriter(Console.Out))
#else
        var writer = Console.Out;
#endif
        {
            new Program(Console.In, writer).EntryPoint();
        }
    }
}

public sealed class Tsil<T>
{
    public Tsil<T> Head { get; }
    public T Value { get; }
    public int Length { get; }

    public T[] ToArray()
    {
        var array = new T[Length];
        var v = this;

        while (v.Length != 0)
        {
            array[v.Length - 1] = v.Value;
            v = v.Head;
        }

        return array;
    }

    public Tsil<T> Add(T value)
    {
        return new Tsil<T>(this, value);
    }

    Tsil()
    {
        Head = this;
        Value = default(T);
        Length = 0;
    }

    public Tsil(Tsil<T> head, T value)
    {
        Head = head;
        Value = value;
        Length = head.Length + 1;
    }

    public static readonly Tsil<T> Empty = new Tsil<T>();
}

public sealed partial class Program
{
    /// <summary>
    /// 最長一致部分列をDPで検出する。
    /// - source の中に最初に現れる部分列が選ばれる。
    /// - 時間計算量 O(|S| |T|)
    /// </summary>
    public string Lcs(string source, string target)
    {
        var sn = source.Length;
        var tn = target.Length;
        var dp = (sn + 1).MakeArray(i => (tn + 1).MakeArray(j => Tsil<char>.Empty));

        for (var si = 1; si < sn + 1; si++)
        {
            for (var ti = 1; ti < tn + 1; ti++)
            {
                if (source[si - 1] == target[ti - 1])
                {
                    dp[si][ti] = dp[si - 1][ti - 1].Add(source[si - 1]);
                }
                else
                {
                    var l = dp[si - 1][ti];
                    var r = dp[si][ti - 1];
                    dp[si][ti] = l.Length >= r.Length ? l : r;
                }
            }
        }

        return new string(dp[sn][tn].ToArray());
    }

    int Solve()
    {
        return 0;
    }

    void Read()
    {
        var a = scanner;

    }

    public void EntryPoint()
    {
        while (true)
        {
            var s = input.ReadLine();
            var t = input.ReadLine();
            WriteLine(Lcs(s, t));
        }
    }
}
