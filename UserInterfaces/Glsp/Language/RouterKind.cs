using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NMF.Glsp.Language
{
    /// <summary>
    /// Denotes supported router kinds
    /// </summary>
    public enum RouterKind
    {
        /// <summary>
        /// Edge is not routed at all
        /// </summary>
        None,
        
        /// <summary>
        /// Denotes that edges are routed using a manhattan style, i.e. in parallel to the axes
        /// </summary>
        Manhattan,

        /// <summary>
        /// Denotes that edges are routed using Bezier curves
        /// </summary>
        Bezier
    }
}
