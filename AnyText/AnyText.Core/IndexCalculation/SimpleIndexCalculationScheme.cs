using NMF.AnyText.Rules;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NMF.AnyText.IndexCalculation
{
    internal class SimpleIndexCalculationScheme : IndexCalculationScheme
    {
        public override int CalculateIndex(RuleApplication ruleApplication)
        {
            if (ruleApplication.Parent != null)
            {
                return ruleApplication.Parent.CalculateIndex(ruleApplication);
            }
            return -1;
        }
    }
}
