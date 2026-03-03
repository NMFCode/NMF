using NMF.AnyText.Rules;

namespace NMF.AnyText.Model
{
    /// <summary>
    /// Represents a rule application that resolves elements of type <typeparamref name="TReference"/>
    /// </summary>
    /// <typeparam name="TSemanticElement">the type of the semantic element</typeparam>
    /// <typeparam name="TReference">the type referenced by this resolution</typeparam>
    public class ResolveRuleApplication<TSemanticElement, TReference> : SingleRuleApplication
    {
        /// <summary>
        /// Creates a new instance
        /// </summary>
        /// <param name="rule">the rule referenced by this rule application</param>
        /// <param name="inner">the inner rule application</param>
        /// <param name="length">the length of the rule application</param>
        /// <param name="examinedTo">the amount of text that was used to conclude this rule application</param>
        public ResolveRuleApplication(Rule rule, RuleApplication inner, ParsePositionDelta length, ParsePositionDelta examinedTo) : base(rule, inner, length, examinedTo)
        {
        }

        /// <summary>
        /// Resolves this rule application with the given item under the given context
        /// </summary>
        /// <param name="resolved">the resolved element</param>
        /// <param name="context">the context in which the element is resolved</param>
        public virtual void Resolve(TReference resolved, ParseContext context)
        {
            Resolved = resolved;
        }

        /// <summary>
        /// Gets the resolved element
        /// </summary>
        public TReference Resolved { get; private set; }

        /// <inheritdoc/>
        public override object SemanticElement => Resolved;

        /// <summary>
        /// Gets the resolve error associated with this rule application
        /// </summary>
        public DiagnosticItem LastError => _lastError;

        private ResolveError<TSemanticElement, TReference> _lastError;

        internal void UpdateErrorMessage(string message, ParseContext context)
        {
            if (message == null)
            {
                if (_lastError != null)
                {
                    context.RemoveDiagnosticItem(LastError);
                }
                _lastError = null;
            }
            else
            {
                if (_lastError != null)
                {
                    _lastError.UpdateMessage(message);
                }
                else
                {
                    _lastError = new ResolveError<TSemanticElement, TReference>(DiagnosticSources.ResolveReferences, this, message);
                    context.AddDiagnosticItem(_lastError);
                }
            }
        }

        internal void ResetResolveError() => _lastError = null;
    }
}
