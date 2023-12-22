using NMF.Glsp.Protocol.Lifecycle;
using System.Collections.Generic;

namespace NMF.Glsp.Server.Contracts
{
    public interface IClientSessionProvider
    {
        string DiagramType { get; }

        IGlspClientSession CreateSession(IDictionary<string, object> args);

        IEnumerable<string> SupportedActions { get; }
    }
}
