using NMF.AnyText.Rules;
using System.Collections.Generic;

namespace NMF.AnyText
{
    internal sealed class MemoColumn
    {
        public MemoColumn(int col, MemoLine line)
        {
            Column = col;
            Line = line;
        }

        public int Column { get; }

        public MemoLine Line { get; }

        public Dictionary<Rule, RuleApplication> Applications { get; } = new Dictionary<Rule, RuleApplication>();

        public List<RuleApplication> Comments { get; set; }
    }
}
