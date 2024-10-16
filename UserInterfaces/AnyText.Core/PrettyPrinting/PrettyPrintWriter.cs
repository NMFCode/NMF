using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NMF.AnyText.PrettyPrinting
{
    /// <summary>
    /// Denotes a helper class to pretty-print texts
    /// </summary>
    public class PrettyPrintWriter
    {
        private readonly TextWriter _inner;
        private readonly string _indentString;
        private int _indentLevel;
        private bool _lastWasNewline = true;

        /// <summary>
        /// Creates a new instance
        /// </summary>
        /// <param name="inner">the inner text writer</param>
        /// <param name="indentString">the indentation string</param>
        public PrettyPrintWriter(TextWriter inner, string indentString)
        {
            _inner = inner;
            _indentString = indentString;
        }

        /// <summary>
        /// Increase the indentation level
        /// </summary>
        public void Indent()
        {
            _indentLevel++;
        }

        /// <summary>
        /// Decrease the indentation level
        /// </summary>
        public void Unindent()
        {
            _indentLevel--;
        }

        /// <summary>
        /// Writes the given token to the underlying writer
        /// </summary>
        /// <param name="token"></param>
        public void WriteToken(string token)
        {
            if (_lastWasNewline)
            {
                for (int i = 0; i < _indentLevel; i++)
                {
                    _inner.Write(_indentString);
                }
                _lastWasNewline = false;
            }
            else
            {
                _inner.Write(' ');
            }
            _inner.Write(token);
        }

        /// <summary>
        /// Writes a newline to the underlying writer
        /// </summary>
        public void WriteNewLine()
        {
            _inner.WriteLine();
            _lastWasNewline = true;
        }
    }
}
