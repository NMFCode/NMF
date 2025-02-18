using System;
using System.Collections.Generic;
using System.Data.Common;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NMF.AnyText
{
    /// <summary>
    /// Denotes a delta between parser positions
    /// </summary>
    /// <param name="Line">the line delta</param>
    /// <param name="Col">the column delta</param>
    public record struct ParsePositionDelta(int Line, int Col)
    {
        /// <summary>
        /// Calculates the larger of two diffs
        /// </summary>
        /// <param name="delta1">the first delta</param>
        /// <param name="delta2">the second delta</param>
        /// <returns>the larger delta of the two deltas</returns>
        public static ParsePositionDelta Larger(ParsePositionDelta delta1, ParsePositionDelta delta2)
        {
            if (delta1.Line >  delta2.Line) { return delta1; }
            if (delta1.Line < delta2.Line) { return delta2; }
            return delta1.Col > delta2.Col ? delta1 : delta2;
        }

        /// <summary>
        /// Calculates the smaller of two diffs
        /// </summary>
        /// <param name="delta1">the first delta</param>
        /// <param name="delta2">the second delta</param>
        /// <returns>the smaller delta of the two deltas</returns>
        public static ParsePositionDelta Smaller(ParsePositionDelta delta1, ParsePositionDelta delta2)
        {
            if (delta1 == Larger(delta1, delta2))
            {
                return delta2;
            }
            return delta1;
        }
    }
}
