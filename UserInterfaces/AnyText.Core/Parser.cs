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
            : this(new ParseContext(root, new Matcher(), stringComparison)) { }

        /// <summary>
        /// Creates a new parser system
        /// </summary>
        /// <param name="context">the parse context to use</param>
        public Parser(ParseContext context)
        {
            ArgumentNullException.ThrowIfNull(context);

            _context = context;
            _matcher = _context.Matcher;
        }

        /// <summary>
        /// Gets the parse context for this parser
        /// </summary>
        public ParseContext Context => _context;

        /// <summary>
        /// Initializes the parser system
        /// </summary>
        /// <param name="input">the initial input</param>
        /// <returns>the value parsed for the given input</returns>
        public object Initialize(string[] input)
        {
            _context.Input = input;
            _ruleApplication = _matcher.Match(_context);
            if (_ruleApplication.IsPositive)
            {
                _context.Root = _ruleApplication.GetValue(_context);
                _ruleApplication.Activate(_context);
                _context.RunResolveActions();
            }
            else
            {
                AddErrors(_ruleApplication);
            }
            return _context.Root;
        }

        private void AddErrors(RuleApplication ruleApplication)
        {
            _context.Errors.Add(new ParseError(ruleApplication.ErrorPosition, _ruleApplication.Message));
        }

        /// <summary>
        /// Updates the parse result with the given edits
        /// </summary>
        /// <param name="edits">A collection of edit operations</param>
        /// <returns>the updated value parsed for the given input</returns>
        public object Update(IEnumerable<TextEdit> edits)
        {
            var input = _context.Input;
            foreach (TextEdit edit in edits)
            {
                input = edit.Apply(input);
                _matcher.RemoveMemoizedRuleApplications(edit);
            }
            _context.Errors.Clear();
            _context.Input = input;
            var newRoot = _matcher.Match(_context);
            if (newRoot.IsPositive)
            {
                _ruleApplication = newRoot.ApplyTo(_ruleApplication, _context);
                _context.Root = _ruleApplication.GetValue(_context);
                _ruleApplication.Activate(_context);
                _context.RunResolveActions();
            }
            else
            {
                AddErrors(_ruleApplication);
            }
            return _context.Root;
        }
    }
}
