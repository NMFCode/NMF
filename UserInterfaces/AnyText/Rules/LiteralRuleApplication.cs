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
    internal class LiteralRuleApplication : RuleApplication
    {
        public LiteralRuleApplication(Rule rule, string literal, ParsePositionDelta endsAt, ParsePositionDelta examinedTo) : base(rule, endsAt, examinedTo)
        {
            Literal = literal;
        }

        public string Literal { get; private set; }

        public override RuleApplication ApplyTo(RuleApplication other, ParseContext context)
        {
            return other.MigrateTo(this, context);
        }

        internal override RuleApplication MigrateTo(LiteralRuleApplication literal, ParseContext context)
        {
            var old = Literal;
            Literal = literal.Literal;
            OnMigrate(old, Literal, context);
            return this;
        }

        protected virtual void OnMigrate(string oldValue, string newValue, ParseContext context)
        {
            if (oldValue != newValue)
            {
                OnValueChange(this, context);
            }
        }

        public override object GetValue(ParseContext context)
        {
            return Literal;
        }
    }
}
