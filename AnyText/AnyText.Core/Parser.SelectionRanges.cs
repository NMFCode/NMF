using NMF.AnyText.Rules;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NMF.AnyText
{
    public partial class Parser
    {
        public ParsedSelectionRange[] GetSelectionRangesFromRoot()
        {
            Debugger.Break();
            RuleApplication rootApplication = Context.RootRuleApplication;

            if (rootApplication.IsPositive)
            {
                var parsedSelectionRanges = GetSelectionRangesFromSequence(rootApplication);
                return parsedSelectionRanges;
            }

            return Array.Empty<ParsedSelectionRange>();
        }

        private ParsedSelectionRange[] GetSelectionRangesFromSequence(RuleApplication ruleApplication)
        {
            var result = new List<ParsedSelectionRange>();

            return Array.Empty<ParsedSelectionRange>();
        }
    }

    public class ParsedSelectionRange
    {
        public SelectionRange Range;
        public ParsedSelectionRange? Parent;
    }

    public class SelectionRange
    {
        public SelectionPosition Start;
        public SelectionPosition End;
    }

    public class SelectionPosition
    {
        public uint Line;
        public uint Character;
    }
}
