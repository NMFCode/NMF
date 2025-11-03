using System.IO;

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
        private bool _lastWasNewline = false;
        private bool _writeSpace = false;

        /// <summary>
        /// Supresses rendering a space character before the next token
        /// </summary>
        public void SupressSpace()
        {
            _writeSpace = false;
            _lastWasNewline = false;
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
        /// Writes the given text to the inner text writer
        /// </summary>
        /// <param name="token">the token to write</param>
        protected virtual void WriteText(string token)
        {
            _inner.Write(token);
        }

        /// <summary>
        /// Writes the given text to the inner text writer
        /// </summary>
        /// <param name="token">the token to write</param>
        protected virtual void WriteText(char token)
        {
            _inner.Write(token);
        }

        /// <summary>
        /// Writes a new line to the inner text writer
        /// </summary>
        protected virtual void WriteLine()
        {
            _inner.WriteLine();
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
                WriteLine();
                for (int i = 0; i < _indentLevel; i++)
                {
                    WriteText(_indentString);
                }
                _lastWasNewline = false;
            }
            else if (_writeSpace)
            {
                WriteText(' ');
            }
            WriteText(token);
            _writeSpace = appendSpace;
        }

        /// <summary>
        /// Writes a newline to the underlying writer
        /// </summary>
        public void WriteNewLine()
        {
            if (_lastWasNewline)
            {
                WriteLine();
            }
            _lastWasNewline = true;
            _writeSpace = false;
        }

        /// <summary>
        /// Writes raw (unformatted) text
        /// </summary>
        /// <param name="text">the unformatted text</param>
        public void WriteRaw(string text)
        {
            if (_lastWasNewline)
            {
                WriteLine();
            }
            WriteText(text);
            _lastWasNewline = false;
            _writeSpace = false;
        }
    }
}
