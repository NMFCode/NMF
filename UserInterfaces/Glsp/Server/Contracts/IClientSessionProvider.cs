using NMF.Models.Services;
using System.Collections.Generic;

namespace NMF.Glsp.Server.Contracts
{
    /// <summary>
    /// Denotes a GLSP session provider
    /// </summary>
    public interface IClientSessionProvider
    {
        /// <summary>
        /// The diagram type for which the provider can create sessions
        /// </summary>
        string DiagramType { get; }

        /// <summary>
        /// Creates a session
        /// </summary>
        /// <param name="args">arguments passed by the GLSP client</param>
        /// <param name="modelServer">the model server for which to create the session</param>
        /// <returns>A GLSP session</returns>
        IGlspClientSession CreateSession(IDictionary<string, object> args, IModelServer modelServer);

        /// <summary>
        /// Gets a collection with supported action kinds
        /// </summary>
        IEnumerable<string> SupportedActions { get; }
    }
}
