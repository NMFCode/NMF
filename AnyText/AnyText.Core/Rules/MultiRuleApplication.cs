using NMF.AnyText.PrettyPrinting;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.Versioning;
using System.Text;
using System.Threading.Tasks;

namespace NMF.AnyText.Rules
{
    internal class MultiRuleApplication : RuleApplication
    {

        public MultiRuleApplication(Rule rule, ParsePosition currentPosition, List<RuleApplication> inner, ParsePositionDelta endsAt, ParsePositionDelta examinedTo) : base(rule, currentPosition, endsAt, examinedTo)
        {
            Inner = inner;
        }

        public List<RuleApplication> Inner { get; }

        public override void Activate(ParseContext context)
        {
            foreach (var inner in Inner)
            {
                inner.Parent = this;
                if (!inner.IsActive)
                {
                    inner.Activate(context);
                }
            }
            base.Activate(context);
        }

        public override RuleApplication ApplyTo(RuleApplication other, ParseContext context)
        {
            return other.MigrateTo(this, context);
        }

        public override void Shift(ParsePositionDelta shift)
        {
            base.Shift(shift);
            foreach (var inner in Inner)
            {
                inner.Shift(shift);
            }
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
            if (multiRule.Rule != Rule)
            {
                return base.MigrateTo(multiRule, context);
            }

            CurrentPosition = multiRule.CurrentPosition;
            Length = multiRule.Length;
            ExaminedTo = multiRule.ExaminedTo;

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
                        newApp.Parent = this;
                        old.Deactivate(context);
                        newApp.Activate(context);
                        old.Parent = null;
                    }
                }
                else
                {
                    var old = Inner[i];
                    removed.Add(old);
                    old.Deactivate(context);
                    old.Parent = null;
                    Inner.RemoveAt(i);
                }
            }
            for (int i = 1; i <= tailOffset; i++)
            {
                var item = multiRule.Inner[lastDifferentIndex + i];
                added.Add(item);
                if (IsActive)
                {
                    item.Parent = this;
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


        /// <inheritdoc />
        public override void IterateLiterals(Action<LiteralRuleApplication> action)
        {
            foreach (var item in Inner)
            {
                item.IterateLiterals(action);
            }
        }

        /// <inheritdoc />
        public override void IterateLiterals<T>(Action<LiteralRuleApplication, T> action, T parameter)
        {
            foreach(var item in Inner)
            {
                item.IterateLiterals(action, parameter);
            }
        }

        /// <inheritdoc />
        public override void Write(PrettyPrintWriter writer, ParseContext context)
        {
            Rule.Write(writer, context, this);
        }
    }
}
