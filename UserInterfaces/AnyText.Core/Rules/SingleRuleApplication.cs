using NMF.AnyText.PrettyPrinting;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NMF.AnyText.Rules
{
    internal class SingleRuleApplication : RuleApplication
    {
        public SingleRuleApplication(Rule rule, RuleApplication inner, ParsePositionDelta endsAt, ParsePositionDelta examinedTo) : base(rule, (inner?.CurrentPosition).GetValueOrDefault(), endsAt, examinedTo)
        {
            Inner = inner;
        }

        public RuleApplication Inner { get; private set; }

        public override void Activate(ParseContext context, ParsePosition position)
        {
            if (Inner != null && !Inner.IsActive)
            {
                Inner.Parent = this;
                Inner.Activate(context, position);
            }
            base.Activate(context, position);
        }

        public override RuleApplication ApplyTo(RuleApplication other, ParsePosition position, ParseContext context)
        {
            return other.MigrateTo(this, position, context);
        }

        public override void Deactivate(ParseContext context)
        {
            if (Inner != null && Inner.IsActive )
            {
                Inner.Parent = null;
                Inner.Deactivate(context);
            }
            base.Deactivate(context);
        }

        internal override RuleApplication MigrateTo(SingleRuleApplication singleRule, ParsePosition position, ParseContext context)
        {
            if (singleRule.Rule != Rule)
            {
                return base.MigrateTo(singleRule, position, context);
            }
            var old = Inner;
            if (old.Rule == singleRule.Inner.Rule)
            {
                Inner = singleRule.Inner.ApplyTo(Inner, position, context);
            }
            else
            {
                Inner = singleRule.Inner;
            }
            if (old != Inner)
            {
                OnMigrate(old, Inner, position, context);
            }
            CurrentPosition = singleRule.CurrentPosition;
            return this;
        }

        protected virtual void OnMigrate(RuleApplication oldValue, RuleApplication newValue, ParsePosition position, ParseContext context)
        {
            if (oldValue.IsActive)
            {
                oldValue.Deactivate(context);
                newValue.Activate(context, position);
                OnValueChange(this, context);
            }
        }

        public override object GetValue(ParseContext context)
        {
            return Inner?.GetValue(context);
        }

        /// <inheritdoc />
        public override void IterateLiterals(Action<LiteralRuleApplication> action)
        {
            Inner.IterateLiterals(action);
        }

        /// <inheritdoc />
        public override void IterateLiterals<T>(Action<LiteralRuleApplication, T> action, T parameter)
        {
            Inner.IterateLiterals(action, parameter);
        }

        /// <inheritdoc />
        public override void Write(PrettyPrintWriter writer, ParseContext context)
        {
            Inner?.Write(writer, context);
            
        }
    }
}
