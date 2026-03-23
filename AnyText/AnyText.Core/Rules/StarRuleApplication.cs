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
                var newStop = RuleHelper.Star(context, null, Stopper.Rule, applications, CurrentPosition, AcceptEverything, ref position, ref examined, ref isRecovered);
                var recovery = new StarRuleApplication(Rule, applications, newStop, position - CurrentPosition, examined).SetRecovered(true);
                ReplaceWith(recovery);
                return recovery;
            }
            position = CurrentPosition;
            return this;
        }

        private static bool AcceptEverything(RuleApplication ruleApplication, List<RuleApplication> applications, ParseContext context, ref ParsePositionDelta examined)
        {
            return true;
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

            if (multiRule is StarRuleApplication star)
            {
                Stopper = star.Stopper;
            }

            MigrateInner(multiRule, context);

            return this;
        }

        private void MigrateInner(MultiRuleApplication multiRule, ParseContext context)
        {
            var migrations = context.ChangeTracker.CalculateListMigrations(Inner, multiRule.Inner, context);
            foreach (var migration in migrations)
            {
                switch (migration.Type)
                {
                    case RuleApplicationListMigrationType.Migrate:
                        MigrateChild(multiRule, context, migration.Index);
                        break;
                    case RuleApplicationListMigrationType.Insert:
                        InsertChild(context, migration.Index, multiRule.Inner[migration.Index]);
                        break;
                    case RuleApplicationListMigrationType.Remove:
                        RemoveChild(context, migration.Index);
                        break;
                    default:
                        break;
                }
            }
        }

        private void InsertChild(ParseContext context, int index, RuleApplication toAdd)
        {
            if (Parent != null && toAdd.CurrentPosition < Parent.CurrentPosition)
            {
                throw new InvalidOperationException("There was an error processing this change.");
            }
            if (index <= Inner.Count)
            {
                Inner.Insert(index, toAdd);
                if (IsActive)
                {
                    toAdd.ChangeParent(this, context);
                    toAdd.Activate(context, false);
                }
            }
        }

        private void RemoveChild(ParseContext context, int index)
        {
            if (index < Inner.Count)
            {
                var old = Inner[index];
                if (old.IsActive)
                {
                    old.Deactivate(context);
                }
                old.ChangeParent(null, context);
                Inner.RemoveAt(index);
            }
        }

        private void MigrateChild(MultiRuleApplication multiRule, ParseContext context, int index)
        {
            var old = Inner[index];
            var newApp = multiRule.Inner[index].ApplyTo(old, context);
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
