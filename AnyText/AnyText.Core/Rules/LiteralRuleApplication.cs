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
        /// Creates a new instance
        /// </summary>
        /// <param name="rule">the rule</param>
        /// <param name="literal">the matched literal</param>
        /// <param name="length">the length (use in case of multiline literals)</param>
        /// <param name="currentPosition">the current position</param>
        /// <param name="examinedTo"></param>
        public LiteralRuleApplication(Rule rule, string literal, ParsePositionDelta length, ParsePosition currentPosition, ParsePositionDelta examinedTo) : base(rule, currentPosition, length, examinedTo)
        {
            Literal = literal;
        }

        /// <summary>
        /// Gets the matched literal
        /// </summary>
        public string Literal { get; private set; }

        /// <inheritdoc />
        public override RuleApplication ApplyTo(RuleApplication other, ParseContext context)
        {
            return other.MigrateTo(this, context);
        }

        internal override RuleApplication MigrateTo(LiteralRuleApplication literal, ParseContext context)
        {
            if (literal.Rule != Rule)
            {
                return base.MigrateTo(literal, context);
            }
            
            var old = Literal;
            Literal = literal.Literal;
            OnMigrate(old, Literal, context);
            EnsurePosition(literal.CurrentPosition, false);
            Length = literal.Length;
            ExaminedTo = literal.ExaminedTo;
            Comments = literal.Comments;
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
                if (context.GetDefinition(oldValue).CurrentPosition.CompareTo(CurrentPosition) == 0)
                {
                    context.RemoveDefinition(oldValue);
                    context.AddDefinition(newValue, this);

                    var references = context.GetReferences(oldValue);
                    context.RemoveReferences(oldValue);
                    context.SetReferences(newValue, references);
                }

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
            if (Comments != null)
            {
                foreach (var comment in Comments)
                {
                    comment.IterateLiterals(action);
                }
            }
            action(this);
        }

        /// <inheritdoc />
        public override void IterateLiterals<T>(Action<LiteralRuleApplication, T> action, T parameter)
        {
            if (Comments != null)
            {
                foreach (var comment in Comments)
                {
                    comment.IterateLiterals(action, parameter);
                }
            }
            action(this, parameter);
        }

        /// <inheritdoc />
        public override void Write(PrettyPrintWriter writer, ParseContext context)
        {
            if (Comments != null)
            {
                foreach (var comment in Comments)
                {
                    comment.Write(writer, context);
                    writer.WriteNewLine();
                }

            }
            writer.WriteToken(Literal, Rule.TrailingWhitespaces);
        }
    }
}
