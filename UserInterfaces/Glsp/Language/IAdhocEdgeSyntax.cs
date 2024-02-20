using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NMF.Glsp.Language
{
    /// <summary>
    /// Denotes an interface for syntax provided for ad-hoc edges
    /// </summary>
    public interface IAdhocEdgeSyntax : IChildSyntax<IAdhocEdgeSyntax>
    {
        /// <summary>
        /// Sets the label for the generated edge to the given value provider
        /// </summary>
        /// <param name="label">A function returning the (optionally localized) tool label</param>
        /// <returns>A syntax instance for chaining purposes</returns>
        IAdhocEdgeSyntax WithLabel(Func<string> label);

        /// <summary>
        /// Customizes the type with which the edges are rendered
        /// </summary>
        /// <param name="type">The type string</param>
        /// <returns>A syntax instance for chaining purposes</returns>
        IAdhocEdgeSyntax WithType(string type);
    }

    /// <summary>
    /// Provides convenience methods for <see cref="IAdhocEdgeSyntax"/>
    /// </summary>
    public static class AdhocEdgeSyntaxExtensions
    {
        /// <summary>
        /// Sets the label for the generated edge to the given value provider
        /// </summary>
        /// <param name="syntax">The syntax instance</param>
        /// <param name="label">The tool label</param>
        /// <returns>A syntax instance for chaining purposes</returns>
        public static IAdhocEdgeSyntax WithLabel(this IAdhocEdgeSyntax syntax, string label)
        {
            return syntax.WithLabel(() => label);
        }
    }
}
