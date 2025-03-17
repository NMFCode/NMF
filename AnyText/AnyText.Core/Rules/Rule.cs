using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using NMF.AnyText.Grammars;
using NMF.AnyText.PrettyPrinting;

namespace NMF.AnyText.Rules
{
    /// <summary>
    /// Denotes a rule for parsing rules
    /// </summary>
    public abstract class Rule
    {
        /// <summary>
        /// Matches the the context at the provided position
        /// </summary>
        /// <param name="context">the context in which the rule is matched</param>
        /// <param name="position">the position in the input</param>
        /// <returns>the rule application for the provided position</returns>
        public abstract RuleApplication Match(ParseContext context, ref ParsePosition position);

        /// <summary>
        /// Gets called when a rule application is activated
        /// </summary>
        /// <param name="application">the rule application that is activated</param>
        /// <param name="context">the context in which the rule application is activated</param>
        protected internal virtual void OnActivate(RuleApplication application, ParseContext context) { }

        /// <summary>
        /// Gets called when a rule application is deactivated
        /// </summary>
        /// <param name="application">the rule application that is deactivated</param>
        /// <param name="context">the context in which the rule application is deactivated</param>
        protected internal virtual void OnDeactivate(RuleApplication application, ParseContext context) { }

        /// <summary>
        /// Gets called when the value of a rule application changes
        /// </summary>
        /// <param name="application">the rule application for which the value changed</param>
        /// <param name="context">the context in which the value changed</param>
        /// <returns>true, if the rule processed the value change, otherwise false (in which case the value change is propagated)</returns>
        protected internal virtual bool OnValueChange(RuleApplication application, ParseContext context) => false;

        /// <summary>
        /// True, if the rule contributes characters, otherwise false
        /// </summary>
        public virtual bool IsLiteral => false;

        /// <summary>
        /// True, if the rule is ignored in the parse tree, otherwise false
        /// </summary>
        public virtual bool IsComment => false;

        /// <summary>
        /// True, if the application of this rule can be folded away (hidden)
        /// </summary>
        public virtual bool IsFoldable() => SymbolKind != SymbolKind.Null;

        /// <summary>
        /// True, if the rule is used to define imports
        /// </summary>
        public virtual bool IsImports() => false;

        /// <summary>
        /// Returns the folding kind for a rule if one is defined for the rule
        /// </summary>
        /// <param name="kind">The folding kind of the rule</param>
        /// <returns>True, if a folding kind is defined for the rule</returns>
        public virtual bool HasFoldingKind(out string kind)
        {
            kind = null;
            return false;
        }

        /// <summary>
        /// True, if the rule permits trailing whitespaces, otherwise false
        /// </summary>
        public bool TrailingWhitespaces { get; protected internal set; } = true;

        /// <summary>
        /// Initializes the rule based on the provided grammar context
        /// </summary>
        /// <param name="context">the grammar context</param>
        public virtual void Initialize(GrammarContext context) { }

        /// <summary>
        /// Initializes the rule based on the provided grammar context
        /// </summary>
        /// <param name="context">the grammar context</param>
        protected internal virtual void PostInitialize(GrammarContext context) { }

        /// <summary>
        /// Gets the token type of tokens created for this rule
        /// </summary>
        public virtual string TokenType => null;


        /// <summary>
        /// True, if the application of this rule denotes a definition
        /// </summary>
        public virtual bool IsDefinition => false;

        /// <summary>
        /// True, if the application of this rule denotes a reference
        /// </summary>
        public virtual bool IsReference => false;

        /// <summary>
        /// True, if the rule application of this rule denotes an identifier
        /// </summary>
        public virtual bool IsIdentifier => false;

        /// <summary>
        /// Gets the kind of document symbol to be used for this rule
        /// </summary>
        public virtual SymbolKind SymbolKind => SymbolKind.Null;

