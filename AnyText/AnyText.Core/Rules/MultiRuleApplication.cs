﻿using NMF.AnyText.PrettyPrinting;
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

        public override void Activate(ParseContext context, ParsePosition position)
        {
            foreach (var inner in Inner)
            {
                inner.Parent = this;
                if (!inner.IsActive)
                {
                    inner.Activate(context, position);
                }
                position += inner.Length;
            }
            base.Activate(context, position);
        }

        public override RuleApplication ApplyTo(RuleApplication other, ParsePosition position, ParseContext context)
        {
            return other.MigrateTo(this, position, context);
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

        internal override RuleApplication MigrateTo(MultiRuleApplication multiRule, ParsePosition position, ParseContext context)
        {
            if (multiRule.Rule != Rule)
            {
                return base.MigrateTo(multiRule, position, context);
            }

            var removed = new List<RuleApplication>();
            var added = new List<RuleApplication>();
            var tailOffset = multiRule.Inner.Count - Inner.Count;
            int firstDifferentIndex = CalculateFirstDifferentIndex(multiRule);
            int lastDifferentIndex = CalculateLastDifferentIndex(multiRule, tailOffset);

            for (int i = 0; i < firstDifferentIndex; i++)
            {
                position += Inner[i].Length;
            }

            for (int i = firstDifferentIndex; i <= lastDifferentIndex; i++)
            {
                if (i < multiRule.Inner.Count)
                {
                    var old = Inner[i];
                    var newApp = multiRule.Inner[i].ApplyTo(old, position, context);
                    Inner[i] = newApp;
                    if (old != newApp && old.IsActive)
                    {
                        old.Deactivate(context);
                        newApp.Activate(context, position);
                    }
                    position += newApp.Length;
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
                    item.Activate(context, position);
                    position += item.Length;
                }
                Inner.Insert(lastDifferentIndex + i, item);
            }
            OnMigrate(removed, added);
            CurrentPosition = Inner[0].CurrentPosition;
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
            foreach (var app in Inner)
            {
                app.Write(writer, context);
            }
            ApplyFormattingInstructions(writer);
        }
    }
}
