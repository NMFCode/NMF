using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NMF.AnyText.Rules
{
    /// <summary>
    /// Denotes a rule application that matches a given literal string
    /// </summary>
    public class LiteralRuleApplication : RuleApplication
    {
        /// <summary>
        /// Creates a new instance
        /// </summary>
        /// <param name="rule">the rule</param>
        /// <param name="literal">the matched literal</param>
        /// <param name="currentPosition">the current position</param>
        /// <param name="examinedTo"></param>
        public LiteralRuleApplication(Rule rule, string literal, ParsePosition currentPosition, ParsePositionDelta examinedTo) : base(rule, currentPosition, new ParsePositionDelta(0, literal.Length), examinedTo)
        {
            Literal = literal;
        }

        /// <summary>
        /// Gets the matched literal
        /// </summary>
        public string Literal { get; private set; }

        /// <inheritdoc />
        public override RuleApplication ApplyTo(RuleApplication other, ParsePosition position, ParseContext context)
        {
            return other.MigrateTo(this, position, context);
        }

        internal override RuleApplication MigrateTo(LiteralRuleApplication literal, ParsePosition position, ParseContext context)
        {
            if (literal.Rule != Rule)
            {
                return base.MigrateTo(literal, position, context);
            }

            var old = Literal;
            Literal = literal.Literal;
            OnMigrate(old, Literal, context);
            CurrentPosition = literal.CurrentPosition;
            return this;
        }

        /// <summary>
        /// Gets called when the rule application is migrated
        /// </summary>
        /// <param name="oldValue">the old literal</param>
        /// <param name="newValue">the new literal</param>
        /// <param name="context">the parse context</param>
        protected virtual void OnMigrate(string oldValue, string newValue, ParseContext context)
        {
            if (oldValue != newValue)
            {
                OnValueChange(this, context);
            }
        }

        /// <inheritdoc />
        public override object GetValue(ParseContext context)
        {
            return Literal;
        }

        /// <inheritdoc />
        public override void IterateLiterals(Action<LiteralRuleApplication> action)
        {
            action(this);
        }

        /// <inheritdoc />
        public override void IterateLiterals<T>(Action<LiteralRuleApplication, T> action, T parameter)
        {
            action(this, parameter);
        }
    }
}
