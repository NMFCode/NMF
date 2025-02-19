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
    /// <typeparam name="TReference">the type of elements to create</typeparam>
    public abstract class ElementRule<TReference> : SequenceRule
    {
        /// <inheritdoc />
        protected internal override void OnActivate(RuleApplication application, ParseContext context)
        {
            var identifier = application.GetIdentifier();
            if (identifier != null )
            {
                context.AddReference(application.SemanticElement, identifier);
                context.AddDefinition(application.SemanticElement, identifier);
            }
            else
            {
                context.AddDefinition(application.SemanticElement, application);
            }
        }

        /// <inheritdoc />
        protected internal override void OnDeactivate(RuleApplication application, ParseContext context)
        {
            context.RemoveDefinition(application.SemanticElement);
            var identifier = application.GetIdentifier();
            if (identifier != null)
            {
                context.RemoveReference(application.SemanticElement, identifier);
            }
        }

        /// <summary>
        /// Creates an element
        /// </summary>
        /// <param name="inner">the inner rule applications</param>
        /// <returns>an instance of the model element type</returns>
        protected virtual TReference CreateElement(IEnumerable<RuleApplication> inner)
        {
            return (TReference)Activator.CreateInstance(typeof(TReference));
        }

        /// <inheritdoc />
        protected override RuleApplication CreateRuleApplication(ParsePosition currentPosition, List<RuleApplication> inner, ParsePositionDelta length, ParsePositionDelta examined)
        {
            return new ModelElementRuleApplication(this, currentPosition, inner, CreateElement(inner), length, examined);
        }

        /// <summary>
        /// Gets the printed reference for the given object
        /// </summary>
        /// <param name="reference">the referenced object</param>
        /// <param name="context">the parse context</param>
        /// <returns>a string representation</returns>
        protected virtual string GetReferenceString(TReference reference, ParseContext context) => reference.ToString();

        /// <inheritdoc />
        public override bool CanSynthesize(object semanticElement, ParseContext context)
        {
            return semanticElement is TReference && base.CanSynthesize(new ParseObject(semanticElement), context);
        }

        /// <inheritdoc />
        public override RuleApplication Synthesize(object semanticElement, ParsePosition position, ParseContext context)
        {
            var parseObject = new ParseObject(semanticElement);
            return SynthesizeParseObject(position, context, parseObject);
        }

        /// <summary>
        /// Gets the list of code lenses for this rule.
        /// </summary>
        protected virtual IEnumerable<CodeLensInfo<TReference>> CodeLenses
        {
            get
            {
                if (SymbolKind == SymbolKind.Null)
                    yield break;
                yield return new CodeLensInfo<TReference>
                {
                    Title = "Reference",
                    CommandIdentifier = "codelens.reference." + typeof(TReference).Name,
                    Action = (f, args) => { },
                    TitleFunc = (modelRule, context) =>
                    {
                        var referenceCount = 1;
                        if (context.TryGetReferences(modelRule, out var references))
                        {
                            referenceCount = references.Count;
                        }
                        return referenceCount == 1 ? "No References" : $"{referenceCount - 1} References";
                    }
                };
            }
        }


        /// <summary>
        /// Gets the list of code actions for this rule.
        /// </summary>
        protected virtual IEnumerable<CodeActionInfo<TReference>> CodeActions => Enumerable.Empty<CodeActionInfo<TReference>>();

        internal override IEnumerable<CodeActionInfo> SupportedCodeActions => CodeActions;

        internal override IEnumerable<CodeLensInfo> SupportedCodeLenses => CodeLenses;

        /// <inheritdoc />
        public override bool IsDefinition => true;

        /// <inheritdoc />
        public override SymbolKind SymbolKind => SymbolKind.Null;

        private sealed class ModelElementRuleApplication : MultiRuleApplication
        {
            private readonly object _semanticElement;

            public ModelElementRuleApplication(Rule rule, ParsePosition currentPosition, List<RuleApplication> inner, object semanticElement, ParsePositionDelta endsAt, ParsePositionDelta examinedTo) : base(rule, currentPosition, inner, endsAt, examinedTo)
            {
                _semanticElement = semanticElement;
            }

            public override object ContextElement => _semanticElement;

            public override object SemanticElement => _semanticElement;

            public override object GetValue(ParseContext context)
            {
                return _semanticElement;
            }
        }
    }
}
