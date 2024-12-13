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
        public override bool CanSynthesize(object semanticElement, ParseContext context)
        {
            return semanticElement is T && base.CanSynthesize(new ParseObject(semanticElement), context);
        }

        /// <inheritdoc />
        public override RuleApplication Synthesize(object semanticElement, ParsePosition position, ParseContext context)
        {
            var parseObject = new ParseObject(semanticElement);
            return SynthesizeParseObject(position, context, parseObject);
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
