using NMF.AnyText.Rules;
using NMF.Transformations;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NMF.AnyText.Grammars
{
    /// <summary>
    /// Denotes a grammar that resolves rules by nested public classes, allowing rule instantiation
    /// </summary>
    public abstract class ReflectiveGrammar : Grammar
    {
        /// <inheritdoc />
        protected override IDictionary<Type, Rule> CreateTypedRules()
        {
            return Reflector.ReflectDictionary(GetGrammarTypeStack(), CreateDefaultRules, null);
        }

        /// <inheritdoc />
        protected override IEnumerable<Rule> CreateCustomRules()
        {
            return Enumerable.Empty<Rule>();
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

        /// <summary>
        /// Creates a collection of default rules
        /// </summary>
        /// <returns>a collection of rules</returns>
        protected virtual IEnumerable<Rule> CreateDefaultRules()
        {
            return Enumerable.Empty<Rule>();
        }

        /// <inheritdoc />
        protected override ParseContext CreateParseContext()
        {
            return new ModelParseContext(this);
        }
    }
}
