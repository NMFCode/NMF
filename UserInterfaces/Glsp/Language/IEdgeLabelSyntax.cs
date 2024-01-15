using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NMF.Glsp.Language
{
    /// <summary>
    /// Extends the label syntax for labels created on an edge
    /// </summary>
    /// <typeparam name="T">The semantic type of elements for which a label is created</typeparam>
    public interface IEdgeLabelSyntax<T> : ILabelSyntax<T, IEdgeLabelSyntax<T>>
    {
        /// <summary>
        /// Overrides the positioning of the label to a relative position
        /// </summary>
        /// <param name="pos">The relative position of the label, from 0 (source anchor) to 1 (target anchor)</param>
        /// <param name="rotate">True, if the label should be rotated, otherwise false</param>
        /// <param name="side">Denotes on which side the label is placed</param>
        /// <returns>A label syntax element for chaining purposes</returns>
        IEdgeLabelSyntax<T> At(double pos, EdgeSide side = EdgeSide.On, bool rotate = false);

        /// <summary>
        /// Sets the move mode of the edge label
        /// </summary>
        /// <param name="mode">The mode in which the label can be moved</param>
        /// <returns>A label syntax element for chaining purposes</returns>
        IEdgeLabelSyntax<T> MoveMode(EdgeMoveMode mode);
    }
}
