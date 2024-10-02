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
        private readonly Queue<ParseResolveAction> _actions = new Queue<ParseResolveAction>();

        /// <summary>
        /// Creates a new instance
        /// </summary>
        /// <param name="rootRule">the root rule</param>
        /// <param name="matcher">the matcher for the context</param>
        /// <param name="stringComparison">the string comparison mode</param>
        public ParseContext(Rule rootRule, Matcher matcher, StringComparison stringComparison = StringComparison.OrdinalIgnoreCase)
        {
            RootRule = rootRule;
            Matcher = matcher;
            StringComparison = stringComparison;
        }

        /// <summary>
        /// Gets the root rule of this parse context
        /// </summary>
        public Rule RootRule { get; }

        /// <summary>
        /// Gets the semantic root of the parsed text
        /// </summary>
        public object Root { get; internal set; }

        /// <summary>
        /// Gets or sets the input text in lines
        /// </summary>
        public string[] Input { get; internal set; }

        /// <summary>
        /// Gets the matcher used in this parse context
        /// </summary>
        public Matcher Matcher { get; }

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
            _actions.Enqueue(action);
        }

        /// <summary>
        /// Runs all resolve actions
        /// </summary>
        public virtual void RunResolveActions()
        {
            while (_actions.Count > 0)
            {
                _actions.Dequeue().OnParsingComplete(this);
            }
        }

        /// <summary>
        /// Gets the errors that occured while parsing
        /// </summary>
        public ICollection<ParseError> Errors { get; } = new List<ParseError>();
    }
}
