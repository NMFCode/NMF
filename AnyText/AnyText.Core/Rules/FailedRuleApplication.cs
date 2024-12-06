using NMF.AnyText.PrettyPrinting;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NMF.AnyText.Rules
{
    /// <summary>
    /// Denotes a rule application that failed
    /// </summary>
    internal class FailedRuleApplication : RuleApplication
    {
        /// <summary>
        /// Creates a new failed rule application
        /// </summary>
        /// <param name="rule">the rule that failed</param>
        /// <param name="currentPosition">the current position of this rule application</param>
        /// <param name="examinedTo">the amount of text that was analyzed to draw the conclusion</param>
        /// <param name="errorPosition">The position of the error</param>
        /// <param name="message">the message to indicate why the rule application failed</param>
        public FailedRuleApplication(Rule rule, ParsePosition currentPosition, ParsePositionDelta examinedTo, ParsePosition errorPosition, string message) : base(rule, currentPosition, default, examinedTo)
        {
            Message = message;
            ErrorPosition = errorPosition;
        }

        /// <summary>
        /// Gets the message to indicate why the rule application failed
        /// </summary>
        public override string Message { get; }

        /// <summary>
        /// Gets the position of the error
        /// </summary>
        public override ParsePosition ErrorPosition { get; }

        /// <inheritdoc />
        public override bool IsPositive => false;

        /// <inheritdoc />
        public override RuleApplication ApplyTo(RuleApplication other, ParseContext context)
        {
            EnsurePosition(CurrentPosition, false);
            return this;
        }

        /// <inheritdoc />
        public override object GetValue(ParseContext context)
        {
            return null;
        }

        /// <inheritdoc />
        public override void IterateLiterals(Action<LiteralRuleApplication> action)
        {
        }

        /// <inheritdoc />
        public override void IterateLiterals<T>(Action<LiteralRuleApplication, T> action, T parameter)
        {
        }

        /// <inheritdoc />
        public override void Write(PrettyPrintWriter writer, ParseContext context)
        {
        }
    }
}
