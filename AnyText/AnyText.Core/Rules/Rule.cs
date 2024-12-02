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
        /// True, if the rule permits trailing whitespaces, otherwise false
        /// </summary>
        public bool TrailingWhitespaces { get; protected internal set; } = true;

        /// <summary>
        /// Initializes the rule based on the provided grammar context
        /// </summary>
        /// <param name="context">the grammar context</param>
        public virtual void Initialize(GrammarContext context) { }

        /// <summary>
        /// Gets the token type of tokens created for this rule
        /// </summary>
        public virtual string TokenType => null;

        /// <summary>
        /// Indicates whether the rule is recursive
        /// </summary>
        public bool IsLeftRecursive { get; internal set; }

        /// <summary>
        /// Gets or sets formatting instructions for this rule
        /// </summary>
        public FormattingInstruction[] FormattingInstructions { get; set; }

        /// <summary>
        /// Determines whether the current rule can synthesize rule applications for the given semantic element
        /// </summary>
        /// <param name="semanticElement">the semantic element</param>
        /// <returns>true, if a rule application can be synthesized, otherwise false</returns>
        public abstract bool CanSynthesize(object semanticElement);

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
        public abstract bool IsEpsilonAllowed();

        /// <summary>
        /// Indicates whether the rule could start with the given other rule
        /// </summary>
        /// <param name="rule">the other rule</param>
        /// <returns>true, if the rule could start with the given other rule, otherwise false</returns>
        public abstract bool CanStartWith(Rule rule);

        /// <summary>
        /// Gets the token modifiers of
        /// </summary>
        public virtual string[] TokenModifiers => Array.Empty<string>();

        /// <summary>
        /// 
        /// </summary>
        /// <param name="ruleApplication"></param>
        /// <returns></returns>
        public virtual IEnumerable<string> SuggestCompletions(RuleApplication ruleApplication) => Enumerable.Empty<string>();

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
    }
}
