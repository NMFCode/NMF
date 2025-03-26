using NMF.AnyText.Rules;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NMF.AnyText.Model
{
    /// <summary>
    /// Denotes the base class for a rule that resolves elements
    /// </summary>
    public abstract class ResolveRule<TSemanticElement, TReference> : QuoteRule
    {

        /// <summary>
        /// Resolves the given input
        /// </summary>
        /// <param name="contextElement">the element in the context of which the string is resolved</param>
        /// <param name="input">the textual reference</param>
        /// <param name="resolved">the resolved reference or the default</param>
        /// <param name="context">the context in which the element is resolved</param>
        /// <returns>true, if the reference could be resolved, otherwise false</returns>
        protected virtual bool TryResolveReference(TSemanticElement contextElement, string input, ParseContext context, out TReference resolved)
        {
            return context.TryResolveReference(contextElement, input, out resolved);
        }

        /// <summary>
        /// Gets the candidates to resolve the given input straing
        /// </summary>
        /// <param name="contextElement">the context element in which the element should be resolved</param>
        /// <param name="input">the input string</param>
        /// <param name="context">the parse context in which candidates are resolved</param>
        /// <returns>A collection of potential references</returns>
        /// <remarks>In case the parse tree was not successful, the context element may be of a different type than <typeparamref name="TSemanticElement"/>.</remarks>
        protected virtual IEnumerable<TReference> GetCandidates(object contextElement, string input, ParseContext context)
        {
            return context.GetPotentialReferences<TReference>(contextElement, input);
        }

        /// <summary>
        /// Gets the printed reference for the given object
        /// </summary>
        /// <param name="reference">the referenced object</param>
        /// <param name="context">the parse context</param>
        /// <param name="contextElement">the semantic context element</param>
        /// <returns>a string representation</returns>
        protected virtual string GetReferenceString(TReference reference, object contextElement, ParseContext context) => reference.ToString();

        /// <inheritdoc />
        public override IEnumerable<CompletionEntry> SuggestCompletions(ParseContext context, RuleApplication ruleApplication, ParsePosition position)
        {
            var restoredContext = context.RestoreContextElement(ruleApplication);
            if (ruleApplication.CurrentPosition.Line != position.Line)
            {
                return null;
            }
            var resolveString = string.Empty;
            if (ruleApplication.CurrentPosition.Col < position.Col && position.Line < context.Input.Length)
            {
                resolveString = context.Input[position.Line].Substring(ruleApplication.CurrentPosition.Col, position.Col - ruleApplication.CurrentPosition.Col);
            }
            return GetCandidates(restoredContext, resolveString, context).Select(r => new CompletionEntry(GetReferenceString(r, restoredContext, context), context.Grammar.GetSymbolKindForType(r.GetType()), position)); 
        }

        /// <inheritdoc />
        protected override RuleApplication CreateRuleApplication(RuleApplication app, ParseContext context)
        {
            return new ResolveRuleApplication(this, app, app.Length, app.ExaminedTo);
        }

        /// <inheritdoc />
        public override bool IsReference => true;

        internal class ResolveRuleApplication : SingleRuleApplication
        {
            public ResolveRuleApplication(Rule rule, RuleApplication inner, ParsePositionDelta endsAt, ParsePositionDelta examinedTo) : base(rule, inner, endsAt, examinedTo)
            {
            }

            public TReference Resolved { get; set; }

            public override object SemanticElement => Resolved;
        }

        /// <summary>
        /// Gets the delay level
        /// </summary>
        /// <remarks>Reference are resolved in layers, one after the other. </remarks>
        protected virtual byte ResolveDelayLevel => 0;

        /// <summary>
        /// Determines, whether the rule should attempt to resolve references directly when a rule application gets activated
        /// </summary>
        protected virtual bool TryResolveOnActivate => false;
    }
}
