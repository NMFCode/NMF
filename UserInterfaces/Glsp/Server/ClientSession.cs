using NMF.Glsp.Server.Protocol.BaseProtocol;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NMF.Glsp.Server
{
    public class ClientSession
    {
        internal Task DisposeAsync()
        {
            throw new NotImplementedException();
        }

        internal Task InitializeAsync()
        {
            throw new NotImplementedException();
        }

        internal void Process(ActionMessage message)
        {
            throw new NotImplementedException();
        }
    }
}
