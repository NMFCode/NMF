using NMF.AnyText.Rules;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NMF.AnyText.Grammars
{
    /// <summary>
    /// Wraps a rule (with pre-initialized dependencies) into a grammar
    /// </summary>
    public class AdHocGrammar : Grammar
    {
        private readonly Rule _root;
        private readonly List<Rule> _furtherRules;

        /// <summary>
        /// Creates a new instance
        /// </summary>
        /// <param name="root">the rule that should be wrapped</param>
        /// <param name="furtherRules">other rules</param>
        public AdHocGrammar(Rule root, IEnumerable<Rule> furtherRules)
        {
            _root = root;
            _furtherRules = new List<Rule>(furtherRules);
        }

        /// <inheritdoc/>
        public override string LanguageId => null;

        /// <inheritdoc/>
        protected override IEnumerable<Rule> CreateCustomRules()
        {
            return _furtherRules;
        }

        /// <inheritdoc/>
        protected override IDictionary<Type, Rule> CreateTypedRules()
        {
            return new Dictionary<Type, Rule>() { [typeof(Rule)] = _root };
        }

        /// <inheritdoc/>
        protected override Rule GetRootRule(GrammarContext context)
        {
            return _root;
        }

        /// <summary>
        /// Gets a collection of all rules
        /// </summary>
        public IEnumerable<Rule> Rules => Enumerable.Repeat(_root, 1).Concat(_furtherRules);
    }
}
