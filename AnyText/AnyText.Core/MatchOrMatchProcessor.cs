using NMF.AnyText.Rules;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NMF.AnyText
{
    internal readonly struct MatchOrMatchProcessor
    {
        private readonly RuleApplication _match;
        private readonly MatchProcessor _matchProcessor;

        public MatchOrMatchProcessor(RuleApplication match)
        {
            _match = match;
        }

        public MatchOrMatchProcessor(MatchProcessor matchProcessor)
        {
            _matchProcessor = matchProcessor;
        }

        public bool IsMatch => _match != null;

        public bool IsMatchProcessor => _matchProcessor != null;

        public MatchProcessor MatchProcessor => _matchProcessor;

        public RuleApplication Match => _match;
    }
}
