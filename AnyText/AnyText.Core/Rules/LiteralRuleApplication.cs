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
        /// <param name="examinedTo"></param>
        /// 
        public LiteralRuleApplication(Rule rule, string literal, ParsePositionDelta examinedTo) : base(rule, new ParsePositionDelta(0, literal.Length), examinedTo)
        {
            Literal = literal;
        }

        /// <summary>
        /// Creates a new instance
        /// </summary>
        /// <param name="rule">the rule</param>
        /// <param name="literal">the matched literal</param>
        /// <param name="length">the length (use in case of multiline literals)</param>
        /// <param name="examinedTo"></param>
        /// 
        public LiteralRuleApplication(Rule rule, string literal, ParsePositionDelta length, ParsePositionDelta examinedTo) : base(rule, length, examinedTo)
        {
            Literal = literal;
        }

        /// <summary>
        /// Gets the matched literal
        /// </summary>
        public string Literal { get; }

        /// <inheritdoc />
        public override object SemanticElement => Literal;


        /// <inheritdoc />
        public override RuleApplication ApplyTo(RuleApplication other, ParseContext context)
        {
            return other.MigrateTo(this, context);
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
                OnValueChange(this, context, null);
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

        /// <inheritdoc />
        public override RuleApplication GetLiteralAt(ParsePosition position, bool active = false)
        {
            var currentPos = CurrentPosition;
            if (position.Line == currentPos.Line && position.Col >= currentPos.Col && position.Col <= currentPos.Col + Literal.Length)
            {
                return this;
            }
            return null;
        }

        /// <inheritdoc />
        public override LiteralRuleApplication GetFirstInnerLiteral()
        {
            return this;
        }

        /// <inheritdoc />
        public override LiteralRuleApplication GetLastInnerLiteral()
        {
            return this;
        }
    }
}
