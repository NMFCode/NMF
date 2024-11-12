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
    public class Parser
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
            return _context.Root;
        }

        /// <summary>
        /// Retrieves semantic elements from the root application with delta encoding for LSP.
        /// </summary>
        /// <returns>A list of semantic elements from the root application.</returns>
        public IEnumerable<uint> GetSemanticElementsFromRoot()
        {
            var semanticTokens = new List<uint>();

            RuleApplication rootApplication = Context.RootRuleApplication;
            uint previousLine = 0;
            uint previousStartChar = 0;
            GetSemanticElementsFromApplication(rootApplication, semanticTokens, ref previousLine, ref previousStartChar);

            return semanticTokens;
        }
        /// <summary>
        /// Recursively retrieves semantic elements from a rule application with delta encoding for LSP.
        /// </summary>
        /// <param name="application">The rule application to process.</param>
        /// <param name="semanticTokens">A list to store semantic elements.</param>
        /// <param name="previousLine">Tracks the previous line for delta encoding.</param>
        /// <param name="previousChar">Tracks the previous start character for delta encoding.</param>
        /// <returns>A list of semantic elements.</returns>
        private IList<uint> GetSemanticElementsFromApplication(RuleApplication application, IList<uint> semanticTokens, ref uint previousLine, ref uint previousChar)
        {
            if (application.IsPositive)
            {
                var rule = application.Rule;
                // var semanticElement = application.GetValue(_context);

                if (rule.TokenType != null)

                {
                    uint length = (uint)application.Length.Col;
                    uint line = (uint)application.CurrentPosition.Line;
                    uint startChar = (uint)application.CurrentPosition.Col;
                    uint modifiers = rule.TokenModifierIndex ?? 0;
                    uint tokenType = rule.TokenTypeIndex ?? 0;

                    uint deltaLine = line - previousLine;
                    uint deltaStartChar = deltaLine == 0 ? startChar - previousChar : startChar;

                    semanticTokens.Add(deltaLine);
                    semanticTokens.Add(deltaStartChar);
                    semanticTokens.Add(length);
                    semanticTokens.Add(tokenType);
                    semanticTokens.Add(modifiers);

                    previousLine = line;
                    previousChar = startChar;


                }

                // Traverse Syntax Tree
                if (application is MultiRuleApplication multiApp)
                {
                    foreach (var child in multiApp.Inner)
                    {
                        GetSemanticElementsFromApplication(child, semanticTokens, ref previousLine, ref previousChar);
                    }
                }
                else if (application is SingleRuleApplication singleApp)
                {
                    if (singleApp.Inner != null)
                    {
                        GetSemanticElementsFromApplication(singleApp.Inner, semanticTokens, ref previousLine, ref previousChar);
                    }
                }
            }

            return semanticTokens;
        }


    }
}
