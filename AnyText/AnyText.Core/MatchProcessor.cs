using NMF.AnyText.Rules;
using System.Diagnostics;

namespace NMF.AnyText
{
    [DebuggerDisplay("{Rule}")]
    internal abstract class MatchProcessor
    {
        public abstract Rule Rule { get; }

        public abstract MatchOrMatchProcessor NextMatchProcessor(ParseContext context, RuleApplication ruleApplication, ref ParsePosition position, ref RecursionContext recursionContext);
    }
}
