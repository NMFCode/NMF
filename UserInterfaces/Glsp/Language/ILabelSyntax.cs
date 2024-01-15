using NMF.Glsp.Protocol.Validation;
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
    /// <typeparam name="TSyntax">The syntax type</typeparam>
    public interface ILabelSyntax<T, TSyntax>
    {
        /// <summary>
        /// Adds a condition for when the label is generated
        /// </summary>
        /// <param name="guard">A function expression expressing a guard condition</param>
        /// <returns>A label syntax element for chaining purposes</returns>
        TSyntax If(Expression<Func<T, bool>> guard);

        /// <summary>
        /// Overrides the GLSP type created for the label
        /// </summary>
        /// <param name="type">The GLSP type for the label</param>
        /// <returns>A label syntax element for chaining purposes</returns>
        TSyntax WithType(string type);

        /// <summary>
        /// Registers a function to validate the label value
        /// </summary>
        /// <param name="validator">A function that validates inputs</param>
        /// <returns>A label syntax element for chaining purposes</returns>
        TSyntax Validate(Func<T, string, ValidationStatus> validator);

        /// <summary>
        /// Registers a function to validate the label value
        /// </summary>
        /// <param name="validator">A function that validates inputs</param>
        /// <param name="message">The error message in case the clause evaluates to false</param>
        /// <returns>A label syntax element for chaining purposes</returns>
        TSyntax Validate(Func<T, string, bool> validator, string message);
    }
}
