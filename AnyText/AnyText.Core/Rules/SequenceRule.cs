using NMF.AnyText.Model;
using NMF.AnyText.PrettyPrinting;
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
        public SequenceRule(params FormattedRule[] rules)
        {
            Rules = rules;
        }


        /// <inheritdoc />
        protected internal override bool CanStartWith(Rule rule, List<Rule> trace)
        {
            if (trace.Contains(this))
            {
                return false;
            }
            trace.Add(this);
            foreach (var inner in Rules.Select(f => f.Rule))
            {
                if (rule == inner || inner.CanStartWith(rule, trace)) return true;
                if (!inner.IsEpsilonAllowed()) return false;
            }
            return false;
        }

        /// <inheritdoc />
        protected internal override bool IsEpsilonAllowed(List<Rule> trace)
        {
            if (trace.Contains(this))
            {
                return false;
            }
            trace.Add(this);
            foreach (var rule in Rules)
            {
                if (!rule.Rule.IsEpsilonAllowed(trace)) return false;
            }
            return true;
        }

        /// <inheritdoc />
        public override bool HasFoldingKind(out string kind)
        {
            if (IsRegion())
            {
                kind = "region";
                return true;
            }

            if (IsFoldable())
            {
                kind = null;
                return true;
            }

            return base.HasFoldingKind(out kind);
        }

        /// <summary>
        /// The rules that should occur in sequence
        /// </summary>
        public FormattedRule[] Rules { get; set; }

        /// <inheritdoc />
        public override RuleApplication Match(ParseContext context, ref ParsePosition position)
        {
            var savedPosition = position;
            var beforeLast = position;
            var applications = new List<RuleApplication>();
            var examined = new ParsePositionDelta();
            List<RuleApplication> errors = null;
            foreach (var rule in Rules)
            {
                var app = context.Matcher.MatchCore(rule.Rule, context, ref position);
                var appExamined = (beforeLast + app.ExaminedTo) - savedPosition;
                examined = ParsePositionDelta.Larger(examined, appExamined);
                if (app.IsPositive)
                {
                    applications.Add(app);
                    beforeLast = position;
                    var indicator = app.PotentialError;
                    errors = AddToErrors(errors, indicator);
                }
                else
                {
                    position = savedPosition;
                    if (app is RecursiveRuleApplication recurse)
                    {
                        recurse.Continuations.Add(new Continuation(this, applications, examined));
                    }
                    errors = AddToErrors(errors, app);
                    return new FailedSequenceRuleApplication(this, errors, applications, savedPosition, default, examined);
                }
            }
            return CreateRuleApplication(applications.Count > 0 ? applications[0].CurrentPosition : savedPosition, applications, position - savedPosition, examined);
        }

        private static List<RuleApplication> AddToErrors(List<RuleApplication> errors, RuleApplication indicator)
        {
            if (indicator != null)
            {
                errors ??= new List<RuleApplication>();
                errors.Add(indicator);
            }

            return errors;
        }

        private IEnumerable<SynthesisRequirement>[] _synthesisRequirements;

        /// <summary>
        /// Gets or creates synthesis requirements for the individual rules
        /// </summary>
        /// <returns>An array with the synthesis requirements for the individual rules of the sequence</returns>
        protected IEnumerable<SynthesisRequirement>[] GetOrCreateSynthesisRequirements()
        {
            if (_synthesisRequirements == null)
            {
                _synthesisRequirements = new IEnumerable<SynthesisRequirement>[Rules.Length];
                for (int i = 0; i < Rules.Length; i++)
                {
                    _synthesisRequirements[i] = Rules[i].Rule.CreateSynthesisRequirements();
                }
            }
            return _synthesisRequirements;
        }

        /// <inheritdoc />
        public override IEnumerable<SynthesisRequirement> CreateSynthesisRequirements()
        {
            return GetOrCreateSynthesisRequirements().SelectMany(r => r);
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
        public override bool CanSynthesize(object semanticElement, ParseContext context)
        {
            return Array.TrueForAll(Rules, r => r.Rule.CanSynthesize(semanticElement, context));
        }

        /// <summary>
        /// Determines whether the current rule represents a region
        /// </summary>
        /// <returns>true, if it represents a region, otherwise false</returns>
        public bool IsRegion()
        {
            if (Rules[0].Rule is LiteralRule startLiteralRule && Rules[Rules.Length - 1].Rule is LiteralRule endLiteralRule)
            {
                return IsRegionStartLiteral(startLiteralRule.Literal) && IsMatchingEndLiteral(endLiteralRule.Literal, startLiteralRule.Literal);
            }
            return false;
        }

        /// <inheritdoc />
        public override bool IsFoldable()
        {
            if (Rules[0].Rule is LiteralRule startLiteralRule && Rules[Rules.Length-1].Rule is LiteralRule endLiteralRule)
            {
                return IsRangeStartLiteral(startLiteralRule.Literal) && IsMatchingEndLiteral(endLiteralRule.Literal, startLiteralRule.Literal);
            }
            return base.IsFoldable();
        }

        protected virtual bool IsRegionStartLiteral(string literal)
        {
            return literal == "#region";
        }

        protected virtual bool IsRangeStartLiteral(string literal)
        {
            return literal == "(" || literal == "[" || literal == "{";
        }

        protected virtual bool IsMatchingEndLiteral(string literal, string startLiteral)
        {
            switch (startLiteral)
            {
                case "#region": return literal == "#endregion";
                case "(": return literal == ")";
                case "[": return literal == "]";
                case "{": return literal == "}";
            }
            return false;
        }

        /// <inheritdoc />
        public override RuleApplication Synthesize(object semanticElement, ParsePosition position, ParseContext context)
        {
            if (semanticElement is ParseObject parseObject)
            {
                return SynthesizeParseObject(position, context, parseObject);
            }
            return SynthesizeCore(semanticElement, position, context);
        }

        internal RuleApplication SynthesizeParseObject(ParsePosition position, ParseContext context, ParseObject parseObject)
        {
            var currentPosition = position;
            var applications = new List<RuleApplication>();
            var requirements = GetOrCreateSynthesisRequirements();
            for (var i = 1; i < Rules.Length; i++)
            {
                foreach (var req in requirements[i])
                {
                    req.PlaceReservations(parseObject);
                }
            }
            var index = 1;
            foreach (var rule in Rules)
            {
                var app = rule.Rule.Synthesize(parseObject, position, context);
                if (app.IsPositive)
                {
                    applications.Add(app);
                    currentPosition += app.Length;
                    if (index < requirements.Length)
                    {
                        foreach (var req in requirements[index])
                        {
                            req.FreeReservations(parseObject);
                        }
                    }
                }
                else
                {
                    return new InheritedFailRuleApplication(this, app, default);
                }
                index++;
            }
            return CreateRuleApplication(position, applications, currentPosition - position, default);
        }

        private RuleApplication SynthesizeCore(object semanticElement, ParsePosition position, ParseContext context)
        {
            var currentPosition = position;
            var applications = new List<RuleApplication>();
            foreach (var rule in Rules)
            {
                var app = rule.Rule.Synthesize(semanticElement, position, context);
                if (app.IsPositive)
                {
                    applications.Add(app);
                    currentPosition += app.Length;
                }
                else
                {
                    return new InheritedFailRuleApplication(this, app, default);
                }
            }
            return CreateRuleApplication(position, applications, currentPosition - position, default);
        }


        internal override void Write(PrettyPrintWriter writer, ParseContext context, MultiRuleApplication ruleApplication)
        {
            var index = 0;
            foreach (var inner in ruleApplication.Inner)
            {
                inner.Write(writer, context);
                RuleHelper.ApplyFormattingInstructions(Rules[index].FormattingInstructions, writer);
                index++;
            }
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
                    var app = parseContext.Matcher.MatchCore(rule.Rule, parseContext, ref position);
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
