using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NMF.Glsp.Server.Protocol.Types
{
    /// <summary>
    /// The bounds are the position (x, y) and dimension (width, height) of an object. As such the Bounds type extends both Point and Dimension.
    /// </summary>
    public class Bounds
    {
        /// <summary>
        ///  The width of an element.
        /// </summary>
        public double Width { get; init; }


        /// <summary>
        ///  the height of an element.
        /// </summary>
        public double Height { get; init; }

        /// <summary>
        ///  The abscissa of the point.
        /// </summary>
        public double X { get; init; }


        /// <summary>
        ///  The ordinate of the point.
        /// </summary>
        public double Y { get; init; }
    }
}
