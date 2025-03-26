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
        /// <summary>
        /// Formats the given rule zero or more times
        /// </summary>
        /// <param name="rule">the base rule</param>
        /// <param name="instructions">an array of formatting instructions</param>
        /// <returns>an instance of <see cref="FormattedRule"/> indicating zero or more occurences of the provided rule</returns>
        public static FormattedRule ZeroOrMore(Rule rule, params FormattingInstruction[] instructions)
        {
            return new FormattedRule(new ZeroOrMoreRule(rule), instructions);
        }

        /// <summary>
        /// Formats the given rule zero or more times
        /// </summary>
        /// <param name="rule">the base rule</param>
        /// <param name="instructions">an array of formatting instructions</param>
        /// <returns>an instance of <see cref="FormattedRule"/> indicating zero or more occurences of the provided rule</returns>
        public static FormattedRule ZeroOrMore(FormattedRule rule, params FormattingInstruction[] instructions)
        {
            return new FormattedRule(new ZeroOrMoreRule(rule), instructions);
        }

        /// <summary>
        /// Formats the given rule one or more times
        /// </summary>
        /// <param name="rule">the base rule</param>
        /// <param name="instructions">an array of formatting instructions</param>
        /// <returns>an instance of <see cref="FormattedRule"/> indicating one or more occurences of the provided rule</returns>
        public static FormattedRule OneOrMore(Rule rule, params FormattingInstruction[] instructions)
        {
            return new FormattedRule(new OneOrMoreRule(rule), instructions);
        }

        /// <summary>
        /// Formats the given rule one or more times
        /// </summary>
        /// <param name="rule">the base rule</param>
        /// <param name="instructions">an array of formatting instructions</param>
        /// <returns>an instance of <see cref="FormattedRule"/> indicating one or more occurences of the provided rule</returns>
        public static FormattedRule OneOrMore(FormattedRule rule, params FormattingInstruction[] instructions)
        {
            return new FormattedRule(new OneOrMoreRule(rule), instructions);
        }

        /// <summary>
        /// Formats the given rule once or not at all
        /// </summary>
        /// <param name="rule">the base rule</param>
        /// <param name="instructions">an array of formatting instructions</param>
        /// <returns>an instance of <see cref="FormattedRule"/> indicating once or not at all occurences of the provided rule</returns>
        public static FormattedRule ZeroOrOne(Rule rule, params FormattingInstruction[] instructions)
        {
            return new FormattedRule(new ZeroOrOneRule(rule), instructions);
        }

        /// <summary>
        /// Formats the given rule once or not at all
        /// </summary>
        /// <param name="rule">the base rule</param>
        /// <param name="instructions">an array of formatting instructions</param>
        /// <returns>an instance of <see cref="FormattedRule"/> indicating once or not at all occurences of the provided rule</returns>
        public static FormattedRule ZeroOrOne(FormattedRule rule, params FormattingInstruction[] instructions)
        {
            return new FormattedRule(new ZeroOrOneRule(rule), instructions);
        }
    }
}
