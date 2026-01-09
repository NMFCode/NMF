using NMF.AnyText.Grammars;
using NMF.AnyText.PrettyPrinting;
using NMF.AnyText.Rules;
using NMF.AnyText.Workspace;
using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.IO;
using System.Linq;
using System.Net.Http.Headers;

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
        /// <param name="fileUri">the Uri of the File</param>
        /// <returns>the value parsed for the given input</returns>
        public object Initialize(Uri fileUri)
        {
            var input = File.ReadAllLines(fileUri.LocalPath);
            Context.FileUri = fileUri;
            Context.CurrentDirectory = Path.GetDirectoryName(fileUri.LocalPath);
            return Initialize(input, false);
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
            try
            {
                _context.IsParsing = true;
                _context.IsExecutingModelChanges = true;
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
            }
            finally
            {
                _context.IsParsing = false;
                _context.IsExecutingModelChanges = false;
            }
            return _context.Root;
        }

        /// <summary>
        /// Initializes the parser system with a semantic object
        /// </summary>
        /// <param name="semanticObject">the semantic object</param>
        /// <param name="skipValidation">if set to true, the parser does not perform validation rules (default: false)</param>
        /// <exception cref="ArgumentException">thrown if no parse tree could be generated for the semantic object</exception>
        public void Initialize(object semanticObject, bool skipValidation = false)
        {
            var ruleApplication = _context.Grammar.Root.Synthesize(semanticObject, default, _context);
            if (!ruleApplication.IsPositive)
            {
                throw new ArgumentException("no parse tree could be created for this object.", nameof(semanticObject));
            }
            _context.Matcher.Reset();
            var writer = new StringWriter();
            var prettyWriter = new PrettyPrintWriter(writer, "  ");
            ruleApplication.Write(prettyWriter, _context);

            _context.Input = writer.ToString().Split(Environment.NewLine);
            try
            {
                _context.IsParsing = true;
                _context.IsExecutingModelChanges = false;
                ruleApplication.Activate(Context, true);
                _context.RootRuleApplication = ruleApplication;
                _context.RefreshRoot();
                UpdateCore(_context.Input, skipValidation);
            }
            finally
            {
                _context.IsParsing = false;
                _context.IsExecutingModelChanges = false;
            }
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
            _context.IsParsing = true;
            _context.IsExecutingModelChanges = true;
            try
            {
                var input = _context.Input;
                _context.RemoveAllErrors(e => e.Source == DiagnosticSources.Parser);

                input = edit.Apply(input);
                _matcher.Apply(edit);

                UpdateCore(input, skipValidation);
            }
            finally
            {
                _context.IsParsing = false;
                _context.IsExecutingModelChanges = false;
            }
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
            if (!Context.ShouldParseChange()) return _context.Root;
            Context.IsParsing = true;
            Context.IsExecutingModelChanges = true;
            try
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
            finally
            {
                Context.IsParsing = false;
                Context.IsExecutingModelChanges = false;
            }

        }

        /// <summary>
        /// Performs necessary changes to update the text according to the given model element
        /// </summary>
        /// <param name="updatedElement">a semantic model element that was updated outside the textual representation</param>
        /// <returns>A list of changes necessary to reflect the updated element</returns>
        public IReadOnlyList<TextEdit> Update(object updatedElement)
        {
            if (_context.IsParsing)
            {
                return null;
            }
            Context.IsParsing = true;
            Context.IsExecutingModelChanges = false;
            try
            {
                var edits = new List<TextEdit>();
                var input = Context.Input;
                if (TryGetDefinitionsAndReferences(Context, updatedElement, out var definitionsAndReferences))
                {
                    foreach (var definition in definitionsAndReferences.ToArray())
                    {
                        var oldApplication = definition.GetFirstReferenceOrDefinition();
                        var newApplication = oldApplication.Rule.Synthesize(updatedElement, oldApplication.CurrentPosition, Context);

                        if (!newApplication.IsPositive)
                        {
                            continue;
                        }

                        var position = oldApplication.CurrentPosition;
                        for (int i = 0; i < edits.Count; i++)
                        {
                            position = edits[i].AdjustPosition(position);
                        }
                        var edit = UpdateFromParseTree(oldApplication, position, newApplication);
                        if (edit != null)
                        {
                            input = edit.Apply(input);
                            _matcher.Apply(edit);
                            edits.Add(edit);
                        }
                    }
                }

                UpdateCore(input, false);

                return edits;
            }
            finally
            {
                Context.IsParsing = false;
                Context.IsExecutingModelChanges = false;
            }
        }

        private bool TryGetDefinitionsAndReferences(ParseContext context, object element, out IEnumerable<RuleApplication> definitionsAndReferences)
        {
            if (context.TryGetDefinitions(element, out var definitions))
            {
                if (context.TryGetReferences(element, out var references))
                {
                    definitionsAndReferences = definitions.Union(references);
                }
                else
                {
                    definitionsAndReferences = definitions;
                }
                return true;
            }
            else if (context.TryGetReferences(element, out var references))
            {
                definitionsAndReferences = references;
                return true;
            }
            definitionsAndReferences = null;
            return false;
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

        private TextEdit UpdateFromParseTree(RuleApplication oldParseTree, ParsePosition oldTreePosition, RuleApplication replaceWith, string indentString = null)
        {
            if (!oldParseTree.IsActive)
            {
                throw new ArgumentException("old tree must be active", nameof(oldParseTree));
            }
            indentString ??= "  ";
            var textWriter = new StringWriter();
            var writer = new PrettyPrintWriter(textWriter, indentString);
            oldParseTree.SetupPrettyPrinter(writer);
            replaceWith.Write(writer, _context);
            var lines = textWriter.ToString().TrimEnd(' ', '\r', '\n').Split(Environment.NewLine);

            if (oldParseTree == _context.RootRuleApplication)
            {
                _context.RootRuleApplication = replaceWith;
                _context.RefreshRoot();
            }
            else if (oldParseTree.Parent == null)
            {
                throw new InvalidOperationException();
            }
            else
            {
                oldParseTree.Parent.ReplaceChild(oldParseTree, replaceWith, _context);
            }

            oldParseTree.Deactivate(_context);
            replaceWith.Activate(_context, false);

            return CreateTextEdit(oldParseTree, oldTreePosition, lines);
        }

        private TextEdit CreateTextEdit(RuleApplication oldParseTree, ParsePosition startPosition, string[] lines)
        {
            var length = oldParseTree.Length;
            var lastLiteral = oldParseTree.GetLastInnerLiteral();
            if (lastLiteral != null)
            {
                length = lastLiteral.CurrentPosition + lastLiteral.Length - oldParseTree.CurrentPosition;
            }
            if (lines.Length == 0)
            {
                if (oldParseTree.Length == default)
                {
                    return null;
                }
                return new TextEdit(startPosition, startPosition + length, lines);
            }
            else if (length == default)
            {
                return new TextEdit(startPosition, startPosition, lines);
            }
            else if (length.Line == 0)
            {
                var prefixLength = MemoryExtensions.CommonPrefixLength(_context.Input[startPosition.Line].AsSpan(startPosition.Col), lines[0]);
                if (lines.Length == 1 && prefixLength == lines[0].Length && prefixLength == length.Col)
                {
                    return null;
                }
                if (prefixLength > 0)
                {
                    lines[0] = lines[0].Substring(prefixLength);
                    return new TextEdit(new ParsePosition(startPosition.Line, startPosition.Col + prefixLength), startPosition + length, lines);
                }
                return new TextEdit(startPosition, startPosition + length, lines);
            }
            else
            {
                var startOffset = 0;
                while (startOffset < length.Line && startOffset < lines.Length &&
                    LineIdentical(startPosition + new ParsePositionDelta(startOffset, 0), lines[startOffset]))
                {
                    startOffset++;
                }
                var endOffset = 0;
                if (startPosition.Line + length.Line < _context.Input.Length && _context.Input[startPosition.Line + length.Line].Substring(0, length.Col) == lines[lines.Length - 1])
                {
                    endOffset++;
                    while (endOffset < length.Line - startOffset && endOffset < lines.Length - startOffset &&
                        LineIdentical(startPosition + new ParsePositionDelta(length.Line - endOffset, 0), lines[lines.Length - endOffset - 1]))
                    {
                        endOffset++;
                    }
                }
                if (startOffset + endOffset == length.Line + 1 && lines.Length == length.Line + 1)
                {
                    return null;
                }
                if (startOffset + endOffset == 0)
                {
                    var commonPrefixLength = MemoryExtensions.CommonPrefixLength(_context.Input[startPosition.Line].AsSpan(startPosition.Col), lines[0]);
                    if (commonPrefixLength > 0)
                    {
                        lines[0] = lines[0].Substring(commonPrefixLength);
                        return new TextEdit(new ParsePosition(startPosition.Line, startPosition.Col + commonPrefixLength), startPosition + length, lines);
                    }
                    return new TextEdit(startPosition, startPosition + length, lines);
                }
                string[] newLines = TrimArray(lines, startOffset, endOffset);
                var endLine = length.Line - endOffset;
                var endCol = endLine == 0 ? length.Col : _context.Input[startPosition.Line + endLine].Length;
                return new TextEdit(startPosition + new ParsePositionDelta(startOffset, 0), startPosition + new ParsePositionDelta(endLine, endCol), newLines);
            }
        }

        private static string[] TrimArray(string[] lines, int startOffset, int endOffset)
        {
            if (startOffset + endOffset >= lines.Length)
            {
                return Array.Empty<string>();
            }
            var newLines = new string[lines.Length - startOffset - endOffset];
            Array.Copy(lines, startOffset, newLines, 0, newLines.Length);
            return newLines;
        }

        private bool LineIdentical(ParsePosition parsePosition, string newLine)
        {
            if (parsePosition.Col == 0)
            {
                return _context.Input[parsePosition.Line] == newLine;
            }
            else
            {
                return MemoryExtensions.Equals( _context.Input[parsePosition.Line].AsSpan(parsePosition.Col), newLine, StringComparison.Ordinal);
            }
        }
    }
}
