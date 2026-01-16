using NMF.AnyText.Rules;
using System;
using System.Diagnostics;

namespace NMF.AnyText
{
    /// <summary>
    /// Denotes an error while parsing
    /// </summary>
    [DebuggerDisplay("{Position} : {Message} ({Source} error)")]
#pragma warning disable S3881 // "IDisposable" should be implemented correctly
    public class DiagnosticItem : IDisposable
#pragma warning restore S3881 // "IDisposable" should be implemented correctly
    {
        /// <summary>
        /// Creates a new instance
        /// </summary>
        /// <param name="source">the source of the error</param>
        /// <param name="ruleApplication">the rule application that points to the error</param>
        /// <param name="message">the error message</param>
        /// <param name="severity">the severity of the diagnostics item</param>
        public DiagnosticItem(string source, RuleApplication ruleApplication, string message, DiagnosticSeverity severity = DiagnosticSeverity.Error)
        {
            Source = source;
            RuleApplication = ruleApplication;
            Message = message;
            Severity = severity;
        }

        /// <summary>
        /// Gets the severity of the diagnostic item
        /// </summary>
        public DiagnosticSeverity Severity { get; }


        /// <summary>
        /// Gets the source of the error
        /// </summary>
        public string Source { get; }

        /// <summary>
        /// Gets the rule application that indicates the error position
        /// </summary>
        public RuleApplication RuleApplication { get; }

        /// <summary>
        /// Gets the position of the error
        /// </summary>
        public ParsePosition Position => RuleApplication.CurrentPosition;

        /// <summary>
        /// Gets the length of the error
        /// </summary>
        public virtual ParsePositionDelta Length => RuleApplication.Length;

        /// <summary>
        /// Gets the error message
        /// </summary>
        public string Message { get; protected set; }

        internal bool CheckIfActiveAndExists(ParseContext context)
        {
            if (Source == DiagnosticSources.Parser || (context.RootRuleApplication.IsPositive && RuleApplication.IsActive && CheckIfStillExist(context)))
            {
                return true;
            }
            RuleApplication.RemoveDiagnosticItem(this);
            Dispose();
            return false;
        }

        /// <summary>
        /// Checks if the error still exists
        /// </summary>
        /// <param name="context">the parsing context</param>
        /// <returns>true, if the error still exists, otherwise false</returns>
        public virtual bool CheckIfStillExist(ParseContext context) => false;

        /// <inheritdoc />
        public override string ToString()
        {
            return $"{Source} at line {Position.Line}, col {Position.Col}: {Message}";
        }

        /// <inheritdoc />
        public virtual void Dispose()
        {
        }
    }
}
