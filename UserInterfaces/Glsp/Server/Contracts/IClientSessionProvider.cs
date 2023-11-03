using NMF.Glsp.Protocol.Lifecycle;
using System.Collections.Generic;

namespace NMF.Glsp.Server.Contracts
{
    public interface IClientSessionProvider
    {
        string DiagramType { get; }

        ClientSession CreateSession(InitializeClientSessionParameters parameters);

        IEnumerable<string> SupportedActions { get; }
    }
}
