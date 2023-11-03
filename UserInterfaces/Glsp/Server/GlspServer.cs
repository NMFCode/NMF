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
    public class GlspServer : IGlspServer
    {
        private readonly ConcurrentDictionary<string, ClientSession> _sessions = new ConcurrentDictionary<string, ClientSession>();
        private readonly Dictionary<string, IClientSessionProvider> _sessionProviders;

        public GlspServer(IEnumerable<IClientSessionProvider> sessionProviders)
        {
            _sessionProviders = sessionProviders?.ToDictionary(sp => sp.DiagramType);
        }

        /// <inheritdoc/>
        public Task DisposeClientSession(DisposeClientSessionParameters parameters)
        {
            if (_sessions.TryRemove(parameters.ClientSessionId, out var session))
            {
                return session.DisposeAsync();
            }
            return Task.CompletedTask;
        }

        /// <inheritdoc/>
        public Task InitializeClientSession(InitializeClientSessionParameters parameters)
        {
            if (parameters == null) throw new ArgumentNullException(nameof(parameters));
            if (!_sessionProviders.TryGetValue(parameters.DiagramType, out var sessionProvider)) throw new InvalidOperationException("Diagram type not supported");

            var session = sessionProvider.CreateSession(parameters);
            _sessions.AddOrUpdate(parameters.ClientSessionId, session, (_,_) => throw new InvalidOperationException("Session id already in use"));
            return session.InitializeAsync();
        }

        /// <inheritdoc/>
        public Task<InitializeResult> InitializeServer(InitializeParameters parameters)
        {
            return Task.FromResult(new InitializeResult
            {
                ProtocolVersion = "1.0.0",
                ServerActions = _sessionProviders.ToDictionary(sp => sp.Key, sp => (ICollection<string>)sp.Value.SupportedActions.ToList())
            });
        }

        /// <inheritdoc/>
        public void SendActionMessage(ActionMessage message)
        {
            if (message == null) throw new ArgumentNullException(nameof(message));

            if (_sessions.TryGetValue(message.ClientId, out var session))
            {
                session.Process(message);
            }
            else
            {
                throw new InvalidOperationException("Client is not known.");
            }
        }

        /// <inheritdoc/>
        public Task ShutdownServer()
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
