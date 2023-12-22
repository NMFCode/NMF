using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NMF.Glsp.Protocol.Layout
{
    /// <summary>
    /// Denotes layout options for the rounded corner view
    /// </summary>
    public class RoundedCornerOptions
    {
        /// <summary>
        /// Gets the radius for rounded corners in the bottom left corner
        /// </summary>
        public int? RadiusBottomLeft { get; init; }

        /// <summary>
        /// Gets the radius for rounded corners in the top left corner
        /// </summary>
        public int? RadiusTopLeft { get; init; }

        /// <summary>
        /// Gets the radius for rounded corners in the top right corner
        /// </summary>
        public int? RadiusTopRight { get; init; }

        /// <summary>
        /// Gets the radius for rounded corners in the bottom right corner
        /// </summary>
        public int? RadiusBottomRight { get; init; }
    }
}
