using NMF.AnyText.PrettyPrinting;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NMF.AnyText.Rules
{
    /// <summary>
    /// Denotes a rule that matches a constant text
    /// </summary>
    public class LiteralRule : Rule
    {
        private readonly string _errorMessage;

        /// <summary>
        /// Creates a new instance
        /// </summary>
        /// <param name="literal">the literal that should be matched</param>
        public LiteralRule(string literal)
        {
            Literal = literal;
            _errorMessage = $"Expected '{literal}'";
        }

        /// <inheritdoc />
        public override bool IsLiteral => true;


        /// <inheritdoc />
        protected internal override bool CanStartWith(Rule rule, List<Rule> trace)
        {
            return false;
        }

        /// <inheritdoc />
        protected internal override bool IsEpsilonAllowed(List<Rule> trace)
        {
            return false;
        }

        /// <summary>
        /// Gets the literal that should be matched
        /// </summary>
        public string Literal { get; }

        /// <inheritdoc />
        public override RuleApplication Match(ParseContext context, ref ParsePosition position)
        {
            if (position.Line >= context.Input.Length)
            {
                return new FailedRuleApplication(this, default, _errorMessage);
            }
            var line = context.Input[position.Line];
            if (line.Length < position.Col + Literal.Length)
            {
                return new FailedRuleApplication(this, new ParsePositionDelta(0, Literal.Length), _errorMessage);
            }

            if (MemoryExtensions.Equals(Literal, line.AsSpan(position.Col, Literal.Length), context.StringComparison))
            {
                var res = new LiteralRuleApplication(this, Literal, new ParsePositionDelta(0, Literal.Length));
                position = position.Proceed(Literal.Length);
                return res;
            }

            var examinationLength = 0;
            while (MemoryExtensions.Equals(line.AsSpan(position.Col + examinationLength, 1), Literal.AsSpan(examinationLength, 1), context.StringComparison))
            {
                examinationLength++;
            }
            examinationLength++;

            return new FailedRuleApplication(this, new ParsePositionDelta(0, examinationLength), _errorMessage);
        }

        /// <inheritdoc />
        public override string TokenType  => !char.IsLetterOrDigit(Literal[0])? "operator": "keyword";

        /// <inheritdoc />
        public override bool CanSynthesize(object semanticElement, ParseContext context, SynthesisPlan synthesisPlan) => true;

        /// <inheritdoc />
        public override RuleApplication Synthesize(object semanticElement, ParsePosition position, ParseContext context) => new LiteralRuleApplication(this, Literal, default);


        /// <inheritdoc />
        public override IEnumerable<CompletionEntry> SuggestCompletions(ParseContext context, RuleApplication ruleApplication, string fragment, ParsePosition position)
        {
            if (!ruleApplication.IsPositive)
            {
                yield return new CompletionEntry(Literal, SymbolKind.Key, ruleApplication.CurrentPosition, position);
            }
        }
    }
}
