using NMF.AnyText.Grammars;
using NMF.AnyText.Model;
using NMF.AnyText.Rules;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using NMF.AnyText.Workspace;

namespace NMF.AnyText
{
    /// <summary>
    /// The context in which a text is parsed
    /// </summary>
    public class ParseContext
    {
        private readonly List<Queue<ParseResolveAction>> _actions = new List<Queue<ParseResolveAction>>();
        private readonly List<DiagnosticItem> _errors = new List<DiagnosticItem>();
        private readonly Dictionary<object, RuleApplication> _definitions = new Dictionary<object, RuleApplication>();
        private readonly Dictionary<object, ICollection<RuleApplication>> _references = new Dictionary<object, ICollection<RuleApplication>>();

        /// <summary>
        /// Creates a new instance
        /// </summary>
        /// <param name="grammar">the grammar for this context</param>
        /// <param name="matcher">the matcher for the context</param>
        /// <param name="stringComparison">the string comparison mode</param>
        public ParseContext(Grammar grammar, Matcher matcher, StringComparison stringComparison = StringComparison.OrdinalIgnoreCase)
        {
            ArgumentNullException.ThrowIfNull(grammar);
            ArgumentNullException.ThrowIfNull(matcher);

            Grammar = grammar;
            Matcher = matcher;
            StringComparison = stringComparison;
            RootRuleApplication = new FailedRuleApplication(grammar.Root, default, "Not initialized");
        }

        /// <summary>
        /// Gets the grammar for this context
        /// </summary>
        public Grammar Grammar { get; }
        
        /// <summary>
        /// Gets the file-Uri for this context
        /// </summary>
        public Uri FileUri { get; internal set; }
        
        /// <summary>
        /// Gets the root rule of this parse context
        /// </summary>
        public Rule RootRule => Grammar.Root;

        /// <summary>
        /// Gets a collection of imports
        /// </summary>
        public List<string> Imports { get; } = new List<string>();

        /// <summary>
        /// Gets the semantic root of the parsed text
        /// </summary>
        public object Root { get; private set; }

        /// <summary>
        /// Refreshes the current root value
        /// </summary>
        public void RefreshRoot()
        {
            if (RootRuleApplication != null && RootRuleApplication.IsPositive)
            {
                LastSuccessfulRootRuleApplication = RootRuleApplication;
                Root = RootRuleApplication.GetValue(this);
            }
        }

        /// <summary>
        /// Gets or sets the current root rule application
        /// </summary>
        public RuleApplication RootRuleApplication { get; internal set; }

        /// <summary>
        /// Gets the last successful root rule application
        /// </summary>
        public RuleApplication LastSuccessfulRootRuleApplication { get; private set; }

        /// <summary>
        /// Gets or sets the input text in lines
        /// </summary>
        public string[] Input { get; internal set; }

        /// <summary>
        /// Gets the matcher used in this parse context
        /// </summary>
        public Matcher Matcher { get; }

        /// <summary>
        /// Indicates whether the last update sent to the parser was successful
        /// </summary>
        public bool IsLastUpdateSuccessful => RootRuleApplication.IsPositive;

        /// <summary>
        /// Gets the string comparison mode
        /// </summary>
        public StringComparison StringComparison { get; }

        /// <summary>
        /// Gets or sets a flag indicating whether the parser uses a synthesized model, preventing updates to semantic elements of a rule application.
        /// </summary>
        public bool UsesSynthesizedModel { get; set; } = false;
        
        /// <summary>
        /// Gets or sets the semantic model element that is being replaced during a CollectionReplace unify.
        /// </summary>
        public object ReplacedModelElement { get; set; }  = null;
        
        /// <summary>
        /// Gets or sets a flag indicating whether the parser is currently in the process of parsing.
        /// </summary>
        public bool IsParsing { get; set; } = false;

        /// <summary>
        /// Resolves the given input
        /// </summary>
        /// <param name="contextElement">the element in the context of which the string is resolved</param>
        /// <param name="input">the textual reference</param>
        /// <param name="resolved">the resolved reference or the default</param>
        /// <returns>true, if the reference could be resolved, otherwise false</returns>
        public virtual bool TryResolveReference<T>(object contextElement, string input, out T resolved)
        {
            resolved = default;
            return false;
        }

