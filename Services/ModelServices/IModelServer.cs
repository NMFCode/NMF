using NMF.Expressions;
using NMF.Models.Repository;
using System;
using System.Collections.Generic;

namespace NMF.Models.Services
{
    /// <summary>
    /// Denotes an abstract interface for a model server
    /// </summary>
    public interface IModelServer
    {
        /// <summary>
        /// Gets or creates a session for the given model uri
        /// </summary>
        /// <param name="uri">The path to the model on the file system</param>
        /// <param name="path">The path to the model on the file system</param>
        /// <returns>The session created for this model element</returns>
        IModelSession GetOrCreateSession(Uri uri, string path);

        /// <summary>
        /// Gets or creates a session for the given model uri
        /// </summary>
        /// <param name="uri">The path to the model on the file system</param>
        /// <returns>The session created for this model element</returns>
        IModelSession GetOrCreateSession(Uri uri);

        /// <summary>
        /// Gets or creates a session for the given model uri
        /// </summary>
        /// <param name="uri">The path to the model on the file system</param>
        /// <param name="session">The session created for this model element</param>
        /// <returns>true, if the server contains a session for the given Uri, otherwise false</returns>
        bool TryGetSession(Uri uri, out IModelSession session);

        /// <summary>
        /// Gets the selected elements from this server
        /// </summary>
        IEnumerable<IModelElement> SelectedElements { get; }

        /// <summary>
        /// Gets the active session, i.e., the session that caused the current selection
        /// </summary>
        IModelSession ActiveSession { get; }

        /// <summary>
        /// Gets raised when the selected element changed
        /// </summary>
        event EventHandler SelectedElementChanged;

        /// <summary>
        /// Gets the model repository for this server
        /// </summary>
        ModelRepository Repository { get; }
    }
}