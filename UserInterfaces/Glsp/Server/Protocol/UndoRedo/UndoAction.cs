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
    /// Trigger an undo of the latest executed command.
    /// </summary>
    public class UndoAction : ExecutableAction
    {
        /// <inheritdoc/>
        public override string Kind => "glspUndo";

        /// <inheritdoc />
        public override void Execute(IClientSession session)
        {
            if (!session.CanUndo)
            {
                throw new InvalidOperationException("Cannot undo");
            }

            session.Undo();
        }
    }
}