        /// <summary>
        /// Retrieves all potential references for a given context element.
        /// </summary>
        /// <typeparam name="T">The type of references to retrieve.</typeparam>
        /// <param name="contextElement">The context element.</param>
        /// <param name="input">The input from the user</param>
        /// <returns>A collection of references.</returns>
        public virtual IEnumerable<T> GetPotentialReferences<T>(object contextElement, string input) => null;


        /// <summary>
        /// Enqueues the given resolve action
        /// </summary>
        /// <param name="action">the resolve action</param>
        public virtual void EnqueueResolveAction(ParseResolveAction action)
        {
            var level = action.ResolveDelayLevel;
            while (_actions.Count <= level)
            {
                _actions.Add(new Queue<ParseResolveAction>());
            }
            _actions[level].Enqueue(action);
        }

        /// <summary>
        /// Runs all resolve actions
        /// </summary>
        internal void RunResolveActions()
        {
            foreach (var queue in _actions)
            {
                while (queue.Count > 0)
                {
                    queue.Dequeue().OnParsingComplete(this);
                }
            }
        }

        /// <summary>
        /// Add a rule application to the list of definitions in the document
        /// </summary>
        /// <param name="key">The semantic element of the rule application</param>
        /// <param name="value">The rule application</param>
        public void AddDefinition(object key, RuleApplication value)
        {
            _definitions[key] = value;
        }

        /// <summary>
        /// Get the rule application for a definition
        /// </summary>
        /// <param name="key">The semantic element of the rule application</param>
        /// <param name="definition">The rule application for the definition</param>
        /// <returns>True, if a definition is present for the given key</returns>
        public bool TryGetDefinition(object key, out RuleApplication definition)
        {
            return _definitions.TryGetValue(key, out definition);
        }

        /// <summary>
        /// Remove a rule application from the list of definitions
        /// </summary>
        /// <param name="key">The semantic element of the rule application</param>
        public void RemoveDefinition(object key)
        {
            _definitions.Remove(key);
        }

        /// <summary>
        /// Add a rule application to the list of references in the document
        /// </summary>
        /// <param name="key">The semantic element of the rule application</param>
        /// <param name="value">The rule application</param>
        public void AddReference(object key, RuleApplication value)
        {
            if (_references.TryGetValue(key, out var references))
            {
                references.Add(value);
            }
            else
            {
                _references[key] = new HashSet<RuleApplication>() { value };
            }
        }

        /// <summary>
        /// Remove a reference of an object from the corresponding list of references
        /// </summary>
        /// <param name="key">The semantic element of the referenced rule application</param>
        /// <param name="value">The the referencing rule application to be removed</param>
        public void RemoveReference(object key, RuleApplication value)
        {
            if (key != null && _references.TryGetValue(key, out var references))
            {
                references.Remove(value);
                if (references.Count == 0)
                {
                    _references.Remove(key);
                }
            }
        }

        /// <summary>
        /// Get the rule applications for references
        /// </summary>
        /// <param name="key">The semantic element of the referenced rule application</param>
        /// <param name="references">A list of rule applications that reference the rule application</param>
        /// <returns>True, if references are present for the given key</returns>
        public bool TryGetReferences(object key, out ICollection<RuleApplication> references)
        {
            return _references.TryGetValue(key, out references);
        }

        /// <summary>
        /// Calculates the context element for the given rule application
        /// </summary>
        /// <param name="ruleApplication">the rule application</param>
        /// <returns>A restored semantic context element or null, if it cannot be restored</returns>
        public object RestoreContextElement(RuleApplication ruleApplication)
        {
            if (ruleApplication.IsActive)
            {
                return ruleApplication.ContextElement;
            }

            if (LastSuccessfulRootRuleApplication == null)
            {
                return null;
            }

            var stack = new Stack<RuleApplication>();
            while (!ruleApplication.IsActive && ruleApplication.Parent != null)
            {
                stack.Push(ruleApplication);
                ruleApplication = ruleApplication.Parent;
            }

            if (stack.Count == 0) { return null; }

