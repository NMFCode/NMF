using NMF.AnyText.Rules;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NMF.AnyText.IndexCalculation
{
    internal class DetailedIndexCalculationScheme : IndexCalculationScheme
    {
        public override int CalculateIndex(RuleApplication ruleApplication)
        {
            if (ruleApplication.Parent != null)
            {
                var stack = new Stack<Rule>();
                stack.Push(ruleApplication.Rule);
                return ruleApplication.Parent.CalculateIndex(ruleApplication, stack);
            }
            return -1;
        }
    }
}
