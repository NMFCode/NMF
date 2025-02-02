﻿using NMF.AnyText.Grammars;
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
            : this(new ParseContext(new AdHocGrammar(root, null), new Matcher(), stringComparison)) { }

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
        /// Creates a new parser system
        /// </summary>
        /// <param name="grammar">the underlying grammar</param>
        public Parser(Grammar grammar) : this((grammar ?? throw new ArgumentNullException(nameof(grammar))).CreateParseContext()) { }

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
            _context.RootRuleApplication = _ruleApplication;
            if (_ruleApplication.IsPositive)
            {
                _context.Root = _ruleApplication.GetValue(_context);
                _ruleApplication.Activate(_context, default);
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
            var expected = new List<string>();

            foreach (var attempt in _matcher.GetErrorsExactlyAt(ruleApplication.ErrorPosition).Where(r => !r.IsPositive))
            {
                if (attempt.Rule.IsLiteral)
                {
                    expected.Add("'" + attempt.Message + "'");
                }
                else if (attempt.ErrorPosition != ruleApplication.ErrorPosition)
                {
                    AddErrors(attempt);
                }
            }
            var message = ruleApplication.Message;

            if (expected.Count > 0)
            {
                message += " Expected any of " + string.Join(", ", expected);
            }

            _context.Errors.Add(new ParseError(ParseErrorSources.Parser, ruleApplication.ErrorPosition, ruleApplication.Length, message));
        }

        /// <summary>
        /// Updates the parse result with the given edits
        /// </summary>
        /// <param name="edits">A collection of edit operations</param>
        /// <returns>the updated value parsed for the given input</returns>
        public object Update(IEnumerable<TextEdit> edits)
        {
            var input = _context.Input;
            _context.Errors.RemoveAll(e => e.Source == ParseErrorSources.Parser);
            foreach (TextEdit edit in edits)
            {
                input = edit.Apply(input);
                _matcher.Apply(edit);
                _context.Errors.RemoveAll(e => !e.ApplyEdit(edit));
            }
            _context.Input = input;
            var newRoot = _matcher.Match(_context);
            _context.RootRuleApplication = newRoot;
            if (newRoot.IsPositive)
            {
                _ruleApplication = newRoot.ApplyTo(_ruleApplication, default, _context);
                _context.Root = _ruleApplication.GetValue(_context);
                _ruleApplication.Activate(_context, default);
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
