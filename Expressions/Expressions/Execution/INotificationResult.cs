namespace NMF.Expressions
{
    /// <summary>
    /// Denotes the interface for a propagation result
    /// </summary>
    public interface INotificationResult
    {
        /// <summary>
        /// The source of this result
        /// </summary>
        INotifiable Source { get; }

        /// <summary>
        /// True, if there were changes, otherwise False
        /// </summary>
        bool Changed { get; }

        /// <summary>
        /// Increases the reference counter
        /// </summary>
        /// <param name="references">The number of references</param>
        void IncreaseReferences(int references);

        /// <summary>
        /// Frees a reference
        /// </summary>
        void FreeReference();
    }
}
