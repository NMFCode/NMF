using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NMF.AnyText.Rules
{
    internal abstract class RecursiveContinuation
    {
        public virtual RuleApplication ResolveRecursion(RuleApplication baseApplication, ParseContext parseContext, ref ParsePosition position)
        {
            return baseApplication;
        }
    }
}
