using NMF.Synchronizations;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NMF.Glsp.Language
{
    public abstract class GraphicalLanguage
    {
        public abstract string DiagramType { get; }

        public abstract Type SemanticRootType { get; }

        private readonly Dictionary<Type, DescriptorBase> _rules = new Dictionary<Type, DescriptorBase>();

        public void Initialize()
        {
            foreach (var ruleType in GetType().GetNestedTypes())
            {
                if (!ruleType.IsAbstract && !ruleType.IsInterface && typeof(DescriptorBase).IsAssignableFrom(ruleType))
                {
                    var ruleInstance = Activator.CreateInstance(ruleType) as DescriptorBase;
                    ruleInstance.Language = this;
                    _rules.Add(ruleType, ruleInstance);
                }
            }
            foreach (var rule in _rules.Values)
            {
                rule.DefineLayout();
            }
        }

        public T Descriptor<T>() where T : DescriptorBase
        {
            if (_rules.TryGetValue(typeof(T), out var ruleInstance))
            {
                return ruleInstance as T;
            }
            else
            {
                return null;
            }
        }
    }
}
