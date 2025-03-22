using NMF.AnyText.Rules;
using NMF.Expressions;
using NMF.Models;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace NMF.AnyText.Model
{

    /// <summary>
    /// Denotes a rule that is used to create model elements
    /// </summary>
    /// <typeparam name="TElement">the type of elements to create</typeparam>
    public class ModelElementRule<TElement> : ElementRule<TElement>
        where TElement : IModelElement
    {
        private readonly List<ValidatorBase<TElement>> _validators = new List<ValidatorBase<TElement>>();

        /// <inheritdoc />
        protected override string GetReferenceString(TElement reference, ParseContext context)
        {
            return reference.ToIdentifierString();
        }

        /// <summary>
        /// Instructs the rule to validate elements using the given validator method, revalidating after every parse run
        /// </summary>
        /// <param name="validator">a function to validate elements</param>
        /// <param name="severity">the severity of errors</param>
        protected void Validate(Func<TElement, string> validator, DiagnosticSeverity severity = DiagnosticSeverity.Error)
        {
            ArgumentNullException.ThrowIfNull(validator);

            _validators.Add(new Revalidator<TElement>
            {
                Validator = validator,
                Severity = severity
            });
        }

        /// <summary>
        /// Instructs the rule to validate elements using the given validator method incrementally
        /// </summary>
        /// <param name="validatorExpression">a function to validate elements</param>
        /// <param name="severity">the severity of errors</param>
        protected void ValidateIncrementally(Expression<Func<TElement, string>> validatorExpression, DiagnosticSeverity severity = DiagnosticSeverity.Error)
        {
            ArgumentNullException.ThrowIfNull(validatorExpression);

            _validators.Add(new IncrementalValidator<TElement>
            {
                Validator = new ObservingFunc<TElement, string>(validatorExpression),
                Severity = severity
            });
        }

        /// <inheritdoc />
        protected override void Validate(TElement element, RuleApplication ruleApplication, ParseContext context)
        {
            foreach (var validator in _validators)
            {
                validator.Process(element, ruleApplication, context);
            }
        }
    }
}
