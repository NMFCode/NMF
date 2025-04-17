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
        /// <param name="examinedTo">the amount of text that was analyzed to draw the conclusion</param>
        /// <param name="message">the message to indicate why the rule application failed</param>
        /// 
        public FailedRuleApplication(Rule rule, ParsePositionDelta examinedTo, string message) : base(rule, default, examinedTo)
        {
            Message = message;
        }

        /// <summary>
        /// Gets the message to indicate why the rule application failed
        /// </summary>
        public string Message { get; }

        /// <summary>
        /// Gets the position of the error
        /// </summary>
        public ParsePosition ErrorPosition { get; }

        public override IEnumerable<DiagnosticItem> CreateParseErrors()
        {
            yield return new DiagnosticItem(DiagnosticSources.Parser, this, Message);
        }

        /// <inheritdoc />
        public override bool IsPositive => false;

        /// <inheritdoc />
        public override RuleApplication ApplyTo(RuleApplication other, ParseContext context)
        {
            other.ReplaceWith(this);
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

        public override RuleApplication GetLiteralAt(ParsePosition position)
        {
            return null;
        }

        public override RuleApplication PotentialError => this;
    }
}
