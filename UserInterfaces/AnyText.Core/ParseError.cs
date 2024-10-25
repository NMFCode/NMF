using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NMF.AnyText
{
    /// <summary>
    /// Denotes an error while parsing
    /// </summary>
    [DebuggerDisplay("{Position} : {Message} ({Source} error)")]
    public class ParseError
    {
        /// <summary>
        /// Creates a new instance
        /// </summary>
        /// <param name="source">the source of the error</param>
        /// <param name="position">the position of the error</param>
        /// <param name="length">the length of the error</param>
        /// <param name="message">the error message</param>
        public ParseError(string source, ParsePosition position, ParsePositionDelta length, string message)
        {
            Source = source;
            Position = position;
            Length = length;
            Message = message;
        }


        /// <summary>
        /// Gets the source of the error
        /// </summary>
        public string Source { get; }

        /// <summary>
        /// Gets the position of the error
        /// </summary>
        public ParsePosition Position { get; }

        /// <summary>
        /// Gets the length of the error
        /// </summary>
        public ParsePositionDelta Length { get; }

        /// <summary>
        /// Gets the error message
        /// </summary>
        public string Message { get; }

        /// <summary>
        /// Applies the given text edit to the error message
        /// </summary>
        /// <param name="edit"></param>
        /// <returns>true, if the error is still valid, otherwise false</returns>
        public bool ApplyEdit(TextEdit edit)
        {
            if (Position + Length < edit.Start)
            {
                return true;
            }
            if (Position > edit.End)
            {
                // TODO: update position
                return true;
            }
            return false;
        }
    }
}
