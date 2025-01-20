namespace NMF.Expressions
{
    /// <summary>
    /// Denotes an interface for objects watching for changes
    /// </summary>
    public interface IChangeListener
    {
        /// <summary>
        /// Gets the target dependency graph node
        /// </summary>
        INotifiable Node { get; }

        /// <summary>
        /// Aggregates the changes
        /// </summary>
        /// <returns>A notification result aggregating the changes since the last propagation</returns>
        INotificationResult AggregateChanges();
    }
}
