using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using NMF.AnyText.Rules;

namespace NMF.AnyText.Grammars
{
    /// <summary>
    /// Denotes an abstract grammar
    /// </summary>
    public abstract class Grammar
    {
        private GrammarContext _context;
        private CommentRule[] _commentRules;

        /// <summary>
        /// Gets the rule with the given rule type
        /// </summary>
        /// <typeparam name="T">the type of the rule</typeparam>
        /// <returns>the rule with the provided type, if it exists and registered for this type</returns>
        public T GetRule<T>() where T : Rule
        {
            Initialize();
            return _context?.ResolveRule<T>();
        }

        /// <summary>
        /// Gets a collection of all rules in this grammar
        /// </summary>
        public IEnumerable<Rule> Rules => _context.Rules;

        /// <summary>
        /// Gets the keywords used in the grammar
        /// </summary>
        public ICollection<LiteralRule> Keywords => _context.Keywords;

        /// <summary>
        /// Initializes the current grammar
        /// </summary>
        public void Initialize()
        {
            if (_context == null)
            {
                var rules = CreateTypedRules();
                _context = new GrammarContext(rules, this);
                var tokenTypes = new List<string>();
                var tokenModifiers = new List<string>();
                IEnumerable<Rule> allRules = _context.Rules;
                var others = CreateCustomRules();
                if (others != null)
                {
                    allRules = allRules.Concat(others);
                }
                foreach (var rule in allRules)
                {
                    rule.Initialize(_context);
                    
                    AddActionsFromRule(rule);

                    if (rule.TokenType != null)
                    {
                        CalculateTokenIndices(tokenTypes, tokenModifiers, rule, out int tokenTypeIndex, out int tokenModifierIndex);
                        rule.TokenTypeIndex = (uint)tokenTypeIndex;
                        rule.TokenModifierIndex = (uint)tokenModifierIndex;
                    }
                }
                foreach (var rule in allRules)
                {
                    rule.IsLeftRecursive = rule.CanStartWith(rule);
                }
                TokenModifiers = tokenModifiers.ToArray();
                TokenTypes = tokenTypes.ToArray();
                CommentRules = allRules.OfType<CommentRule>().ToArray();
            }
        }

        private void AddActionsFromRule(Rule rule)
        {
            if (rule.SupportedCodeActions.Any())
                foreach (var actionInfo in rule.SupportedCodeActions)
                    if (!string.IsNullOrEmpty(actionInfo.CommandIdentifier))
                        ExecutableActions.TryAdd(actionInfo.CommandIdentifier, actionInfo);

            if (rule.SupportedCodeLenses.Any())
                foreach (var lensInfo in rule.SupportedCodeLenses)
                    ExecutableActions.TryAdd(lensInfo.CommandIdentifier, lensInfo);
        }

        /// <summary>
        /// Gets an array of comment rules used by this grammar
        /// </summary>
        public CommentRule[] CommentRules
        {
            get
            {
                Initialize();
                return _commentRules;
            }
            private set { _commentRules = value; }
        }

        /// <summary>
        /// Creates the keyword rule for the given keyword
        /// </summary>
        /// <param name="keyword">the keyword</param>
        /// <returns>A literal rule that represents matching the provided keyword</returns>
        protected internal virtual LiteralRule CreateKeywordRule(string keyword)
        {
            return new LiteralRule(keyword);
        }

        private static void CalculateTokenIndices(List<string> tokenTypes, List<string> tokenModifiers, Rule rule, out int tokenTypeIndex, out int tokenModifierIndex)
        {
            tokenTypeIndex = tokenTypes.IndexOf(rule.TokenType);
            if (tokenTypeIndex == -1)
            {
                tokenTypeIndex = tokenTypes.Count;
                tokenTypes.Add(rule.TokenType);
            }
            tokenModifierIndex = 0;
            foreach (var modifier in rule.TokenModifiers)
            {
                var modifierIndex = tokenModifiers.IndexOf(modifier);
                if (modifierIndex == -1)
                {
                    modifierIndex = tokenModifiers.Count;
                    tokenModifiers.Add(modifier);
                }
                tokenModifierIndex |= 1 << modifierIndex;
            }
        }

        /// <summary>
        /// Create the typed rules as a dictionary
        /// </summary>
        /// <returns>a dictionary of rules by type</returns>
        protected abstract IDictionary<Type, Rule> CreateTypedRules();

        /// <summary>
        /// Create custom rules that are not resolvable by type
        /// </summary>
        /// <returns>a collection of custom rules</returns>
        protected abstract IEnumerable<Rule> CreateCustomRules();

        /// <summary>
        /// Create a parser for this grammar
        /// </summary>
        /// <returns>a parser configured for this grammar</returns>
        public Parser CreateParser()
        {
            return new Parser(CreateParseContext());
        }

        /// <summary>
        /// Gets the language id for this grammar
        /// </summary>
        public abstract string LanguageId { get; }

        /// <summary>
        /// Creates a parsing context for this grammar
        /// </summary>
        /// <returns>a parsing context for the current grammar</returns>
        protected internal virtual ParseContext CreateParseContext() => new ParseContext(this, new Matcher(CommentRules));

        /// <summary>
        /// Gets the root rule
        /// </summary>
        public Rule Root
        {
            get
            {
                Initialize();
                return GetRootRule(_context);
            }
        }

        /// <summary>
        /// Gets an array of token types used by this grammar
        /// </summary>
        public string[] TokenTypes { get; private set; }

        /// <summary>
        /// Gets an array of token modifiers used by this grammar
        /// </summary>
        public string[] TokenModifiers { get; private set; }

        /// <summary>
        /// Gets the root rule
        /// </summary>
        /// <param name="context">a context to resolve the root rule</param>
        /// <returns>the root rule for this grammar</returns>
        protected abstract Rule GetRootRule(GrammarContext context);
        
        /// <summary>
        /// Dictionary of executable actions.
        /// The key is the action identifier, and the value is the action executor.
        /// </summary>
        public Dictionary<string, ActionInfo> ExecutableActions { get; } = new ();
    }
}
