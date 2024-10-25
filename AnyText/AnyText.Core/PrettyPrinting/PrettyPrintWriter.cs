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
        private bool _writeSpace = false;

        /// <summary>
        /// Supresses rendering a space character before the next token
        /// </summary>
        public void SupressSpace()
        {
            _writeSpace = false;
        }

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
        /// <param name="token">the token that should be written</param>
        /// <param name="appendSpace">true, if a space should be appended when necessary</param>
        public void WriteToken(string token, bool appendSpace)
        {
            if (_lastWasNewline)
            {
                for (int i = 0; i < _indentLevel; i++)
                {
                    _inner.Write(_indentString);
                }
                _lastWasNewline = false;
            }
            else if (_writeSpace)
            {
                _inner.Write(' ');
            }
            _inner.Write(token);
            _writeSpace = appendSpace;
        }

        /// <summary>
        /// Writes a newline to the underlying writer
        /// </summary>
        public void WriteNewLine()
        {
            _inner.WriteLine();
            _lastWasNewline = true;
            _writeSpace = false;
        }
    }
}