        /// <summary>
        /// Gets the kind of document symbol to be used for this rule
        /// </summary>
        public virtual SymbolTag[] SymbolTags => Enumerable.Empty<SymbolTag>().ToArray();

        /// <summary>
        /// True, if inner document symbols should be passed on to be handled separately,
        /// e.g. if the inner elements of the corresponding rule application should be
        /// visible in the outline, but not the rule application itself
        /// </summary>
        public virtual bool PassAlongDocumentSymbols => false;

        /// <summary>
        /// Indicates whether the rule is recursive
        /// </summary>
        public bool IsLeftRecursive { get; internal set; }

        /// <summary>
        /// Determines whether the current rule can synthesize rule applications for the given semantic element
        /// </summary>
        /// <param name="semanticElement">the semantic element</param>
        /// <param name="context">the context in which the rule is synthesized</param>
        /// <returns>true, if a rule application can be synthesized, otherwise false</returns>
        public abstract bool CanSynthesize(object semanticElement, ParseContext context);

        /// <summary>
        /// Creates a collection of requirements for synthesis
        /// </summary>
        /// <returns>A collection of synthesis requirements</returns>
        public virtual IEnumerable<SynthesisRequirement> CreateSynthesisRequirements() => Enumerable.Empty<SynthesisRequirement>();

        internal virtual void Write(PrettyPrintWriter writer, ParseContext context, MultiRuleApplication ruleApplication) => throw new NotSupportedException("Cannot write a multi rule");

        internal virtual void Write(PrettyPrintWriter writer, ParseContext context, SingleRuleApplication ruleApplication) => throw new NotSupportedException("Cannot write a multi rule");

        /// <summary>
        /// Synthesizes text for the given element
        /// </summary>
        /// <param name="element">the element for which text should be synthesized</param>
        /// <param name="context">the parse context</param>
        /// <param name="indentString">an indentation string. If none is provided, a double space is used as default.</param>
        /// <returns>the synthesized text or null, if no text can be synthesized</returns>
        public string Synthesize(object element, ParseContext context, string indentString = null)
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
        /// Synthesizes text for the given element
        /// </summary>
        /// <param name="element">the element for which text should be synthesized</param>
        /// <param name="context">the parse context</param>
        /// <param name="writer">the text writer the synthesized text should be written to</param>
        /// <param name="indentString">an indentation string. If none is provided, a double space is used as default.</param>
        /// <returns>the synthesized text or null, if no text can be synthesized</returns>
        public void Synthesize(object element, ParseContext context, TextWriter writer, string indentString = null)
        {
            ArgumentNullException.ThrowIfNull(element);

            var ruleApplication = Synthesize(element, default, context);
            if (!ruleApplication.IsPositive)
            {
                return;
            }
            var prettyWriter = new PrettyPrintWriter(writer, indentString ?? "  ");
            ruleApplication.Write(prettyWriter, context);
        }

        /// <summary>
        /// Synthesizes a rule application for the given semantic element
        /// </summary>
        /// <param name="semanticElement"></param>
        /// <param name="position">the parse position at which the element should be synthesized</param>
        /// <param name="context">the parse context</param>
        /// <returns>a rule application</returns>
        public abstract RuleApplication Synthesize(object semanticElement, ParsePosition position, ParseContext context);

        /// <summary>
        /// Determines whether the rule could capture empty input
        /// </summary>
        /// <returns>true, if the rule can be expanded to an empty string, otherwise false</returns>
        public bool IsEpsilonAllowed() => IsEpsilonAllowed(new List<Rule>());

        /// <summary>
        /// Determines whether the rule could capture empty input
        /// </summary>
        /// <returns>true, if the rule can be expanded to an empty string, otherwise false</returns>
        protected internal abstract bool IsEpsilonAllowed(List<Rule> trace);

        /// <summary>
        /// Indicates whether the rule could start with the given other rule
        /// </summary>
        /// <param name="rule">the other rule</param>
        /// <returns>true, if the rule could start with the given other rule, otherwise false</returns>
        public bool CanStartWith(Rule rule) => CanStartWith(rule, new List<Rule>());

