using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NMF.AnyText.Rules
{
    internal class MultiRuleApplication : RuleApplication
    {

        public MultiRuleApplication(Rule rule, List<RuleApplication> inner, ParsePositionDelta endsAt, ParsePositionDelta examinedTo) : base(rule, endsAt, examinedTo)
        {
            Inner = inner;
        }

        public List<RuleApplication> Inner { get; }

        public override void Activate(ParseContext context)
        {
            base.Activate(context);
            foreach (var inner in Inner)
            {
                inner.Parent = this;
                if (!inner.IsActive)
                {
                    inner.Activate(context);
                }
            }
        }

        public override RuleApplication ApplyTo(RuleApplication other, ParseContext context)
        {
            return other.MigrateTo(this, context);
        }

        public override void Deactivate(ParseContext context)
        {
            foreach (var inner in Inner)
            {
                inner.Parent = null;
                if (inner.IsActive)
                {
                    inner.Deactivate(context);
                }
            }
            base.Deactivate(context);
        }

        internal override RuleApplication MigrateTo(MultiRuleApplication multiRule, ParseContext context)
        {
            var removed = new List<RuleApplication>();
            var added = new List<RuleApplication>();
            var tailOffset = multiRule.Inner.Count - Inner.Count;
            int firstDifferentIndex = CalculateFirstDifferentIndex(multiRule);
            int lastDifferentIndex = CalculateLastDifferentIndex(multiRule, tailOffset);

            for (int i = firstDifferentIndex; i <= lastDifferentIndex; i++)
            {
                if (i < multiRule.Inner.Count)
                {
                    var old = Inner[i];
                    var newApp = multiRule.Inner[i].ApplyTo(old, context);
                    Inner[i] = newApp;
                    if (old != newApp && old.IsActive)
                    {
                        old.Deactivate(context);
                        newApp.Activate(context);
                    }
                }
                else
                {
                    removed.Add(Inner[i]);
                    Inner[i].Deactivate(context);
                    Inner.RemoveAt(i);
                }
            }
            for (int i = 1; i <= tailOffset; i++)
            {
                var item = multiRule.Inner[lastDifferentIndex + i];
                added.Add(item);
                if (IsActive)
                {
                    item.Activate(context);
                }
                Inner.Insert(lastDifferentIndex + i, item);
            }
            OnMigrate(removed, added);
            return this;
        }

        protected virtual void OnMigrate(List<RuleApplication> removed, List<RuleApplication> added) { }

        private int CalculateLastDifferentIndex(MultiRuleApplication multiRule, int tailOffset)
        {
            var lastIndex = Inner.Count - 1;
            while (lastIndex > 0 && Inner[lastIndex] == multiRule.Inner[lastIndex + tailOffset])
            {
                lastIndex--;
            }

            return lastIndex;
        }

        private int CalculateFirstDifferentIndex(MultiRuleApplication multiRule)
        {
            var index = 0;
            while (index < Inner.Count && index < multiRule.Inner.Count && Inner[index] == multiRule.Inner[index])
            {
                index++;
            }

            return index;
        }

        public override object GetValue(ParseContext context)
        {
            return Inner.Select(app => app.GetValue(context));
        }
    }
}
