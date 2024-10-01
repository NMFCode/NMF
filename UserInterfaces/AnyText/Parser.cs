using NMF.AnyText.Rules;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NMF.AnyText
{
    /// <summary>
    /// Denotes an incremental parser system
    /// </summary>
    public class Parser
    {
        private readonly Matcher _matcher;
        private readonly ParseContext _context;
        private RuleApplication _ruleApplication;

        /// <summary>
        /// Creates a new parser system
        /// </summary>
        /// <param name="root">the root rule</param>
        /// <param name="stringComparison">the string comparison mode</param>
        public Parser(Rule root, StringComparison stringComparison = StringComparison.OrdinalIgnoreCase)
        {
            _matcher = new Matcher();
            _context = new ParseContext(root, _matcher, stringComparison);
        }

        /// <summary>
        /// Initializes the parser system
        /// </summary>
        /// <param name="input">the initial input</param>
        /// <returns>the value parsed for the given input</returns>
        public object Initialize(string[] input)
        {
            _context.Input = input;
            _ruleApplication = _matcher.Match(_context);
            _ruleApplication.Activate(_context);
            return _ruleApplication.GetValue(_context);
        }

        public object Update(IEnumerable<TextEdit> edits)
        {
            var input = _context.Input;
            foreach (TextEdit edit in edits)
            {
                input = edit.Apply(input);
                _matcher.RemoveMemoizedRuleApplications(edit);
            }
            _context.Input = input;
            var newRoot = _matcher.Match(_context);
            if (newRoot.IsPositive)
            {
                _ruleApplication = newRoot.ApplyTo(_ruleApplication, _context);
                _ruleApplication.Activate(_context);
            }
            return _ruleApplication.GetValue(_context);
        }
    }
}
