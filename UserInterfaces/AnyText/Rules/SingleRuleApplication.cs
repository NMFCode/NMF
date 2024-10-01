using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NMF.AnyText.Rules
{
    internal class SingleRuleApplication : RuleApplication
    {
        public SingleRuleApplication(Rule rule, RuleApplication inner, ParsePositionDelta endsAt, ParsePositionDelta examinedTo) : base(rule, endsAt, examinedTo)
        {
            Inner = inner;
        }

        public RuleApplication Inner { get; private set; }

        public override void Activate(ParseContext context)
        {
            base.Activate(context);
            if (Inner != null && !Inner.IsActive)
            {
                Inner.Parent = this;
                Inner.Activate(context);
            }
        }

        public override RuleApplication ApplyTo(RuleApplication other, ParseContext context)
        {
            return other.MigrateTo(this, context);
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

        internal override RuleApplication MigrateTo(SingleRuleApplication singleRule, ParseContext context)
        {
            var old = Inner;
            if (old.Rule == singleRule.Inner.Rule)
            {
                Inner = singleRule.Inner.ApplyTo(Inner, context);
            }
            else
            {
                Inner = singleRule.Inner;
            }
            if (old != Inner)
            {
                OnMigrate(old, Inner, context);
            }
            return this;
        }

        protected virtual void OnMigrate(RuleApplication oldValue, RuleApplication newValue, ParseContext context)
        {
            if (oldValue.IsActive)
            {
                oldValue.Deactivate(context);
                newValue.Activate(context);
                OnValueChange(this, context);
            }
        }

        public override object GetValue(ParseContext context)
        {
            return Inner?.GetValue(context);
        }
    }
}
