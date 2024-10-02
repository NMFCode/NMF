using NMF.AnyText.Rules;
using NMF.Transformations;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NMF.AnyText.Grammar
{
    public abstract class ReflectiveGrammar
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
                var rules = Reflector.ReflectDictionary(GetGrammarTypeStack(), CreateDefaultRules, CreateCustomRules);
                _context = new GrammarContext(rules);
                foreach (var rule in rules.Values)
                {
                    rule.Initialize(_context);
                }
            }
        }

        private Stack<Type> GetGrammarTypeStack()
        {
            var typeStack = new Stack<Type>();
            var currentType = this.GetType();
            while (currentType != typeof(ReflectiveGrammar))
            {
                typeStack.Push(currentType);
                currentType = currentType.BaseType;
            }
            return typeStack;
        }

        protected virtual IEnumerable<Rule> CreateDefaultRules()
        {
            return null;
        }

        protected virtual IEnumerable<Rule> CreateCustomRules()
        {
            return null;
        }
    }
}
