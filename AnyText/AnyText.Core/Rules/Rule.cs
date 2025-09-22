using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
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
        /// Gets or sets the name of this rule
        /// </summary>
        public string Name { get; set; }

        /// <inheritdoc />
        public override string ToString()
        {
            if (string.IsNullOrEmpty(Name))
            {
                return GetType().Name;
            }
            return Name;
        }

        /// <summary>
        /// Matches the the context at the provided position
        /// </summary>
        /// <param name="context">the context in which the rule is matched</param>
        /// <param name="recursionContext">the recursion context of the matching</param>
        /// <param name="position">the position in the input</param>
        /// <returns>the rule application for the provided position</returns>
        public abstract RuleApplication Match(ParseContext context, RecursionContext recursionContext, ref ParsePosition position);

        internal virtual MatchOrMatchProcessor NextMatchProcessor(ParseContext context, RecursionContext recursionContext, ref ParsePosition position)
        {
            return new MatchOrMatchProcessor(Match(context, recursionContext, ref position));
        }

        /// <summary>
        /// Gets called when a rule application is activated
        /// </summary>
        /// <param name="application">the rule application that is activated</param>
        /// <param name="context">the context in which the rule application is activated</param>
        /// <param name="initial">flag indicating whether the activation happened due to initial parse</param>
        protected internal virtual void OnActivate(RuleApplication application, ParseContext context, bool initial) { }

        /// <summary>
        /// Gets called when a rule application is deactivated
        /// </summary>
        /// <param name="application">the rule application that is deactivated</param>
        /// <param name="context">the context in which the rule application is deactivated</param>
        protected internal virtual void OnDeactivate(RuleApplication application, ParseContext context) { }

        /// <summary>
        /// Recovers from the given inner failed rule application
        /// </summary>
        /// <param name="ruleApplication">the rule application that should be recovered</param>
        /// <param name="failedRuleApplication">the inner rule application that caused the failure</param>
        /// <param name="currentRoot">the current root rule application</param>
        /// <param name="context">the parse context in which the rule application is recovered</param>
        /// <param name="position">the position after the recovery</param>
        /// <returns>the recovered rule application if the recovery was successful or the old rule application if not</returns>
        protected internal virtual RuleApplication Recover(RuleApplication ruleApplication, RuleApplication failedRuleApplication, RuleApplication currentRoot, ParseContext context, out ParsePosition position)
        {
            position = ruleApplication.CurrentPosition;
            return ruleApplication;
        }

        /// <summary>
        /// Gets called when the value of a rule application changes
        /// </summary>
        /// <param name="application">the rule application for which the value changed</param>
        /// <param name="context">the context in which the value changed</param>
        /// <param name="oldRuleApplication">the old rule application</param>
        /// <returns>true, if the rule processed the value change, otherwise false (in which case the value change is propagated)</returns>
        protected internal virtual bool OnValueChange(RuleApplication application, ParseContext context, RuleApplication oldRuleApplication) => false;

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
        public virtual SymbolTag[] SymbolTags(RuleApplication ruleApplication) => Array.Empty<SymbolTag>();

        /// <summary>
        /// True, if inner document symbols should be passed on to be handled separately,
        /// e.g. if the inner elements of the corresponding rule application should be
        /// visible in the outline, but not the rule application itself
        /// </summary>
        public bool PassAlongDocumentSymbols => SymbolKind == SymbolKind.Null;

        /// <summary>
        /// Invalidates the given rule application, checking for potential errors
        /// </summary>
        /// <param name="ruleApplication">the rule application to invalidate</param>
        /// <param name="context">the parse context in which the rule application is invalidated</param>
        public virtual void Invalidate(RuleApplication ruleApplication, ParseContext context) { }

        /// <summary>
        /// Indicates whether the rule is recursive
        /// </summary>
        public bool IsLeftRecursive { get; internal set; }

        /// <summary>
        /// Gets a collection of recursive continuations for this rule
        /// </summary>
        public IReadOnlyCollection<RecursiveContinuation> Continuations { get; internal set; }

        /// <summary>
        /// Determines whether the current rule can synthesize rule applications for the given semantic element
        /// </summary>
        /// <param name="semanticElement">the semantic element</param>
        /// <param name="context">the context in which the rule is synthesized</param>
        /// <returns>true, if a rule application can be synthesized, otherwise false</returns>
        public bool CanSynthesize(object semanticElement, ParseContext context) => CanSynthesize(semanticElement, context, null);

        /// <summary>
        /// Determines whether the current rule can synthesize rule applications for the given semantic element
        /// </summary>
        /// <param name="semanticElement">the semantic element</param>
        /// <param name="context">the context in which the rule is synthesized</param>
        /// <param name="synthesisPlan">the plan of the synthesis</param>
        /// <returns>true, if a rule application can be synthesized, otherwise false</returns>
        public abstract bool CanSynthesize(object semanticElement, ParseContext context, SynthesisPlan synthesisPlan);

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
        /// Creates a new rule application at the given position
        /// </summary>
        /// <param name="position">another rule application at the desired position</param>
        /// <returns>A new rule application</returns>
        public virtual RuleApplication CreateEpsilonRuleApplication(RuleApplication position) => throw new NotSupportedException();

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
        /// Adds rules to the given left recursion trace
        /// </summary>
        /// <param name="trace">the trace</param>
        /// <param name="continuations">a collection of recursion continuations</param>
        protected internal virtual void AddLeftRecursionRules(List<Rule> trace, List<RecursiveContinuation> continuations) { }

        /// <summary>
        /// Gets the token modifiers of
        /// </summary>
        public virtual string[] TokenModifiers => Array.Empty<string>();
        
        /// <summary>
        /// Calculates an inlay text that should be shown in front of this rule application
        /// </summary>
        /// <param name="ruleApplication">the rule application</param>
        /// <param name="context">the parse context in which the inlay hint is queried</param>
        /// <returns>An inlay entry</returns>
        public virtual InlayEntry GetInlayHintText(RuleApplication ruleApplication, ParseContext context) => null;

        /// <summary>
        /// Suggests useful code completions
        /// </summary>
        /// <param name="context">the parse context</param>
        /// <param name="fragment">the fragment available</param>
        /// <param name="position">the exact position</param>
        /// <param name="ruleApplication">the rule application for which a completion was queried</param>
        public virtual IEnumerable<CompletionEntry> SuggestCompletions(ParseContext context, RuleApplication ruleApplication, string fragment, ParsePosition position) => null;

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
        /// Retrieves the hover text for a symbol at a given position in the document, if available.
        /// </summary>
        /// <param name="ruleApplication">The rule application context that is used to process the document.</param>
        /// <param name="document">The document being parsed.</param>
        /// <param name="position">The position in the document where the hover text is requested.</param>
        /// <returns>
        /// A string containing the hover text, or null if no matching symbol or hover text is found.
        /// </returns>
        public virtual string GetHoverText(RuleApplication ruleApplication, Parser document, ParsePosition position)
        {
            if (ruleApplication.Parent != null)
            {
                return ruleApplication.Parent.Rule.GetHoverText(ruleApplication.Parent, document, position);
            }
            return null;
        }

        /// <summary>
        /// Registers the correct symbol kind for the reference type if any.
        /// </summary>
        public virtual void RegisterSymbolKind(Dictionary<Type, SymbolKind> _symbolKinds) { }
    }
}
