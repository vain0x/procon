using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace VainZero.IO
{
    /// <summary>
    /// Represents a text writer to multicast messages
    /// to the underlying text writer and trace listeners.
    /// </summary>
    internal sealed class DebugTextWriter
        : TextWriter
    {
        private readonly TextWriter _writer;

        public override Encoding Encoding
        {
            get { return _writer.Encoding; }
        }

        public override void Close()
        {
            _writer.Close();
            Debug.Close();
            base.Close();
        }

        protected override void Dispose(bool disposing)
        {
            base.Dispose(disposing);
        }

        public override void Flush()
        {
            _writer.Flush();
            Debug.Flush();
            base.Flush();
        }

        public override void Write(bool value)
        {
            _writer.Write(value);
            Debug.Write(value);
        }

        public override void Write(char value)
        {
            _writer.Write(value);
            Debug.Write(value);
        }

        public override void Write(char[] buffer)
        {
            _writer.Write(buffer);
            Debug.Write(buffer);
        }

        public override void Write(decimal value)
        {
            _writer.Write(value);
            Debug.Write(value);
        }

        public override void Write(double value)
        {
            _writer.Write(value);
            Debug.Write(value);
        }

        public override void Write(float value)
        {
            _writer.Write(value);
            Debug.Write(value);
        }

        public override void Write(int value)
        {
            _writer.Write(value);
            Debug.Write(value);
        }

        public override void Write(long value)
        {
            _writer.Write(value);
            Debug.Write(value);
        }

        public override void Write(object value)
        {
            _writer.Write(value);
            Debug.Write(value);
        }

        public override void Write(string value)
        {
            _writer.Write(value);
            Debug.Write(value);
        }

        public override void Write(uint value)
        {
            _writer.Write(value);
            Debug.Write(value);
        }

        public override void Write(ulong value)
        {
            _writer.Write(value);
            Debug.Write(value);
        }

        public override void Write(string format, object arg0)
        {
            _writer.Write(format, arg0);
            Debug.Write(string.Format(format, arg0));
        }

        public override void Write(string format, params object[] arg)
        {
            _writer.Write(format, arg);
            Debug.Write(string.Format(format, arg));
        }

        public override void Write(char[] buffer, int index, int count)
        {
            _writer.Write(buffer, index, count);
            Debug.Write(new string(buffer, index, count));
        }

        public override void Write(string format, object arg0, object arg1)
        {
            _writer.Write(format, arg0, arg1);
            Debug.Write(string.Format(format, arg0, arg1));
        }

        public override void Write(string format, object arg0, object arg1, object arg2)
        {
            _writer.Write(format, arg0, arg1, arg2);
            Debug.Write(string.Format(format, arg0, arg1, arg2));
        }

        public override void WriteLine()
        {
            _writer.WriteLine();
            Debug.WriteLine("");
        }

        public override void WriteLine(bool value)
        {
            _writer.WriteLine(value);
            Debug.WriteLine(value);
        }

        public override void WriteLine(char value)
        {
            _writer.WriteLine(value);
            Debug.WriteLine(value);
        }

        public override void WriteLine(char[] buffer)
        {
            _writer.WriteLine(buffer);
            Debug.WriteLine(buffer);
        }

        public override void WriteLine(decimal value)
        {
            _writer.WriteLine(value);
            Debug.WriteLine(value);
        }

        public override void WriteLine(double value)
        {
            _writer.WriteLine(value);
            Debug.WriteLine(value);
        }

        public override void WriteLine(float value)
        {
            _writer.WriteLine(value);
            Debug.WriteLine(value);
        }

        public override void WriteLine(int value)
        {
            _writer.WriteLine(value);
            Debug.WriteLine(value);
        }

        public override void WriteLine(long value)
        {
            _writer.WriteLine(value);
            Debug.WriteLine(value);
        }

        public override void WriteLine(object value)
        {
            _writer.WriteLine(value);
            Debug.WriteLine(value);
        }

        public override void WriteLine(string value)
        {
            _writer.WriteLine(value);
            Debug.WriteLine(value);
        }

        public override void WriteLine(uint value)
        {
            _writer.WriteLine(value);
            Debug.WriteLine(value);
        }

        public override void WriteLine(ulong value)
        {
            _writer.WriteLine(value);
            Debug.WriteLine(value);
        }

        public override void WriteLine(string format, object arg0)
        {
            _writer.WriteLine(format, arg0);
            Debug.WriteLine(string.Format(format, arg0));
        }

        public override void WriteLine(string format, params object[] arg)
        {
            _writer.WriteLine(format, arg);
            Debug.WriteLine(string.Format(format, arg));
        }

        public override void WriteLine(char[] buffer, int index, int count)
        {
            _writer.WriteLine(buffer, index, count);
            Debug.WriteLine(new string(buffer, index, count));
        }

        public override void WriteLine(string format, object arg0, object arg1)
        {
            _writer.WriteLine(format, arg0, arg1);
            Debug.WriteLine(string.Format(format, arg0, arg1));
        }

        public override void WriteLine(string format, object arg0, object arg1, object arg2)
        {
            _writer.WriteLine(format, arg0, arg1, arg2);
            Debug.WriteLine(string.Format(format, arg0, arg1, arg2));
        }

        public DebugTextWriter(TextWriter writer)
        {
            _writer = writer;
        }
    }
}
