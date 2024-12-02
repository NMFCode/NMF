using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NMF.AnyText
{
    /// <summary>
    /// Denotes a position of a parser
    /// </summary>
    /// <param name="Line">The line of the position</param>
    /// <param name="Col">The column of the position</param>
    public record struct ParsePosition(int Line, int Col) : IComparable<ParsePosition>
    {
        /// <summary>
        /// Proceeds the position by the given number of characters
        /// </summary>
        /// <param name="chars">the numbers of characters to proceed</param>
        /// <returns>The updated parser position</returns>
        public ParsePosition Proceed(int chars)
        {
            return new ParsePosition(Line, Col + chars);
        }

        /// <inheritdoc />
        public int CompareTo(ParsePosition other)
        {
            var ret = Line.CompareTo(other.Line);
            if (ret == 0) ret = Col.CompareTo(other.Col);
            return ret;
        }

        /// <summary>
        /// Decides which of the two positions is smaller
        /// </summary>
        /// <param name="pos1">the first position</param>
        /// <param name="pos2">the second position</param>
        /// <returns>true, if the first position is smaller, otherwise false</returns>
        public static bool operator <(ParsePosition pos1, ParsePosition pos2)
        {
            return pos1.CompareTo(pos2) < 0;
        }

        /// <summary>
        /// Decides which of the two positions is greater
        /// </summary>
        /// <param name="pos1">the first position</param>
        /// <param name="pos2">the second position</param>
        /// <returns>true, if the first position is greater, otherwise false</returns>
        public static bool operator >(ParsePosition pos1, ParsePosition pos2)
        {
            return pos1.CompareTo(pos2) > 0;
        }

        /// <summary>
        /// Decides which of the two positions is smaller or equal
        /// </summary>
        /// <param name="pos1">the first position</param>
        /// <param name="pos2">the second position</param>
        /// <returns>true, if the first position is smaller, otherwise false</returns>
        public static bool operator <=(ParsePosition pos1, ParsePosition pos2)
        {
            return pos1.CompareTo(pos2) <= 0;
        }

        /// <summary>
        /// Decides which of the two positions is greater or equal
        /// </summary>
        /// <param name="pos1">the first position</param>
        /// <param name="pos2">the second position</param>
        /// <returns>true, if the first position is greater, otherwise false</returns>
        public static bool operator >=(ParsePosition pos1, ParsePosition pos2)
        {
            return pos1.CompareTo(pos2) >= 0;
        }

        /// <summary>
        /// Subtracts two parse positions
        /// </summary>
        /// <param name="to">the forward position</param>
        /// <param name="from">the backward position</param>
        /// <returns>The delta between the given positions</returns>
        public static ParsePositionDelta operator -(ParsePosition to, ParsePosition from)
        {
            if (from.Line == to.Line)
            {
                return new ParsePositionDelta(0, to.Col - from.Col);
            }
            return new ParsePositionDelta(to.Line - from.Line, to.Col);
        }

        /// <summary>
        /// Adds the given delta to the current position
        /// </summary>
        /// <param name="pos">the origin position</param>
        /// <param name="delta">the position delta</param>
        /// <returns>the updated position</returns>
        public static ParsePosition operator +(ParsePosition pos, ParsePositionDelta delta)
        {
            if (delta.Line == 0)
            {
                return new ParsePosition(pos.Line, pos.Col + delta.Col);
            }
            return new ParsePosition(pos.Line + delta.Line, delta.Col);
        }
    }
}
