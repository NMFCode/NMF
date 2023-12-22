using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace NMF.Glsp.Language
{
    /// <summary>
    /// Denotes an interface for a syntax to customize labels
    /// </summary>
    /// <typeparam name="T">The semantic type of elements for which a label is created</typeparam>
    public interface ILabelSyntax<T>
    {
        /// <summary>
        /// Overrides the positioning of the label to a fixed position
        /// </summary>
        /// <param name="x">The x coordinate of the new label position</param>
        /// <param name="y">The y coordinate of the new label position</param>
        /// <returns>A label syntax element for chaining purposes</returns>
        ILabelSyntax<T> At(double x, double y);

        /// <summary>
        /// Adds a condition for when the label is generated
        /// </summary>
        /// <param name="guard">A function expression expressing a guard condition</param>
        /// <returns>A label syntax element for chaining purposes</returns>
        ILabelSyntax<T> If(Expression<Func<T, bool>> guard);

        /// <summary>
        /// Overrides the GLSP type created for the label
        /// </summary>
        /// <param name="type">The GLSP type for the label</param>
        /// <returns>A label syntax element for chaining purposes</returns>
        ILabelSyntax<T> WithType(string type);
    }
}
