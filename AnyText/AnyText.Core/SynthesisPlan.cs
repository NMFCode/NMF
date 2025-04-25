using NMF.AnyText.Rules;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NMF.AnyText
{
    /// <summary>
    /// Denotes a planned synthesis
    /// </summary>
    public class SynthesisPlan
    {
        private readonly Dictionary<(Rule,object), bool> _decisions = new Dictionary<(Rule,object), bool>();

        /// <summary>
        /// Checks whether the provided rule can synthesize the provided object in this synthesis plan
        /// </summary>
        /// <param name="rule">the rule that should be synthesized</param>
        /// <param name="semanticObject">the object that should be synthesized</param>
        /// <param name="context">the context in which the synthesis is required</param>
        /// <returns>true, if the object can be synthesized, otherwise false</returns>
        public bool CanSynthesize(Rule rule, object semanticObject, ParseContext context)
        {
            if (!_decisions.TryGetValue((rule, semanticObject), out var decision))
            {
                decision = rule.CanSynthesize(semanticObject, context);
                _decisions[(rule, semanticObject)] = decision;
            }
            return decision;
        }
    }
}
