using NMF.AnyText.Rules;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NMF.AnyText
{
    /// <summary>
    /// Denotes an action that occurs when the parser
    /// </summary>
    public abstract class ParseResolveAction
    {
        /// <summary>
        /// Creates a new instance
        /// </summary>
        /// <param name="ruleApplication">the rule application for which the action is performed</param>
        /// <param name="resolveString">the resolve string</param>
        /// <param name="position">the position of the resolve action</param>
        protected ParseResolveAction(RuleApplication ruleApplication, string resolveString, ParsePosition position)
        {
            RuleApplication = ruleApplication;
            ResolveString = resolveString;
            Position = position;
        }

        /// <summary>
        /// The position of the resolution
        /// </summary>
        public ParsePosition Position { get; }

        /// <summary>
        /// Gets the rule application for which the action is performed
        /// </summary>
        public RuleApplication RuleApplication { get; }

        /// <summary>
        /// Gets the resolve string
        /// </summary>
        public string ResolveString { get; }

        /// <summary>
        /// Gets called when the parsing is complete
        /// </summary>
        /// <param name="parseContext">the parse context</param>
        public abstract void OnParsingComplete(ParseContext parseContext);
    }
}
