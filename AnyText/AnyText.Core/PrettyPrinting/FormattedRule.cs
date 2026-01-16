using NMF.AnyText.Rules;

namespace NMF.AnyText.PrettyPrinting
{
    /// <summary>
    /// Denotes a tuple of a rule and formatting instructions
    /// </summary>
    /// <param name="Rule">The actual rule</param>
    /// <param name="FormattingInstructions">Formatting instructions</param>
    public record struct FormattedRule(Rule Rule, FormattingInstruction[] FormattingInstructions)
    {
        /// <summary>
        /// Implicitly converts a rule into a formatted rule by not applying formatting
        /// </summary>
        /// <param name="rule"></param>
        public static implicit operator FormattedRule(Rule rule)
        {
            return new FormattedRule(rule, null);
        }
    }
}
