using NMF.Glsp.Protocol.BaseProtocol;
using System;

namespace NMF.Glsp.Server.Contracts
{
    public interface IActionHandler
    {
        bool CanHandle(Type actionType);

        void Handle(BaseAction action);
    }
}
