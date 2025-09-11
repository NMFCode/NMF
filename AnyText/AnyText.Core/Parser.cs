using NMF.AnyText.Grammars;
using NMF.AnyText.Model;
using NMF.AnyText.Rules;
using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.IO;
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
        /// <param name="fileUri">the Uri of the File</param>
        /// <returns>the value parsed for the given input</returns>
        public object Initialize(Uri fileUri)
        {
            var input = File.ReadAllLines(fileUri.AbsolutePath);
            Context.FileUri = fileUri;
            return Initialize(input, false);
        }
        /// <summary>
        /// Initializes the parser system
        /// </summary>
        /// <param name="input">the initial input</param>
        /// <returns>the value parsed for the given input</returns>
        public object Initialize(string[] input) => Initialize(input, false);

        /// <summary>
        /// Initializes the parser system
        /// </summary>
        /// <param name="input">the initial input</param>
        /// <param name="skipValidation">if set to true, the parser does not perform validation rules</param>
        /// <returns>the value parsed for the given input</returns>
        public object Initialize(string[] input, bool skipValidation)
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
            _context.AddAllErrors(ruleApplication.CreateParseErrors());
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
            var textEdits = edits.ToList();
            if (!Context.ShouldParseChange()) return _context.Root;
            Context.IsParsing = true;

            try
            {
                var input = _context.Input;
                _context.RemoveAllErrors(e => e.Source == DiagnosticSources.Parser);
                foreach (TextEdit edit in textEdits)
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
            }

        }

        private void UpdateCore(string[] input, bool skipValidation)
        {
            _context.Input = input;
            var newRoot = _matcher.Match(_context);
            if (newRoot.IsPositive)
            {
                if (_context.LastSuccessfulRootRuleApplication != null)
                {
                    newRoot = newRoot.ApplyTo(_context.LastSuccessfulRootRuleApplication, _context);
                }
                _context.RootRuleApplication = newRoot;
                _context.RefreshRoot();
                newRoot.Activate(_context);
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
        /// <summary>
        /// Initializes the root context for unification with the provided rule application and input.
        /// Does not change the semantic elements of the provided rule application.
        /// </summary>
        /// <param name="app">The root rule application to initialize the context with.</param>
        /// <param name="input">The input text to parse.</param>
        /// <param name="uri">The uri of the file.</param>

        public void UnifyInitialize(RuleApplication app, string input, Uri uri, bool overwrite = false)
        {
            if (!app.IsPositive) return;

            var model = app.SemanticElement;
            if (model == null) return;

            if (_context.Root == null || overwrite)
                InitializeRootContext(app, input, uri);
        }
        /// <summary>
        /// Unififies the provided RuleApplication with the correspondig ruleapplication of the parser and applies the TextEdits.
        /// Does not change any semantic elements of the provided rule applications.
        /// </summary>
        /// <param name="app">The rule application to unify.</param>
        /// <param name="edits">The array of text edits to apply.</param>
        /// <param name="isCollectionChange">Indicates whether the unification is for a collection change.</param>
        /// <param name="action">The collection change action, if applicable.</param>
        public void Unify(RuleApplication app, TextEdit[] edits, bool isCollectionChange = false,
            NotifyCollectionChangedAction? action = null)
        {
            if (!app.IsPositive || edits.Length == 0 || app.SemanticElement == null) return;
            if (!_context.TryGetDefinition(app.SemanticElement, out var definition)) return;

            var currentApp = definition;

            if (isCollectionChange && action.HasValue)
            {
                switch (action.Value)
                {
                    case NotifyCollectionChangedAction.Add:
                        ApplyInsertion(app, currentApp.Parent, edits);
                        break;
                    case NotifyCollectionChangedAction.Remove:
                        ApplyRemove(currentApp.Parent, edits);
                        break;
                    case NotifyCollectionChangedAction.Replace:
                        break;
                    case NotifyCollectionChangedAction.Move:
                        break;
                    case NotifyCollectionChangedAction.Reset:
                        ApplyUpdate(app, currentApp, edits);
                        break;
                }
            }
            else
            {
                ApplyUpdate(app, currentApp, edits);
            }
        }

        private void InitializeRootContext(RuleApplication rootApp, string input, Uri uri)
        {
            _matcher.Reset();
            _context.Input = input.Split(Environment.NewLine);
            _context.RootRuleApplication = rootApp;
            _context.RefreshRoot();
            _context.UsesSynthesizedModel = true;
            var matchedRoot = _matcher.Match(_context);
            var newApp = matchedRoot.ApplyTo(rootApp, _context);
            newApp.SetActivate(true, _context);
            _context.FileUri = uri;
            _context.RootRuleApplication = newApp;
            _context.RefreshRoot();
            _context.RunResolveActions();
        }
        
        private void ApplyInsertion(RuleApplication synthesized, RuleApplication currentApp, TextEdit[] edits)
        {
            var tempPosition = currentApp.CurrentPosition;

            ApplyEditsAndPrepareContext(currentApp, edits);

            var matched = _matcher.MatchCore(synthesized.Rule, null, _context, ref tempPosition);
            synthesized.ApplyTo(currentApp, _context);
            matched.ApplyTo(currentApp, _context);
            FinalizeUnification(currentApp);
        }

        private void ApplyUpdate(RuleApplication synthesized, RuleApplication currentApp, TextEdit[] edits)
        {
            var tempPosition = currentApp.CurrentPosition;

            ApplyEditsAndPrepareContext(currentApp, edits);

            var matched = _matcher.MatchCore(synthesized.Rule, null, _context, ref tempPosition);
            matched.ApplyTo(currentApp, _context);
            FinalizeUnification(currentApp);


        }

        private void ApplyRemove(RuleApplication currentApp, TextEdit[] edits)
        {
            var tempPos = currentApp.CurrentPosition;

            ApplyEditsAndPrepareContext(currentApp, edits);

            var matched = _matcher.MatchCore(currentApp.Rule, null, _context, ref tempPos);
            matched.ApplyTo(currentApp, _context);
            FinalizeUnification(currentApp);

        }

        private void ApplyEditsAndPrepareContext(RuleApplication currentApp, TextEdit[] edits)
        {
            foreach (var edit in edits)
            {
                _context.Input = edit.Apply(_context.Input);
                _matcher.Apply(edit);
            }
            currentApp.SetActivate(false, _context);
        }

        private void FinalizeUnification(RuleApplication newApp)
        {
            newApp.SetActivate(true, _context);
            _context.RunResolveActions();
        }
    }
}
