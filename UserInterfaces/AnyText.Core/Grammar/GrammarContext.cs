using NMF.AnyText.Rules;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NMF.AnyText.Grammar
{
    public class GrammarContext
    {
        private readonly IDictionary<Type, Rule> _rules;
        private readonly Dictionary<string, LiteralRule> _keywords = new Dictionary<string, LiteralRule>();

        public GrammarContext(IDictionary<Type, Rule> rules)
        {
            _rules = rules;
        }

        public T ResolveRule<T>() where T : Rule
        {
            if (_rules.TryGetValue(typeof(T), out var rule))
            {
                return rule as T;
            }
            return null;
        }

        public LiteralRule ResolveKeyword(string keyword)
        {
            if (!_keywords.TryGetValue(keyword, out var rule))
            {
                rule = new LiteralRule(keyword);
                _keywords.Add(keyword, rule);
            }
            return rule;
        }
    }
}
