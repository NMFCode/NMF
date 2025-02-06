using NMF.AnyText.Grammars;
using NMF.AnyText.Model;
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
    public partial class Parser
    {
        private readonly Matcher _matcher;
        private readonly ParseContext _context;

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
            _matcher.Reset();
            var ruleApplication = _matcher.Match(_context);
            _context.RootRuleApplication = ruleApplication;
            if (ruleApplication.IsPositive)
            {
                _context.RefreshRoot();
                ruleApplication.Activate(_context);
                _context.RunResolveActions();
            }
            else
            {
                AddErrors(ruleApplication);
            }
            return _context.Root;
        }

        private void AddErrors(RuleApplication ruleApplication)
        {
            _context.Errors.AddRange(ruleApplication.CreateParseErrors());
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
            }
            _context.Input = input;
            var newRoot = _matcher.Match(_context);
            if (newRoot.IsPositive)
            {
                newRoot = newRoot.ApplyTo(_context.RootRuleApplication, _context);
                _context.RootRuleApplication = newRoot;
                _context.RefreshRoot();
                newRoot.Activate(_context);
                _context.RunResolveActions();
            }
            else
            {
                _context.RootRuleApplication = newRoot;
                AddErrors(newRoot);
            }
            _context.Errors.RemoveAll(e => !e.CheckIfActiveAndExists(_context));
            return _context.Root;
        }
    }
}
