using NMF.AnyText.Grammars;
using NMF.AnyText.Rules;
using System;
using System.Collections.Generic;
using System.IO;

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
        /// <param name="filePath">the path to the input file</param>
        /// <param name="skipValidation">if set to true, the parser does not perform validation rules (default: false)</param>
        /// <returns>the value parsed for the given input</returns>
        public object Initialize(string filePath, bool skipValidation = false)
        {
            var path = filePath.Replace('\\', '/');
            var directory = path[..path.LastIndexOf('/')];
            _context.CurrentDirectory = directory;
            var input = File.ReadAllLines(path);
            return Initialize(input, skipValidation);
        }

        /// <summary>
        /// Initializes the parser system
        /// </summary>
        /// <param name="input">the initial input</param>
        /// <param name="skipValidation">if set to true, the parser does not perform validation rules (default: false)</param>
        /// <returns>the value parsed for the given input</returns>
        public object Initialize(string[] input, bool skipValidation = false)
        {
            _context.Input = input;
            _matcher.Reset();
            var ruleApplication = _matcher.Match(_context);
            _context.RootRuleApplication = ruleApplication;
            if (ruleApplication.IsPositive && !ruleApplication.IsRecovered)
            {
                _context.RefreshRoot();
                ruleApplication.Activate(_context, true);
                _context.RunResolveActions();
                if (!skipValidation)
                {
                    _context.RootRuleApplication.Validate(_context);
                }
            }
            else
            {
                AddErrors(ruleApplication);
            }
            return _context.Root;
        }

        private void AddErrors(RuleApplication ruleApplication)
        {
            ruleApplication.AddParseErrors(Context);
        }

        /// <summary>
        /// Updates the parse result with the given edit
        /// </summary>
        /// <param name="edit">An edit operations</param>
        /// <returns>the updated value parsed for the given input</returns>
        public object Update(TextEdit edit) => Update(edit, false);

        /// <summary>
        /// Updates the parse result with the given edit
        /// </summary>
        /// <param name="edit">An edit operations</param>
        /// <param name="skipValidation">if set to true, the parser does not perform validation rules</param>
        /// <returns>the updated value parsed for the given input</returns>
        public object Update(TextEdit edit, bool skipValidation)
        {
            var input = _context.Input;
            _context.RemoveAllErrors(e => e.Source == DiagnosticSources.Parser);

            input = edit.Apply(input);
            _matcher.Apply(edit);

            UpdateCore(input, skipValidation);
            return _context.Root;
        }

        /// <summary>
        /// Updates the parse result with the given edits
        /// </summary>
        /// <param name="edits">A collection of edit operations</param>
        /// <returns>the updated value parsed for the given input</returns>
        public object Update(IEnumerable<TextEdit> edits) => Update(edits, false);

        /// <summary>
        /// Updates the parse result with the given edits
        /// </summary>
        /// <param name="edits">A collection of edit operations</param>
        /// <param name="skipValidation">if set to true, the parser does not perform validation rules</param>
        /// <returns>the updated value parsed for the given input</returns>
        public object Update(IEnumerable<TextEdit> edits, bool skipValidation)
        {
            var input = _context.Input;
            _context.RemoveAllErrors(e => e.Source == DiagnosticSources.Parser);
            foreach (TextEdit edit in edits)
            {
                input = edit.Apply(input);
                _matcher.Apply(edit);
            }
            UpdateCore(input, skipValidation);
            return _context.Root;
        }

        private void UpdateCore(string[] input, bool skipValidation)
        {
            _context.Input = input;
            var newRoot = _matcher.Match(_context);
            if (newRoot.IsPositive && !newRoot.IsRecovered)
            {
                if (_context.LastSuccessfulRootRuleApplication != null)
                {
                    newRoot = newRoot.ApplyTo(_context.LastSuccessfulRootRuleApplication, _context);
                }
                _context.RootRuleApplication = newRoot;
                _context.RefreshRoot();
                newRoot.Activate(_context, false);
                _context.RunResolveActions();
                if (!skipValidation)
                {
                    _context.RootRuleApplication.Validate(_context);
                }
            }
            else
            {
                _context.RootRuleApplication = newRoot;
                AddErrors(newRoot);
            }
            _context.RemoveAllErrors(e => !e.CheckIfActiveAndExists(_context));
        }
    }
}
