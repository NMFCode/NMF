using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using NMF.AnyText.Rules;

namespace NMF.AnyText.Grammars
{
    public abstract class Grammar
    {
        private GrammarContext _context;

        public T GetRule<T>() where T : Rule
        {
            Initialize();
            return _context?.ResolveRule<T>();
        }

        public void Initialize()
        {
            if (_context == null)
            {
                var rules = CreatesTypedRules();
                _context = new GrammarContext(rules);
                var tokenTypes = new List<string>();
                var tokenModifiers = new List<string>();
                IEnumerable<Rule> allRules = rules.Values;
                var others = CreateCustomRules();
                if (others != null)
                {
                    allRules = allRules.Concat(others);
                }
                foreach (var rule in allRules)
                {
                    rule.Initialize(_context);
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
            }
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

        protected abstract IDictionary<Type, Rule> CreatesTypedRules();

        protected abstract IEnumerable<Rule> CreateCustomRules();

        public Parser CreateParser()
        {
            return new Parser(CreateParseContext());
        }

        public abstract string LanguageId { get; }

        protected internal virtual ParseContext CreateParseContext() => new ParseContext(this, new Matcher());

        public Rule Root
        {
            get
            {
                Initialize();
                return GetRootRule(_context);
            }
        }

        public string[] TokenTypes { get; private set; }

        public string[] TokenModifiers { get; private set; }

        protected abstract Rule GetRootRule(GrammarContext context);
    }
}
