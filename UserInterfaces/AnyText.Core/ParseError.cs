using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NMF.AnyText
{
    /// <summary>
    /// Denotes an error while parsing
    /// </summary>
    /// <param name="Message">the error message</param>
    /// <param name="Position">the position of the error</param>
    public record struct ParseError(ParsePosition Position, string Message)
    {
    }
}
