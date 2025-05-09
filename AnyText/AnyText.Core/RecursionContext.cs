using NMF.AnyText.Rules;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NMF.AnyText
{
    public class RecursionContext
    {
        public RecursionContext(ParsePosition position)
        {
            Position = position;
            AllowLeftRecursion = true;
            Continuations = new List<RecursiveContinuation>();
            RuleStack = new Stack<Rule>();
        }

        public Stack<Rule> RuleStack { get; }

        public List<RecursiveContinuation> Continuations { get; }

        public ParsePosition Position { get; }

        public bool AllowLeftRecursion { get; private set; }

        internal void BlockLeftRecursion()
        {
            AllowLeftRecursion = false;
        }
    }
}
