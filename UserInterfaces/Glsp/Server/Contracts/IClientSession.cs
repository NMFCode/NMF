using NMF.Glsp.Graph;
using NMF.Glsp.Server.Protocol.BaseProtocol;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NMF.Glsp.Server.Contracts
{
    public interface IClientSession
    {
        GGraph Root { get; set; }

        string[] SelectedElements { get; set; }

        bool IsDirty { get; set; }

        void SendToClient(BaseAction action);

        bool CanUndo { get; }

        bool CanRedo { get; }

        void Undo();

        void Redo();
    }
}
