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
    /// Denotes a rule that is used to create elements
    /// </summary>
    /// <typeparam name="TElement">the type of elements to create</typeparam>
    public abstract class ElementRule<TElement> : SequenceRule
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
        protected virtual TElement CreateElement(IEnumerable<RuleApplication> inner)
        {
            return (TElement)Activator.CreateInstance(typeof(TElement));
        }

        /// <inheritdoc />
        protected override RuleApplication CreateRuleApplication(ParsePosition currentPosition, List<RuleApplication> inner, ParsePositionDelta length, ParsePositionDelta examined)
        {
            return new ModelElementRuleApplication(this, inner, CreateElement(inner), length, examined);
        }

        /// <summary>
        /// Gets the printed reference for the given object
        /// </summary>
        /// <param name="reference">the referenced object</param>
        /// <param name="context">the parse context</param>
        /// <returns>a string representation</returns>
        protected virtual string GetReferenceString(TElement reference, ParseContext context) => reference.ToString();

        /// <inheritdoc />
        public override bool CanSynthesize(object semanticElement, ParseContext context)
        {
            return semanticElement is TElement && base.CanSynthesize(new ParseObject(semanticElement), context);
        }

        /// <summary>
        /// Validates the given semantic element
        /// </summary>
        /// <param name="element">the element that should be validated</param>
        /// <param name="context">the context in which validation is performed</param>
        /// <param name="ruleApplication">the rule application in the context of which validation is performed</param>
        protected virtual void Validate(TElement element, RuleApplication ruleApplication, ParseContext context)
        {
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
        protected virtual IEnumerable<CodeLensInfo<TElement>> CodeLenses
        {
            get
            {
                if (SymbolKind == SymbolKind.Null)
                    yield break;
                yield return new CodeLensInfo<TElement>
                {
                    Title = "Reference",
                    CommandIdentifier = "codelens.reference." + typeof(TElement).Name,
                    Action = (f, args) =>
                    {
                        if (args.Context.TryGetDefinition(f, out var definition))
                        {
                            args.ShowReferences(definition.CurrentPosition);
                        }
                    },
                    TitleFunc = (modelRule, context) =>
                    {
                        var referenceCount = 1;
                        if (context.TryGetReferences(modelRule, out var references))
                        {
                            referenceCount = references.Count;
                        }
                        return $"{referenceCount - 1} references";
                    },
                };
            }
        }

        /// <summary>
        /// Gets the list of code actions for this rule.
        /// </summary>
        protected virtual IEnumerable<CodeActionInfo<TElement>> CodeActions => Enumerable.Empty<CodeActionInfo<TElement>>();

        internal override IEnumerable<CodeActionInfo> SupportedCodeActions => CodeActions;

        internal override IEnumerable<CodeLensInfo> SupportedCodeLenses => CodeLenses;

        /// <inheritdoc />
        public override bool IsDefinition => true;

        private sealed class ModelElementRuleApplication : MultiRuleApplication
        {
            private readonly object _semanticElement;

            public ModelElementRuleApplication(Rule rule, List<RuleApplication> inner, object semanticElement, ParsePositionDelta endsAt, ParsePositionDelta examinedTo) : base(rule, inner, endsAt, examinedTo)
            {
                _semanticElement = semanticElement;
            }

            public override object ContextElement => _semanticElement;

            public override object SemanticElement => _semanticElement;

            public override object GetValue(ParseContext context)
            {
                return _semanticElement;
            }

            public override void Validate(ParseContext context)
            {
                if (Rule is ElementRule<TElement> elementRule && _semanticElement is TElement element)
                {
                    elementRule.Validate(element, this, context);
                }
                base.Validate(context);
            }
        }

        /// <inheritdoc/>
        public override void RegisterSymbolKind(Dictionary<Type, SymbolKind> _symbolKinds)
        {
            var key = typeof(TElement);

            if (!_symbolKinds.ContainsKey(key))
            {
                _symbolKinds.Add(key, this.SymbolKind);
            }
        }
    }
}
