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
        ///     Retrieves semantic elements from the root application with delta encoding for Language Server Protocol (LSP).
        /// </summary>
        /// <returns>
        ///     A list of semantic tokens represented as unsigned integers,
        ///     including delta-encoded line and character positions, length, token type, and modifiers.
        /// </returns>
        public IEnumerable<uint> GetSemanticElementsFromRoot()
        {
            var semanticTokens = new List<uint>();

            var rootApplication = Context.RootRuleApplication;
            uint previousLine = 0;
            uint previousStartChar = 0;

            rootApplication.IterateLiterals(literalRuleApp =>
            {
                var line = (uint)literalRuleApp.CurrentPosition.Line;
                var startChar = (uint)literalRuleApp.CurrentPosition.Col;
                var modifiers = GetTokenModifierIndexFromHierarchy(literalRuleApp) ?? 0;
                var tokenType = GetTokenTypeIndexFromHierarchy(literalRuleApp) ?? 0;

                //split literal into lines for multiline literals to assign tokens (comments)
                var lines = literalRuleApp.Literal.Split(new[] { '\r', '\n' }, StringSplitOptions.None);
                var currentLine = line;
                foreach (var lineText in lines)
                {
                    if (string.IsNullOrWhiteSpace(lineText))
                    {
                        currentLine++;
                        continue;
                    }

                    var currentLength = (uint)lineText.Length;

                    var deltaLine = currentLine - previousLine;
                    var deltaStartChar = deltaLine == 0 ? startChar - previousStartChar : startChar;

                    semanticTokens.Add(deltaLine);
                    semanticTokens.Add(deltaStartChar);
                    semanticTokens.Add(currentLength);
                    semanticTokens.Add(tokenType);
                    semanticTokens.Add(modifiers);

                    previousLine = currentLine;
                    previousStartChar = startChar;

                    startChar = 0;
                    currentLine++;
                }
            });


            return semanticTokens;
        }

        /// <summary>
        ///     Traverses the parent hierarchy of a <see cref="RuleApplication" /> to find the first non-null TokenModifierIndex.
        /// </summary>
        /// <param name="application">The starting <see cref="RuleApplication" /> from which to begin the traversal.</param>
        /// <returns>
        ///     The first non-null TokenModifierIndex encountered in the hierarchy, or <c>null</c> if no non-null index is
        ///     found.
        /// </returns>
        private uint? GetTokenModifierIndexFromHierarchy(RuleApplication application)
        {
            while (application != null)
            {
                var rule = application.Rule;
                if (rule.TokenModifierIndex != null) return rule.TokenModifierIndex.Value;
                application = application.Parent;
            }

            return null;
        }

        /// <summary>
        ///     Traverses the parent hierarchy of a <see cref="RuleApplication" /> to find the first non-null TokenTypeIndex.
        /// </summary>
        /// <param name="application">The starting <see cref="RuleApplication" /> from which to begin the traversal.</param>
        /// <returns>The first non-null TokenTypeIndex encountered in the hierarchy, or <c>null</c> if no non-null index is found.</returns>
        private uint? GetTokenTypeIndexFromHierarchy(RuleApplication application)
        {
            while (application != null)
            {
                var rule = application.Rule;
                if (rule.TokenTypeIndex != null) return rule.TokenTypeIndex.Value;
                application = application.Parent;
            }

            return null;
        }




    }
}
