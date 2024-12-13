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
        protected ParseResolveAction(RuleApplication ruleApplication, string resolveString)
        {
            RuleApplication = ruleApplication;
            ResolveString = resolveString;
        }

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
