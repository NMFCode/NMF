using System;
using System.Collections.Generic;
using System.Runtime.CompilerServices;

namespace NMF.AnyText.Rules
{
    internal class StarRuleApplication : MultiRuleApplication
    {
        public RuleApplication Stopper { get; set; }

        public StarRuleApplication(Rule rule, List<RuleApplication> inner, RuleApplication stopper, ParsePositionDelta length, ParsePositionDelta examinedTo) : base(rule, inner, length, examinedTo)
        {
            Stopper = stopper;
            if (stopper != null && !stopper.IsPositive)
            {
                stopper.ChangeParent(this, null);
            }
        }

        internal override IEnumerable<CompletionEntry> SuggestCompletions(ParsePosition position, string fragment, ParseContext context, ParsePosition nextTokenPosition)
        {
            var baseSuggestions = base.SuggestCompletions(position, fragment, context, nextTokenPosition);
            if (Stopper != null && Stopper.CurrentPosition <= nextTokenPosition && Stopper.CurrentPosition + Stopper.ScopeLength >= position)
            {
                return baseSuggestions.NullsafeConcat(Stopper.SuggestCompletions(position, fragment, context, nextTokenPosition));
            }
            return baseSuggestions;
        }

        public override void AddParseErrors(ParseContext context)
        {
            Stopper?.AddParseErrors(context);
            base.AddParseErrors(context);
        }

        public override RuleApplication GetLiteralAt(ParsePosition position, bool onlyActive = false)
        {
            var lit = base.GetLiteralAt(position, onlyActive);
            if (lit != null || Stopper == null)
            {
                return lit;
            }
            return Stopper.GetLiteralAt(position, onlyActive);
        }

        public override RuleApplication PotentialError => Stopper;

        public override void IterateLiterals(Action<LiteralRuleApplication> action, bool includeFailures)
        {
            base.IterateLiterals(action, true);
            if (Stopper != null && includeFailures)
            {
                Stopper.IterateLiterals(action, true);
            }
        }

        public override void IterateLiterals<T>(Action<LiteralRuleApplication, T> action, T parameter, bool includeFailures)
        {
            base.IterateLiterals(action, parameter, includeFailures);
            if (Stopper != null && includeFailures)
            {
                Stopper.IterateLiterals(action, parameter, true);
            }
        }

        public override RuleApplication Recover(RuleApplication currentRoot, ParseContext context, out ParsePosition position)
        {
            var recovered = Stopper.Recover(currentRoot, context, out position);
            if (recovered.IsPositive)
            {
                position = Stopper.CurrentPosition;
                var examined = ExaminedTo;
                var applications = new List<RuleApplication>(Inner);
                var isRecovered = true;
                var newStop = RuleHelper.Star(context, null, Stopper.Rule, applications, CurrentPosition, (_,_,_) => true, ref position, ref examined, ref isRecovered);
                var recovery = new StarRuleApplication(Rule, applications, newStop, position - CurrentPosition, examined).SetRecovered(true);
                ReplaceWith(recovery);
                return recovery;
            }
            position = CurrentPosition;
            return this;
        }



        internal override RuleApplication MigrateTo(MultiRuleApplication multiRule, ParseContext context)
        {
            if (multiRule.Rule != Rule)
            {
                return base.MigrateTo(multiRule, context);
            }

            Length = multiRule.Length;
            ExaminedTo = multiRule.ExaminedTo;
            Comments = multiRule.Comments;
            multiRule.ReplaceWith(this);

            var removed = new List<RuleApplication>();
            var added = new List<RuleApplication>();
            var skew = multiRule.Inner.Count - Inner.Count;

            MigrateInner(multiRule, context, removed, added);
            OnMigrate(removed, added);

            return this;
        }

        private void MigrateInner(MultiRuleApplication multiRule, ParseContext context, List<RuleApplication> removed, List<RuleApplication> added)
        {
            int index = 0;
            int foreignIndex = index;
            while (index < Inner.Count)
            {
                var current = Inner[index];
                if (foreignIndex < multiRule.Inner.Count && multiRule.Inner[foreignIndex] == current)
                {
                    index++;
                    foreignIndex++;
                    continue;
                }
                if (context.ChangeTracker.IsObsoleted(current, context))
                {
                    RemoveChild(context, removed, index);
                    continue;
                }
                while (foreignIndex < multiRule.Inner.Count)
                {
                    var nextInner = multiRule.Inner[foreignIndex];
                    if (context.ChangeTracker.IsInsertion(nextInner, context))
                    {
                        InsertChild(context, added, index, nextInner);
                        foreignIndex++;
                        index++;
                    }
                    else
                    {
                        break;
                    }
                }
                if (foreignIndex < multiRule.Inner.Count)
                {
                    MigrateChild(multiRule, context, index, foreignIndex);
                    index++;
                    foreignIndex++;
                }
                else
                {
                    RemoveChild(context, removed, index);
                }
            }
            while (foreignIndex < multiRule.Inner.Count)
            {
                var nextInner = multiRule.Inner[foreignIndex];
                InsertChild(context, added, index, nextInner);
                foreignIndex++;
                index++;
            }
        }

        private void BalanceSkew(MultiRuleApplication multiRule, ParseContext context, List<RuleApplication> removed, List<RuleApplication> added, int skew, int lastDifferentIndex)
        {
            while (skew < 0 && lastDifferentIndex + 1 < Inner.Count)
            {
                RemoveChild(context, removed, lastDifferentIndex + 1);
                skew++;
            }
            if (skew > 0)
            {
                for (int i = 1; i <= skew; i++)
                {
                    InsertChild(context, added, lastDifferentIndex + i, multiRule.Inner[lastDifferentIndex + i]);
                }
            }
        }

        private int CalculateLastDifferentIndex(MultiRuleApplication multiRule, int tailOffset)
        {
            var lastIndex = Inner.Count - 1;
            var min = Math.Max(0, -tailOffset);
            while (lastIndex > min && Inner[lastIndex] == multiRule.Inner[lastIndex + tailOffset])
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

        private void InsertChild(ParseContext context, List<RuleApplication> added, int index, RuleApplication toAdd)
        {
            if (index <= Inner.Count)
            {
                added.Add(toAdd);
                Inner.Insert(index, toAdd);
                if (IsActive)
                {
                    toAdd.ChangeParent(this, context);
                    toAdd.Activate(context, false);
                }
            }
        }

        private void RemoveChild(ParseContext context, List<RuleApplication> removed, int index)
        {
            if (index < Inner.Count)
            {
                var old = Inner[index];
                removed.Add(old);
                if (old.IsActive)
                {
                    old.Deactivate(context);
                }
                old.ChangeParent(null, context);
                Inner.RemoveAt(index);
            }
        }

        private void MigrateChild(MultiRuleApplication multiRule, ParseContext context, int index, int foreignIndex)
        {
            var old = Inner[index];
            var newApp = multiRule.Inner[foreignIndex].ApplyTo(old, context);
            Inner[index] = newApp;
            if (old != newApp && old.IsActive)
            {
                newApp.ChangeParent(this, context);
                old.Deactivate(context);
                newApp.Activate(context, false);
            }
        }

    }
}
