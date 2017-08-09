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
    sealed class DebugTextWriter
        : TextWriter
    {
        readonly TextWriter writer;

        public override Encoding Encoding
        {
            get { return writer.Encoding; }
        }

        public override void Close()
        {
            writer.Close();
            Debug.Close();
            base.Close();
        }

        protected override void Dispose(bool disposing)
        {
            base.Dispose(disposing);
        }

        public override void Flush()
        {
            writer.Flush();
            Debug.Flush();
            base.Flush();
        }

        public override void Write(bool value)
        {
            writer.Write(value);
            Debug.Write(value);
        }

        public override void Write(char value)
        {
            writer.Write(value);
            Debug.Write(value);
        }

        public override void Write(char[] buffer)
        {
            writer.Write(buffer);
            Debug.Write(buffer);
        }

        public override void Write(decimal value)
        {
            writer.Write(value);
            Debug.Write(value);
        }

        public override void Write(double value)
        {
            writer.Write(value);
            Debug.Write(value);
        }

        public override void Write(float value)
        {
            writer.Write(value);
            Debug.Write(value);
        }

        public override void Write(int value)
        {
            writer.Write(value);
            Debug.Write(value);
        }

        public override void Write(long value)
        {
            writer.Write(value);
            Debug.Write(value);
        }

        public override void Write(object value)
        {
            writer.Write(value);
            Debug.Write(value);
        }

        public override void Write(string value)
        {
            writer.Write(value);
            Debug.Write(value);
        }

        public override void Write(uint value)
        {
            writer.Write(value);
            Debug.Write(value);
        }

        public override void Write(ulong value)
        {
            writer.Write(value);
            Debug.Write(value);
        }

        public override void Write(string format, object arg0)
        {
            writer.Write(format, arg0);
            Debug.Write(string.Format(format, arg0));
        }

        public override void Write(string format, params object[] arg)
        {
            writer.Write(format, arg);
            Debug.Write(string.Format(format, arg));
        }

        public override void Write(char[] buffer, int index, int count)
        {
            writer.Write(buffer, index, count);
            Debug.Write(new string(buffer, index, count));
        }

        public override void Write(string format, object arg0, object arg1)
        {
            writer.Write(format, arg0, arg1);
            Debug.Write(string.Format(format, arg0, arg1));
        }

        public override void Write(string format, object arg0, object arg1, object arg2)
        {
            writer.Write(format, arg0, arg1, arg2);
            Debug.Write(string.Format(format, arg0, arg1, arg2));
        }

        public override void WriteLine()
        {
            writer.WriteLine();
            Debug.WriteLine("");
        }

        public override void WriteLine(bool value)
        {
            writer.WriteLine(value);
            Debug.WriteLine(value);
        }

        public override void WriteLine(char value)
        {
            writer.WriteLine(value);
            Debug.WriteLine(value);
        }

        public override void WriteLine(char[] buffer)
        {
            writer.WriteLine(buffer);
            Debug.WriteLine(buffer);
        }

        public override void WriteLine(decimal value)
        {
            writer.WriteLine(value);
            Debug.WriteLine(value);
        }

        public override void WriteLine(double value)
        {
            writer.WriteLine(value);
            Debug.WriteLine(value);
        }

        public override void WriteLine(float value)
        {
            writer.WriteLine(value);
            Debug.WriteLine(value);
        }

        public override void WriteLine(int value)
        {
            writer.WriteLine(value);
            Debug.WriteLine(value);
        }

        public override void WriteLine(long value)
        {
            writer.WriteLine(value);
            Debug.WriteLine(value);
        }

        public override void WriteLine(object value)
        {
            writer.WriteLine(value);
            Debug.WriteLine(value);
        }

        public override void WriteLine(string value)
        {
            writer.WriteLine(value);
            Debug.WriteLine(value);
        }

        public override void WriteLine(uint value)
        {
            writer.WriteLine(value);
            Debug.WriteLine(value);
        }

        public override void WriteLine(ulong value)
        {
            writer.WriteLine(value);
            Debug.WriteLine(value);
        }

        public override void WriteLine(string format, object arg0)
        {
            writer.WriteLine(format, arg0);
            Debug.WriteLine(string.Format(format, arg0));
        }

        public override void WriteLine(string format, params object[] arg)
        {
            writer.WriteLine(format, arg);
            Debug.WriteLine(string.Format(format, arg));
        }

        public override void WriteLine(char[] buffer, int index, int count)
        {
            writer.WriteLine(buffer, index, count);
            Debug.WriteLine(new string(buffer, index, count));
        }

        public override void WriteLine(string format, object arg0, object arg1)
        {
            writer.WriteLine(format, arg0, arg1);
            Debug.WriteLine(string.Format(format, arg0, arg1));
        }

        public override void WriteLine(string format, object arg0, object arg1, object arg2)
        {
            writer.WriteLine(format, arg0, arg1, arg2);
            Debug.WriteLine(string.Format(format, arg0, arg1, arg2));
        }

        public DebugTextWriter(TextWriter writer)
        {
            this.writer = writer;
        }
    }
}
