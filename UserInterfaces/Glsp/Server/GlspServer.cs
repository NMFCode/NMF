using NMF.Glsp.Protocol.BaseProtocol;
using NMF.Glsp.Protocol.Lifecycle;
using NMF.Glsp.Server.Contracts;
using NMF.Models.Services;
using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace NMF.Glsp.Server
{
    /// <summary>
    /// Denotes the default implementation of a GLSP server
    /// </summary>
    public class GlspServer : IGlspServer
    {
        private readonly ConcurrentDictionary<string, IGlspClientSession> _sessions = new ConcurrentDictionary<string, IGlspClientSession>();
        private readonly Dictionary<string, IClientSessionProvider> _sessionProviders;
        private readonly IModelServer _modelServer;

        /// <inheritdoc />
        public event EventHandler<ActionMessage> Process;

        /// <summary>
        /// Creates a new instance
        /// </summary>
        /// <param name="modelServer">the model server that should be used to serve requests</param>
        /// <param name="sessionProviders">A collection of session providers</param>
        public GlspServer(IModelServer modelServer, params IClientSessionProvider[] sessionProviders)
            : this(modelServer, (IEnumerable<IClientSessionProvider>)sessionProviders)
        {
        }

        /// <summary>
        /// Creates a new instance
        /// </summary>
        /// <param name="modelServer">the model server that should be used to serve requests</param>
        /// <param name="sessionProviders">A collection of session providers</param>
        public GlspServer(IModelServer modelServer, IEnumerable<IClientSessionProvider> sessionProviders)
        {
            _sessionProviders = sessionProviders?.ToDictionary(sp => sp.DiagramType);
            _modelServer = modelServer;
        }

        /// <inheritdoc/>
        public Task DisposeClientSessionAsync(string clientSessionId, IDictionary<string, object> args = null)
        {
            if (_sessions.TryRemove(clientSessionId, out var session))
            {
                return session.DisposeAsync();
            }
            return Task.CompletedTask;
        }

        /// <inheritdoc/>
        public Task InitializeClientSessionAsync(string clientSessionId, string diagramType, string[] clientActionKinds = null, IDictionary<string, object> args = null)
        {
            if (!_sessionProviders.TryGetValue(diagramType, out var sessionProvider)) throw new InvalidOperationException("Diagram type not supported");

            if (_sessions.TryGetValue(clientSessionId, out var session))
            {
                return Task.CompletedTask;
            }
            else
            {
                session = sessionProvider.CreateSession(args, _modelServer);
                _sessions.AddOrUpdate(clientSessionId, session, (_, _) => throw new InvalidOperationException("Session id already in use"));
                return session.InitializeAsync(SendToClient, clientSessionId);
            }
        }

        private void SendToClient(ActionMessage message)
        {
            Process?.Invoke(this, message);
        }

        /// <inheritdoc/>
        public Task ProcessAsync(string clientId, BaseAction action)
        {
            if (_sessions.TryGetValue(clientId, out var session))
            {
                return session.ProcessAsync(action);
            }
            else
            {
                throw new InvalidOperationException("Client is not known.");
            }
        }

        /// <inheritdoc/>
        public Task ShutdownAsync()
        {
            var disposeTasks = new List<Task>();
            foreach (var session in _sessions.Values)
            {
                disposeTasks.Add(session.DisposeAsync());
            }
            return Task.WhenAll(disposeTasks);
        }

        /// <inheritdoc/>
        public Task<InitializeResult> InitializeAsync(string applicationId, string protocolVersion, IDictionary<string, object> args = null)
        {
            return Task.FromResult(new InitializeResult
            {
                ProtocolVersion = "1.0.0",
                ServerActions = _sessionProviders.ToDictionary(sp => sp.Key, sp => (ICollection<string>)sp.Value.SupportedActions.ToList())
            });
        }
    }
}
