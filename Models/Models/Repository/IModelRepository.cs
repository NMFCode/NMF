using System;

namespace NMF.Models.Repository
{
    /// <summary>
    /// Denotes a repository of model elements
    /// </summary>
    public interface IModelRepository
    {
        /// <summary>
        /// Resolves the given URI to a model element
        /// </summary>
        /// <param name="uri">the URI of the model element</param>
        /// <param name="loadOnDemand">true, if the model should be loaded in case it is not already in memory</param>
        /// <returns>the model element with the given URI or null, if it was not found</returns>
        IModelElement Resolve(Uri uri, bool loadOnDemand = true);

        /// <summary>
        /// Gets the collection of models managed by this repository
        /// </summary>
        ModelCollection Models { get; }

        /// <summary>
        /// Gets raised when there is any change in any of the models
        /// </summary>
        event EventHandler<BubbledChangeEventArgs> BubbledChange;
    }
}
