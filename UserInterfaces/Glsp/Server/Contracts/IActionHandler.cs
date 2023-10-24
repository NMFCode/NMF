using NMF.Glsp.Server.Protocol.BaseProtocol;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NMF.Glsp.Server.Contracts
{
    public interface IActionHandler
    {
        bool CanHandle(Type actionType);

        void Handle(BaseAction action);
    }
}
