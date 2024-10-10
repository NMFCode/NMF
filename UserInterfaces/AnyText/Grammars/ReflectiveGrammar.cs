using NMF.AnyText.Rules;
using NMF.Transformations;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NMF.AnyText.Grammars
{
    public abstract class ReflectiveGrammar : Grammar
    {
        protected override IDictionary<Type, Rule> CreatesTypedRules()
        {
            return Reflector.ReflectDictionary(GetGrammarTypeStack(), CreateDefaultRules, null);
        }

        protected override IEnumerable<Rule> CreateCustomRules()
        {
            return null;
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
    }
}
