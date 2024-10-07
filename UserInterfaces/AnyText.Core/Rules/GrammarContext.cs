using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NMF.AnyText.Rules
{
    /// <summary>
    /// Denotes the context in which a rule is initialized
    /// </summary>
    public class GrammarContext
    {
        private readonly IDictionary<Type, Rule> _rules;
        private readonly Dictionary<string, LiteralRule> _keywords = new Dictionary<string, LiteralRule>();

        /// <summary>
        /// Creates a new instance
        /// </summary>
        /// <param name="rules">a dictionary of rules that should be used in this context</param>
        public GrammarContext(IDictionary<Type, Rule> rules)
        {
            _rules = rules;
        }

        /// <summary>
        /// Resolves the grammar rule of the given type
        /// </summary>
        /// <typeparam name="T">the rule type</typeparam>
        /// <returns>the rule instance with the given rule type</returns>
        public T ResolveRule<T>() where T : Rule
        {
            if (_rules.TryGetValue(typeof(T), out var rule))
            {
                return rule as T;
            }
            return null;
        }

        /// <summary>
        /// Resolves the rule for the given keyword
        /// </summary>
        /// <param name="keyword">the keyword</param>
        /// <returns>a literal rule that represents matching the provided keyword</returns>
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
