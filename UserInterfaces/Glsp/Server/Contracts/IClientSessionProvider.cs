using NMF.Glsp.Server.Protocol.Lifecycle;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NMF.Glsp.Server.Contracts
{
    public interface IClientSessionProvider
    {
        string DiagramType { get; }

        ClientSession CreateSession(InitializeClientSessionParameters parameters);

        IEnumerable<string> SupportedActions { get; }
    }
}
