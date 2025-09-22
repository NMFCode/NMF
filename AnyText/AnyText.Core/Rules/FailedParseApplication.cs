using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NMF.AnyText.Rules
{
    internal class FailedParseApplication : FailedRuleApplication
    {
        private readonly RuleApplication _inner;

        public FailedParseApplication(Rule rule, RuleApplication inner, ParsePositionDelta examinedTo, string message) : base(rule, examinedTo, message)
        {
            _inner = inner;
        }

        public override IEnumerable<RuleApplication> Children => Enumerable.Repeat(_inner, 1);

        internal override IEnumerable<CompletionEntry> SuggestCompletions(ParsePosition position, string fragment, ParseContext context, ParsePosition nextTokenPosition)
        {
            return _inner.SuggestCompletions(position, fragment, context, nextTokenPosition);
        }

        public override RuleApplication Recover(RuleApplication currentRoot, ParseContext context, out ParsePosition position)
        {
            return _inner.Recover(currentRoot, context, out position);
        }

        public override void IterateLiterals(Action<LiteralRuleApplication> action, bool includeFailures)
        {
            _inner.IterateLiterals(action, includeFailures);
        }

        public override void IterateLiterals<T>(Action<LiteralRuleApplication, T> action, T parameter, bool includeFailures)
        {
            _inner.IterateLiterals(action, parameter, includeFailures);
        }

        public override void AddParseErrors(ParseContext context)
        {
            _inner.AddParseErrors(context);
            base.AddParseErrors(context);
        }

        internal override void AddDocumentSymbols(ParseContext context, ICollection<DocumentSymbol> result)
        {
            _inner.AddDocumentSymbols(context, result);
        }

        internal override void AddCodeLenses(ICollection<CodeLensApplication> codeLenses, Predicate<RuleApplication> predicate = null)
        {
            _inner.AddCodeLenses(codeLenses, predicate);
        }

        public override RuleApplication GetLiteralAt(ParsePosition position, bool onlyActive = false)
        {
            return _inner.GetLiteralAt(position, onlyActive);
        }
    }
}