        /// <summary>
        /// Indicates whether the rule could start with the given other rule
        /// </summary>
        /// <param name="rule">the other rule</param>
        /// <param name="trace">a list of rules visited so far</param>
        /// <returns>true, if the rule could start with the given other rule, otherwise false</returns>
        protected internal abstract bool CanStartWith(Rule rule, List<Rule> trace);

        /// <summary>
        /// Gets the token modifiers of
        /// </summary>
        public virtual string[] TokenModifiers => Array.Empty<string>();

        /// <summary>
        /// Suggests useful code completions
        /// </summary>
#pragma warning disable S1168 // Empty arrays and collections should be returned instead of null
        public virtual IEnumerable<string> SuggestCompletions(ParseContext context, RuleApplication ruleApplication, ParsePosition position) => null;
#pragma warning restore S1168 // Empty arrays and collections should be returned instead of null

        /// <summary>
        /// Gets the index of the token type
        /// </summary>
        public uint? TokenTypeIndex
        {
            get;
            internal set;
        }
        
        /// <summary>
        /// Gets the index of the token modifiers
        /// </summary>
        public uint? TokenModifierIndex
        {
            get;
            internal set;
        }
        
        /// <summary>
        /// Gets the list of code actions for this rule.
        /// </summary>
        internal virtual IEnumerable<CodeActionInfo> SupportedCodeActions => Enumerable.Empty<CodeActionInfo>();
        
        /// <summary>
        /// Gets the list of code lenses for this rule.
        /// </summary>
        internal virtual IEnumerable<CodeLensInfo> SupportedCodeLenses => Enumerable.Empty<CodeLensInfo>();

        /// <summary>
        /// Gibt den Hover-Text für diese Rule zurück, wenn definiert.
        /// </summary>
        /// <param name="context">Der Kontext, in dem die Rule verarbeitet wird.</param>
        /// <param name="position">Die Position, an der der Hover-Text angefordert wird.</param>
        /// <returns>Der Hover-Text oder null, wenn keiner definiert ist.</returns>
        public virtual string GetHoverText(RuleApplication ruleApplication, Parser document, ParsePosition position)
        {
            var documentSymbols = document.GetDocumentSymbolsFromRoot();
            if (documentSymbols == null || !documentSymbols.Any())
            {
                return null;
            }

            var matchingSymbol = FindSymbolAtPosition(documentSymbols, position);
            if (matchingSymbol == null)
            {
                return null;
            }

            string symbolType = matchingSymbol.Kind.ToString();

            var sb = new StringBuilder();
            sb.AppendLine($"**{matchingSymbol.Name}** ({symbolType})");

            if (!string.IsNullOrWhiteSpace(matchingSymbol.Detail))
            {
                sb.AppendLine($"\n```{document.Context.Grammar.LanguageId}\n{matchingSymbol.Detail}\n```");
            }

            if (matchingSymbol.Tags != null && matchingSymbol.Tags.Length > 0)
            {
                sb.AppendLine("\n**Tags:**");
                foreach (var tag in matchingSymbol.Tags)
                {
                    sb.AppendLine($"- {tag}");
                }
            }

            return sb.ToString();

            DocumentSymbol FindSymbolAtPosition(IEnumerable<DocumentSymbol> symbols, ParsePosition position)
            {
                foreach (var symbol in symbols)
                {
                    var childSymbol = symbol.Children != null ? FindSymbolAtPosition(symbol.Children, position) : null;
                    if (childSymbol != null)
                    {
                        return childSymbol;
                    }

                    if (IsExactPosition(position, symbol.Range))
                    {
                        return symbol;
                    }
                }
                return null;
            }

            bool IsExactPosition(ParsePosition position, ParseRange range)
            {
                return position.Line == range.Start.Line && position.Col >= range.Start.Col;
            }
        }

    }
}
