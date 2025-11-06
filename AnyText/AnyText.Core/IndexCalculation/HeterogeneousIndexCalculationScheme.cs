using NMF.AnyText.Rules;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NMF.AnyText.IndexCalculation
{
    internal class HeterogeneousIndexCalculationScheme : IndexCalculationScheme
    {
        private readonly int _maxDepth;

        public HeterogeneousIndexCalculationScheme(int maxDepth)
        {
            _maxDepth = maxDepth;
        }

        public override int CalculateIndex(RuleApplication ruleApplication)
        {
            var rootApplication = ruleApplication.Parent;
            var counter = 0;
            while (rootApplication != null && !rootApplication.Rule.IsDefinition && counter < _maxDepth)
            {
                rootApplication = rootApplication.Parent;
                counter++;
            }
            if (rootApplication == null)
            {
                return -1;
            }

            var stack = new Stack<RuleApplication>();
            stack.Push(rootApplication);
            var index = 0;
            while (stack.Count > 0)
            {
                var rule = stack.Pop();
                if (rule == ruleApplication)
                {
                    break;
                }
                if (rule.Rule == ruleApplication.Rule)
                {
                    index++;
                }
                foreach (var child in rule.Children.Reverse())
                {
                    stack.Push(child);
                }
            }
            return index;
        }
    }
}
