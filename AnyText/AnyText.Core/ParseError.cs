using NMF.AnyText.Rules;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NMF.AnyText
{
    /// <summary>
    /// Denotes an error while parsing
    /// </summary>
    [DebuggerDisplay("{Position} : {Message} ({Source} error)")]
    public class ParseError
    {
        /// <summary>
        /// Creates a new instance
        /// </summary>
        /// <param name="source">the source of the error</param>
        /// <param name="ruleApplication">the rule application that points to the error</param>
        /// <param name="message">the error message</param>
        public ParseError(string source, RuleApplication ruleApplication, string message)
        {
            Source = source;
            RuleApplication = ruleApplication;
            Message = message;
        }


        /// <summary>
        /// Gets the source of the error
        /// </summary>
        public string Source { get; }

        public RuleApplication RuleApplication { get; }

        /// <summary>
        /// Gets the position of the error
        /// </summary>
        public ParsePosition Position => RuleApplication.CurrentPosition;

        /// <summary>
        /// Gets the length of the error
        /// </summary>
        public ParsePositionDelta Length => RuleApplication.Length;

        /// <summary>
        /// Gets the error message
        /// </summary>
        public string Message { get; protected set; }

        internal bool CheckIfActiveAndExists(ParseContext context) => RuleApplication.IsActive && CheckIfStillExist(context);

        /// <summary>
        /// Checks if the error still exists
        /// </summary>
        /// <param name="context">the parsing context</param>
        /// <returns>true, if the error still exists, otherwise false</returns>
        protected virtual bool CheckIfStillExist(ParseContext context) => true;

        /// <inheritdoc />
        public override string ToString()
        {
            return $"{Source} at line {Position.Line}, col {Position.Col}: {Message}";
        }
    }
}
