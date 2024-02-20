using NMF.Glsp.Protocol.Notification;
using NMF.Glsp.Protocol.Validation;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Text.RegularExpressions;
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
        /// Overrides the setter of the label
        /// </summary>
        /// <param name="setter">A setter function or null to make the label readonly</param>
        /// <returns>A label syntax element for chaining purposes</returns>
        TSyntax WithSetter(Action<T, string> setter);
    }

    /// <summary>
    /// Denotes a basic label syntax
    /// </summary>
    /// <typeparam name="T"></typeparam>
    public interface ILabelSyntax<T> : ILabelSyntax<T, ILabelSyntax<T>> { }

    /// <summary>
    /// Provides convenience methods for <see cref="ILabelSyntax{T, TSyntax}"/>
    /// </summary>
    public static class LabelSyntaxExtensions
    {
        /// <summary>
        /// Registers a function to validate the label value
        /// </summary>
        /// <param name="syntax">The syntax element</param>
        /// <param name="validator">A function that validates inputs</param>
        /// <param name="message">The error message in case the clause evaluates to false</param>
        /// <returns>A label syntax element for chaining purposes</returns>
        public static TSyntax Validate<T, TSyntax>(this ILabelSyntax<T, TSyntax> syntax, Func<T, string, bool> validator, string message)
        {
            if (syntax == null) throw new ArgumentNullException(nameof(syntax));
            if (validator == null) throw new ArgumentNullException(nameof(validator));

            var error = new ValidationStatus { Message = message, Severity = SeverityLevels.Error };

            return syntax.Validate((el, txt) => validator(el, txt) ? ok : error);
        }

        /// <summary>
        /// Registers a function to validate the label value
        /// </summary>
        /// <param name="syntax">The syntax element</param>
        /// <param name="validator">A function that validates inputs</param>
        /// <param name="message">The error message in case the clause evaluates to false</param>
        /// <returns>A label syntax element for chaining purposes</returns>
        public static TSyntax Validate<T, TSyntax>(this ILabelSyntax<T, TSyntax> syntax, Regex validator, string message)
        {
            if (syntax == null) throw new ArgumentNullException(nameof(syntax));
            if (validator == null) throw new ArgumentNullException(nameof(validator));

            var error = new ValidationStatus { Message = message, Severity = SeverityLevels.Error };

            return syntax.Validate((_, txt) => validator.IsMatch(txt) ? ok : error);
        }

        private static readonly ValidationStatus ok = new() { Message = string.Empty, Severity = SeverityLevels.Ok };
    }
}