            if (!ruleApplication.IsActive)
            {
                stack.Push(ruleApplication);
                ruleApplication = LastSuccessfulRootRuleApplication;
            }
            var next = stack.Pop();
            while (stack.Count > 0)
            {
                if (next.Rule != ruleApplication.Rule || next.CurrentPosition != ruleApplication.CurrentPosition)
                {
                    break;
                }
                next = stack.Pop();
                var child = ruleApplication.FindChildAt(next.CurrentPosition, next.Rule);
                if (child == null)
                {
                    break;
                }
                else
                {
                    ruleApplication = child;
                }
            }
            return ruleApplication.ContextElement;
        }

        internal void AddAllErrors(IEnumerable<DiagnosticItem> diagnosticItems)
        {
            _errors.AddRange(diagnosticItems);
        }

        internal void RemoveAllErrors(Predicate<DiagnosticItem> predicate)
        {
            _errors.RemoveAll(predicate);
        }

        /// <summary>
        /// Adds the given diagnostic item
        /// </summary>
        /// <param name="diagnosticItem">the diagnostic item to add</param>
        public void AddDiagnosticItem(DiagnosticItem diagnosticItem)
        {
            ArgumentNullException.ThrowIfNull(diagnosticItem);

            _errors.Add(diagnosticItem);
            diagnosticItem.RuleApplication.AddDiagnosticItem(diagnosticItem);
        }

        /// <summary>
        /// Removes the given diagnostic item
        /// </summary>
        /// <param name="diagnosticItem">the diagnostic item</param>
        /// <returns>true, if the diagnostic item was present, otherwise false</returns>
        public bool RemoveDiagnosticItem(DiagnosticItem diagnosticItem)
        {
            ArgumentNullException.ThrowIfNull(diagnosticItem);

            if (_errors.Remove(diagnosticItem))
            {
                diagnosticItem.RuleApplication.RemoveDiagnosticItem(diagnosticItem);
                diagnosticItem.Dispose();
                return true;
            }
            return false;
        }

        /// <summary>
        /// Gets the errors that occured while parsing
        /// </summary>
        public IEnumerable<DiagnosticItem> Errors => _errors;

        protected internal virtual bool AcceptSequenceAdd(SequenceRule sequence, ref RuleApplication toAdd, List<RuleApplication> added)
        {
            return true;
        }

        protected internal virtual bool AcceptZeroOrMoreAdd(ZeroOrMoreRule star, RuleApplication toAdd, List<RuleApplication> added)
        {
            return true;
        }

        protected internal virtual bool AcceptOneOrMoreAdd(OneOrMoreRule rule, RuleApplication toAdd, List<RuleApplication> added)
        {
            return true;
        }

        private int _workspaceEditCount;
        
        /// <summary>
        /// Tracks and creates a new <see cref="WorkspaceEdit"/> with the provided text edits.
        /// </summary>
        /// <remarks>
        /// This method increments an internal counter to track the number of pending workspace edits.
        /// This is typically used to prevent the system from reparsing changes that were generated
        /// by the synchronization logic itself, thus avoiding infinite loops.
        /// </remarks>
        /// <param name="edit">An array of <see cref="TextEdit"/> objects representing the changes to be applied.</param>
        /// <param name="documentUri">The URI of the document to which the changes apply.</param>
        /// <returns>A new <see cref="WorkspaceEdit"/> instance containing the provided text edits.</returns>

        public WorkspaceEdit TrackAndCreateWorkspaceEdit(TextEdit[] edit, Uri documentUri)
        {
            Interlocked.Increment(ref _workspaceEditCount);
            if (documentUri == null) return null;
            
            var workspaceEdit = new WorkspaceEdit
            {
                Changes = new Dictionary<string, TextEdit[]>
                {
                    [documentUri.AbsoluteUri] = edit
                },
            };
            return workspaceEdit;

        }
        /// <summary>
        /// Determines whether a change should trigger a reparsing of the document.
        /// </summary>
        /// <remarks>
        /// This method is designed to prevent infinite loops in a synchronization system.
        /// It checks if there are any pending workspace edits that were generated by the system.
        /// </remarks>
        /// <returns>
        /// <c>false</c> if there are pending workspace edits from the system; otherwise, <c>true</c>.
        /// </returns>
        public bool ShouldParseChange()
        {
            if (Interlocked.CompareExchange(ref _workspaceEditCount, 0, 0) > 0)
            {
                Interlocked.Decrement(ref _workspaceEditCount);
                return false;
            }
            return true;
        }
    }
}
