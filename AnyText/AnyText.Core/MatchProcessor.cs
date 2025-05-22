using NMF.AnyText.Rules;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NMF.AnyText
{
    [DebuggerDisplay("{Rule}")]
    internal abstract class MatchProcessor
    {
        public abstract Rule Rule { get; }

        public abstract MatchOrMatchProcessor NextMatchProcessor(ParseContext context, RuleApplication ruleApplication, ref ParsePosition position, ref RecursionContext recursionContext);
    }
}
