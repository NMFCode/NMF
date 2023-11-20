using NMF.Glsp.Protocol.BaseProtocol;
using NMF.Glsp.Protocol.Lifecycle;
using NMF.Glsp.Server.Contracts;
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

        /// <inheritdoc />
        public event EventHandler<ActionMessage> Process;

        /// <summary>
        /// Creates a new instance
        /// </summary>
        /// <param name="sessionProviders">A collection of session providers</param>
        public GlspServer(params IClientSessionProvider[] sessionProviders)
            : this((IEnumerable<IClientSessionProvider>)sessionProviders)
        {
        }

        /// <summary>
        /// Creates a new instance
        /// </summary>
        /// <param name="sessionProviders">A collection of session providers</param>
        public GlspServer(IEnumerable<IClientSessionProvider> sessionProviders)
        {
            _sessionProviders = sessionProviders?.ToDictionary(sp => sp.DiagramType);
        }

        /// <inheritdoc/>
        public Task DisposeClientSessionAsync(DisposeClientSessionParameters parameters)
        {
            if (_sessions.TryRemove(parameters.ClientSessionId, out var session))
            {
                return session.DisposeAsync();
            }
            return Task.CompletedTask;
        }

        /// <inheritdoc/>
        public Task InitializeClientSessionAsync(InitializeClientSessionParameters parameters)
        {
            if (parameters == null) throw new ArgumentNullException(nameof(parameters));
            if (!_sessionProviders.TryGetValue(parameters.DiagramType, out var sessionProvider)) throw new InvalidOperationException("Diagram type not supported");

            var session = sessionProvider.CreateSession(parameters);
            _sessions.AddOrUpdate(parameters.ClientSessionId, session, (_,_) => throw new InvalidOperationException("Session id already in use"));
            return session.InitializeAsync(SendToClient, parameters.ClientSessionId);
        }

        private void SendToClient(ActionMessage message)
        {
            Process?.Invoke(this, message);
        }

        /// <inheritdoc/>
        public Task<InitializeResult> InitializeAsync(InitializeParameters parameters)
        {
            return Task.FromResult(new InitializeResult
            {
                ProtocolVersion = "1.0.0",
                ServerActions = _sessionProviders.ToDictionary(sp => sp.Key, sp => (ICollection<string>)sp.Value.SupportedActions.ToList())
            });
        }

        /// <inheritdoc/>
        public Task ProcessAsync(ActionMessage message)
        {
            if (message == null) throw new ArgumentNullException(nameof(message));

            if (_sessions.TryGetValue(message.ClientId, out var session))
            {
                return Task.Run(() => session.Process(message));
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
    }
}
