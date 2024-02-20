using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace NMF.Glsp.Language
{
    /// <summary>
    /// Extends the syntax elements for configurations possible at a node label
    /// </summary>
    /// <typeparam name="T">The semantic type of elements for which a label is created</typeparam>
    public interface INodeLabelSyntax<T> : ILabelSyntax<T, INodeLabelSyntax<T>>
    {
        /// <summary>
        /// Overrides the positioning of the label to a fixed position
        /// </summary>
        /// <param name="x">The x coordinate of the new label position</param>
        /// <param name="y">The y coordinate of the new label position</param>
        /// <returns>A label syntax element for chaining purposes</returns>
        INodeLabelSyntax<T> At(double x, double y);
    }
}
