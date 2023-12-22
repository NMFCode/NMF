using NMF.Glsp.Protocol.BaseProtocol;
using System;
using System.Threading.Tasks;

namespace NMF.Glsp.Server
{
    public interface IGlspClientSession
    {
        Task InitializeAsync(Action<ActionMessage> messageHandler, string clientId);

        Task ProcessAsync(BaseAction action);

        Task DisposeAsync();
    }
}