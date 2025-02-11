using NMF.AnyText.Grammars;
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
                Root = RootRuleApplication.GetValue(this);
            }
        }

        /// <summary>
        /// Gets or sets the current root rule application
        /// </summary>
        public RuleApplication RootRuleApplication { get; internal set; }

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
        public virtual void RunResolveActions()
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
        public bool GetDefinition(object key, out RuleApplication definition)
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
                _references[key] = new List<RuleApplication>() { value };
            }
        }

        /// <summary>
        /// Set the list of references for a rule application
        /// </summary>
        /// <param name="key">The semantic element of the referenced rule application</param>
        /// <param name="references">The reference rule applications to be stored</param>
        public void SetReferences(object key, ICollection<RuleApplication> references)
        {
            _references[key] = references;
        }

        /// <summary>
        /// Remove a reference of an object from the corresponding list of references
        /// </summary>
        /// <param name="key">The semantic element of the referenced rule application</param>
        /// <param name="value">The the referencing rule application to be removed</param>
        public void RemoveReference(object key, RuleApplication value)
        {
            if (_references.TryGetValue(key, out var references))
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
        public bool GetReferences(object key, out ICollection<RuleApplication> references)
        {
            return _references.TryGetValue(key, out references);
        }

        /// <summary>
        /// Gets the errors that occured while parsing
        /// </summary>
        public List<ParseError> Errors { get; } = new List<ParseError>();
    }
}
