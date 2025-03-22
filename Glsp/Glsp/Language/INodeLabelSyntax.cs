using System;
using System.Linq.Expressions;

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

        /// <summary>
        /// Overrides the styling of the label to a fixed class
        /// </summary>
        /// <param name="css">The CSS class to use</param>
        /// <returns>A label syntax element for chaining purposes</returns>
        INodeLabelSyntax<T> WithCss(string css);

        /// <summary>
        /// Overrides the styling of the label to a custom class
        /// </summary>
        /// <param name="css">The CSS class to use</param>
        /// <returns>A label syntax element for chaining purposes</returns>
        INodeLabelSyntax<T> WithConditionalCss(Expression<Func<T, string>> css);

        /// <summary>
        /// Overrides the styling of the label to a fixed class
        /// </summary>
        /// <param name="css">The CSS class to use</param>
        /// <param name="condition">A condition when to apply the CSS class</param>
        /// <returns>A label syntax element for chaining purposes</returns>
        INodeLabelSyntax<T> WithConditionalCss(string css, Expression<Func<T, bool>> condition);
    }
}
