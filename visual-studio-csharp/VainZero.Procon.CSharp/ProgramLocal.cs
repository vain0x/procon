using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

public sealed class ProgramLocal
{
    public static void Main(string[] args)
    {
        using (var writer = new VainZero.IO.DebugTextWriter(Console.Out))
        {
            new Program(Console.In, writer).EntryPoint();
        }
    }
}
