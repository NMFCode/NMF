﻿using NMF.AnyText.Model;
using NMF.AnyText.PrettyPrinting;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
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

        /// <inheritdoc/>
        public override string ToString()
        {
            if (GetType() == typeof(SequenceRule))
            {
                return string.Join(' ', Rules.Select(r => r.Rule.ToString()));
            }
            return base.ToString();
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
        /// Decides whether the provided rule application shall be accepted
        /// </summary>
        /// <param name="ruleApplication">the rule application that shall be accepted</param>
        /// <param name="ruleApplications">the rule applications accepted so far</param>
        /// <param name="context">the parse context in which the rule applications shall be accepted</param>
        /// <returns>true, if the rule application shall be accepted, otherwise false</returns>
        protected bool Accept(ref RuleApplication ruleApplication, List<RuleApplication> ruleApplications, ParseContext context)
        {
            return context.AcceptSequenceAdd(this, ref ruleApplication, ruleApplications);
        }

        /// <summary>
        /// The rules that should occur in sequence
        /// </summary>
        public FormattedRule[] Rules { get; set; }

        /// <inheritdoc />
        public override RuleApplication Match(ParseContext context, RecursionContext recursionContext, ref ParsePosition position)
        {
            var savedPosition = position;
            var beforeLast = position;
            var applications = new List<RuleApplication>();
            var examined = new ParsePositionDelta();
            List<RuleApplication> errors = null;
            foreach (var rule in Rules)
            {
                var app = context.Matcher.MatchCore(rule.Rule, recursionContext, context, ref position);
                var appExamined = (beforeLast + app.ExaminedTo) - savedPosition;
                examined = ParsePositionDelta.Larger(examined, appExamined);
                if (app.IsPositive && Accept(ref app, applications, context))
                {
                    applications.Add(app);
                    beforeLast = position;
                    var indicator = app.PotentialError;
                    errors = AddToErrors(errors, indicator);
                }
                else
                {
                    position = savedPosition;
                    if (recursionContext != null && IsLeftRecursive && recursionContext.Position == savedPosition && recursionContext.AddContinuations)
                    {
                        recursionContext.Continuations.Add(new Continuation(this, applications, examined, recursionContext.RuleStack));
                    }
                    return new FailedSequenceRuleApplication(this, app, errors, applications, savedPosition, examined);
                }
            }
            return CreateRuleApplication(savedPosition, applications, position - savedPosition, examined);
        }

        internal override MatchOrMatchProcessor NextMatchProcessor(ParseContext context, RecursionContext recursionContext, ref ParsePosition position)
        {
            return new MatchOrMatchProcessor(new SequenceMatchProcessor(this, position));
        }

        private sealed class SequenceMatchProcessor : MatchProcessor
        {
            private readonly SequenceRule _parent;
            private readonly List<RuleApplication> _applications = new List<RuleApplication>();
            private readonly ParsePosition _startPosition;
            private int _index;
            private ParsePositionDelta _examined;
            private List<RuleApplication> _errors;
            private ParsePosition _beforeLast;

            public SequenceMatchProcessor(SequenceRule parent, ParsePosition position)
            {
                _parent = parent;
                _beforeLast = position;
                _startPosition = position;
            }

            public override Rule Rule => _parent;

            public override MatchOrMatchProcessor NextMatchProcessor(ParseContext context, RuleApplication ruleApplication, ref ParsePosition position, ref RecursionContext recursionContext)
            {
                if (_index > 0 && ProcessRuleApplication(context, ref position, ref ruleApplication, recursionContext))
                {
                    return new MatchOrMatchProcessor(ruleApplication);
                }
                while (_index < _parent.Rules.Length)
                {
                    var rule = _parent.Rules[_index].Rule;
                    _index++;
                    var next = context.Matcher.MatchOrCreateMatchProcessor(rule, context, ref recursionContext, ref position);
                    if (next.IsMatch)
                    {
                        ruleApplication = next.Match;
                        if (ProcessRuleApplication(context, ref position, ref ruleApplication, recursionContext))
                        {
                            return new MatchOrMatchProcessor(ruleApplication);
                        }
                    }
                    else
                    {
                        return next;
                    }
                }
                return new MatchOrMatchProcessor(_parent.CreateRuleApplication(_startPosition, _applications, position - _startPosition, _examined));
            }

            private bool ProcessRuleApplication(ParseContext context, ref ParsePosition position, ref RuleApplication ruleApplication, RecursionContext recursionContext)
            {
                var appExamined = (_beforeLast + ruleApplication.ExaminedTo) - _startPosition;
                _examined = ParsePositionDelta.Larger(_examined, appExamined);
                var toAdd = ruleApplication;
                if (ruleApplication.IsPositive && _parent.Accept(ref ruleApplication, _applications, context))
                {
                    if (toAdd != ruleApplication)
                    {
                        position = _beforeLast + ruleApplication.Length;
                        context.Matcher.MoveOverWhitespaceAndComments(context, ref position);
                    }
                    _applications.Add(ruleApplication);
                    _beforeLast = position;
                    _errors = AddToErrors(_errors, ruleApplication.PotentialError);
                    ruleApplication = null;
                    return false;
                }
                else
                {
                    position = _startPosition;
                    if (recursionContext != null && _parent.IsLeftRecursive && recursionContext.Position == _startPosition && recursionContext.AddContinuations)
                    {
                        recursionContext.Continuations.Add(new Continuation(_parent, _applications, _examined, recursionContext.RuleStack));
                    }
                    ruleApplication = new FailedSequenceRuleApplication(_parent, ruleApplication, _errors, _applications, _startPosition, _examined);
                    return true;
                }
            }
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
            return new MultiRuleApplication(this, inner, length, examined);
        }

        /// <inheritdoc />
        public override bool CanSynthesize(object semanticElement, ParseContext context, SynthesisPlan synthesisPlan)
        {
            return Array.TrueForAll(Rules, r => r.Rule.CanSynthesize(semanticElement, context, synthesisPlan));
        }

        /// <summary>
        /// Determines whether the current rule represents a region
        /// </summary>
        /// <returns>true, if it represents a region, otherwise false</returns>
        public virtual bool IsRegion()
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

        /// <summary>
        /// Checks if a literal denotes the start of a region
        /// </summary>
        /// <param name="literal">The start literal</param>
        /// <returns>True, if the given literal denotes the start of a region</returns>
        public virtual bool IsRegionStartLiteral(string literal)
        {
            return literal == "#region";
        }

        /// <summary>
        /// Checks if a given literal denotes the start of a generic folding range
        /// </summary>
        /// <param name="literal"></param>
        /// <returns>True, if the given literal denotes the start of a generic foldable range</returns>
        public virtual bool IsRangeStartLiteral(string literal)
        {
            return literal == "(" || literal == "[" || literal == "{";
        }

        /// <summary>
        /// Checks if a given pair of start and end literal match
        /// </summary>
        /// <param name="literal">The end literal</param>
        /// <param name="startLiteral">The start literal</param>
        /// <returns>True, if the given literal is a matching end literal for a given start literal</returns>
        public virtual bool IsMatchingEndLiteral(string literal, string startLiteral)
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

            public Continuation(SequenceRule parent, List<RuleApplication> rules, ParsePositionDelta examinedSoFar, IEnumerable<Rule> ruleStack)
                : base(ruleStack)
            {
                _parent = parent;
                _rules = rules;
                _examinedSoFar = examinedSoFar;
            }

            public override RuleApplication ResolveRecursion(RuleApplication baseApplication, ParseContext parseContext, RecursionContext recursionContext, ref ParsePosition position)
            {
                var examined = _examinedSoFar;
                var applications = new List<RuleApplication>(_rules);
                position = baseApplication.CurrentPosition;
                for (int i = _rules.Count; i < _parent.Rules.Length; i++)
                {
                    var rule = _parent.Rules[i];
                    var app = parseContext.Matcher.MatchCore(rule.Rule, recursionContext, parseContext, ref position);
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

            public override string ToString()
            {
                return $"recurse with {_parent}";
            }
        }
    }
}
