using NMF.AnyText.PrettyPrinting;
using NMF.AnyText.Rules;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;

namespace NMF.AnyText.Model
{
    /// <summary>
    /// Denotes a rule that is used to create models
    /// </summary>
    /// <typeparam name="T">the type of elements to create</typeparam>
    public class ModelElementRule<T> : SequenceRule
    {
        /// <summary>
        /// Synthesizes text for the given element
        /// </summary>
        /// <param name="element">the element for which text should be synthesized</param>
        /// <param name="context">the parse context</param>
        /// <param name="indentString">an indentation string. If none is provided, a double space is used as default.</param>
        /// <returns>the synthesized text or null, if no text can be synthesized</returns>
        public string Synthesize(T element, ParseContext context, string indentString = null)
        {
            ArgumentNullException.ThrowIfNull(element);

            var ruleApplication = Synthesize(element, default, context);
            if (!ruleApplication.IsPositive)
            {
                return null;
            }
            var writer = new StringWriter();
            var prettyWriter = new PrettyPrintWriter(writer, indentString ?? "  ");
            ruleApplication.Write(prettyWriter, context);
            return writer.ToString();
        }

        /// <summary>
        /// Creates an element
        /// </summary>
        /// <param name="inner">the inner rule applications</param>
        /// <returns>an instance of the model element type</returns>
        protected virtual T CreateElement(IEnumerable<RuleApplication> inner)
        {
            return (T)Activator.CreateInstance(typeof(T));
        }

        /// <inheritdoc />
        protected override RuleApplication CreateRuleApplication(ParsePosition currentPosition, List<RuleApplication> inner, ParsePositionDelta length, ParsePositionDelta examined)
        {
            return new ModelElementRuleApplication(this, currentPosition, inner, CreateElement(inner), length, examined);
        }

        /// <inheritdoc />
        public override bool CanSynthesize(object semanticElement)
        {
            return semanticElement is T;
        }

        /// <inheritdoc />
        public override RuleApplication Synthesize(object semanticElement, ParsePosition position, ParseContext context)
        {
            return base.Synthesize(new ParseObject(semanticElement), position, context);
        }

        private sealed class ModelElementRuleApplication : MultiRuleApplication
        {
            private readonly object _semanticElement;

            public ModelElementRuleApplication(Rule rule, ParsePosition currentPosition, List<RuleApplication> inner, object semanticElement, ParsePositionDelta endsAt, ParsePositionDelta examinedTo) : base(rule, currentPosition, inner, endsAt, examinedTo)
            {
                _semanticElement = semanticElement;
            }

            public override object ContextElement => _semanticElement;

            public override object GetValue(ParseContext context)
            {
                return _semanticElement;
            }
        }
    }
}
