using NMF.AnyText.Rules;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NMF.AnyText.PrettyPrinting
{
    /// <summary>
    /// Helper class to format rules
    /// </summary>
    public static class RuleFormatter
    {
        public static FormattedRule ZeroOrMore(Rule rule, params FormattingInstruction[] instructions)
        {
            return new FormattedRule(new ZeroOrMoreRule(rule), instructions);
        }
        public static FormattedRule ZeroOrMore(FormattedRule rule, params FormattingInstruction[] instructions)
        {
            return new FormattedRule(new ZeroOrMoreRule(rule), instructions);
        }

        public static FormattedRule OneOrMore(Rule rule, params FormattingInstruction[] instructions)
        {
            return new FormattedRule(new OneOrMoreRule(rule), instructions);
        }

        public static FormattedRule OneOrMore(FormattedRule rule, params FormattingInstruction[] instructions)
        {
            return new FormattedRule(new OneOrMoreRule(rule), instructions);
        }

        public static FormattedRule ZeroOrOne(Rule rule, params FormattingInstruction[] instructions)
        {
            return new FormattedRule(new ZeroOrOneRule(rule), instructions);
        }

        public static FormattedRule ZeroOrOne(FormattedRule rule, params FormattingInstruction[] instructions)
        {
            return new FormattedRule(new ZeroOrOneRule(rule), instructions);
        }
    }
}
