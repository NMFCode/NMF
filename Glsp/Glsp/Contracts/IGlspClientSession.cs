using NMF.Glsp.Protocol.BaseProtocol;
using System;
using System.Threading.Tasks;

namespace NMF.Glsp.Contracts
{
    /// <summary>
    /// Denotes a GLSP session
    /// </summary>
    public interface IGlspClientSession
    {
        /// <summary>
        /// Initializes the GLSP session
        /// </summary>
        /// <param name="messageHandler">A method that can be used to handle outgoing messages</param>
        /// <param name="clientId">The Client id</param>
        /// <returns>A task to support asynchronous operations</returns>
        Task InitializeAsync(Action<ActionMessage> messageHandler, string clientId);

        /// <summary>
        /// Processes the provided action asynchronously
        /// </summary>
        /// <param name="action">The action that needs to be processed</param>
        /// <returns>A task to support asynchronous operations</returns>
        Task ProcessAsync(BaseAction action);

        /// <summary>
        /// Disposes the resources used by this client session
        /// </summary>
        /// <returns>A task to support asynchronous operations</returns>
        Task DisposeAsync();
    }
}