using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NMF.AnyText.Rules
{
    /// <summary>
    /// Denotes a rule that other rules occur in sequence
    /// </summary>
    public class SequenceRule : Rule
    {
        /// <summary>
        /// Creates a new instance
        /// </summary>
        public SequenceRule() { }

        /// <summary>
        /// Creates a new instance
        /// </summary>
        /// <param name="rules">The rules that should occur in sequence</param>
        public SequenceRule(params Rule[] rules)
        {
            Rules = rules;
        }


        /// <inheritdoc />
        public override bool CanStartWith(Rule rule)
        {
            foreach (var inner in Rules)
            {
                if (rule == inner || inner.CanStartWith(rule)) return true;
                if (!inner.IsEpsilonAllowed()) return false;
            }
            return false;
        }

        /// <inheritdoc />
        public override bool IsEpsilonAllowed()
        {
            foreach (var rule in Rules)
            {
                if (!rule.IsEpsilonAllowed()) return false;
            }
            return true;
        }

        /// <summary>
        /// The rules that should occur in sequence
        /// </summary>
        public Rule[] Rules { get; set; }

        /// <inheritdoc />
        public override RuleApplication Match(ParseContext context, ref ParsePosition position)
        {
            var savedPosition = position;
            var applications = new List<RuleApplication>();
            var examined = new ParsePositionDelta();
            foreach (var rule in Rules)
            {
                var app = context.Matcher.MatchCore(rule, context, ref position);
                examined = ParsePositionDelta.Larger(examined, app.ExaminedTo);
                if (app.IsPositive)
                {
                    applications.Add(app);
                }
                else
                {
                    position = savedPosition;
                    if (app is RecursiveRuleApplication recurse)
                    {
                        recurse.Continuations.Add(new Continuation(this, applications, examined));
                    }
                    return new FailedRuleApplication(this, savedPosition, examined, app.ErrorPosition, app.Message);
                }
            }
            return CreateRuleApplication(applications.Count > 0 ? applications[0].CurrentPosition : savedPosition, applications, position - savedPosition, examined);
        }

        /// <summary>
        /// Creates a rule application for a success
        /// </summary>
        /// <param name="currentPosition">the current parser position</param>
        /// <param name="inner">the inner list of rule applications</param>
        /// <param name="length">the length of the match</param>
        /// <param name="examined">the amount of text examined</param>
        /// <returns>a new rule application</returns>
        protected virtual RuleApplication CreateRuleApplication(ParsePosition currentPosition, List<RuleApplication> inner, ParsePositionDelta length, ParsePositionDelta examined)
        {
            return new MultiRuleApplication(this, currentPosition, inner, length, examined);
        }

        /// <inheritdoc />
        public override bool CanSynthesize(object semanticElement)
        {
            return Array.TrueForAll(Rules, r => r.CanSynthesize(semanticElement));
        }

        protected virtual bool IsOpeningParanthesis(string literal)
        {
            return literal == "(" || literal == "[" || literal == "{";
        }

        protected virtual bool IsMatchingClosingParanthesis(string literal, string openingParanthesis)
        {
            switch (openingParanthesis)
            {
                case "(": return literal == ")";
                case "[": return literal == "]";
                case "{": return literal == "}";
            }
            return false;
        }

        /// <inheritdoc />
        public override RuleApplication Synthesize(object semanticElement, ParsePosition position, ParseContext context)
        {
            var currentPosition = position;
            var applications = new List<RuleApplication>();
            foreach (var rule in Rules)
            {
                var app = rule.Synthesize(semanticElement, position, context);
                if (app.IsPositive)
                {
                    applications.Add(app);
                    currentPosition += app.Length;
                }
                else
                {
                    return new FailedRuleApplication(this, position, default, app.ErrorPosition, app.Message);
                }
            }
            return CreateRuleApplication(position, applications, currentPosition - position, default);
        }

        private sealed class Continuation : RecursiveContinuation
        {
            private readonly SequenceRule _parent;
            private readonly List<RuleApplication> _rules;
            private readonly ParsePositionDelta _examinedSoFar;

            public Continuation(SequenceRule parent, List<RuleApplication> rules, ParsePositionDelta examinedSoFar)
            {
                _parent = parent;
                _rules = rules;
                _examinedSoFar = examinedSoFar;
            }

            public override RuleApplication ResolveRecursion(RuleApplication baseApplication, ParseContext parseContext, ref ParsePosition position)
            {
                var examined = _examinedSoFar;
                var applications = new List<RuleApplication>(_rules);
                position = baseApplication.CurrentPosition;
                for (int i = _rules.Count; i < _parent.Rules.Length; i++)
                {
                    var rule = _parent.Rules[i];
                    var app = parseContext.Matcher.MatchCore(rule, parseContext, ref position);
                    examined = ParsePositionDelta.Larger(examined, app.ExaminedTo);
                    if (app.IsPositive)
                    {
                        applications.Add(app);
                    }
                    else
                    {
                        position = baseApplication.CurrentPosition + baseApplication.Length;
                        return baseApplication;
                    }
                }
                return _parent.CreateRuleApplication(baseApplication.CurrentPosition, applications, position - baseApplication.CurrentPosition, examined);
            }
        }
    }
}
