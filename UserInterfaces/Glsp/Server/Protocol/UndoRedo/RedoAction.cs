using NMF.Glsp.Server.Contracts;
using NMF.Glsp.Server.Protocol.BaseProtocol;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NMF.Glsp.Server.Protocol.UndoRedo
{
    /// <summary>
    /// Trigger a redo of the latest undone command.
    /// </summary>
    public class RedoAction : ExecutableAction
    {
        /// <inheritdoc/>
        public override string Kind => "glspRedo";

        /// <inheritdoc/>
        public override void Execute(IClientSession session)
        {
            if (!session.CanRedo)
            {
                throw new InvalidOperationException("Cannot redo");
            }

            session.Redo();
        }
    }
}
