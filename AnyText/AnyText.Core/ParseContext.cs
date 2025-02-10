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
    /// The context in which a text is parsed
    /// </summary>
    public class ParseContext
    {
        private readonly List<Queue<ParseResolveAction>> _actions = new List<Queue<ParseResolveAction>>();

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
            RootRuleApplication = new FailedRuleApplication(grammar.Root, default, default, "Not initialized");
        }

        /// <summary>
        /// Gets the grammar for this context
        /// </summary>
        public Grammar Grammar { get; }

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
        /// Calculates the context element for the given rule application
        /// </summary>
        /// <param name="ruleApplication">the rule application</param>
        /// <returns>A restored semantic context element or null, if it cannot be restored</returns>
        public object RestoreContextElement(RuleApplication ruleApplication)
        {
            if (ruleApplication.IsPositive)
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

        /// <summary>
        /// Gets the errors that occured while parsing
        /// </summary>
        public List<DiagnosticItem> Errors { get; } = new List<DiagnosticItem>();
    }
}
