namespace NMF.AnyText
{
    /// <summary>
    /// Denotes default error sources
    /// </summary>
    public static class DiagnosticSources
    {
        /// <summary>
        /// Denotes that an error occured while parsing
        /// </summary>
        public const string Parser = nameof(Parser);

        /// <summary>
        /// Denotes that an error occured while resolving references
        /// </summary>
        public const string ResolveReferences = nameof(ResolveReferences);

        /// <summary>
        /// Denotes that there is an error in the grammar
        /// </summary>
        public const string Grammar = nameof(Grammar);

        /// <summary>
        /// Denotes that there is an error appeared during validation
        /// </summary>
        public const string Validation = nameof(Validation);
    }
}
