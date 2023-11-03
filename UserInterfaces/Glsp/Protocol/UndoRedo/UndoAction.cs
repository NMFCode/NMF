using NMF.Glsp.Protocol.BaseProtocol;
using NMF.Glsp.Server.Contracts;
using System;

namespace NMF.Glsp.Protocol.UndoRedo
{
    /// <summary>
    /// Trigger an undo of the latest executed command.
    /// </summary>
    public class UndoAction : ExecutableAction
    {
        /// <summary>
        /// The kind value used for this kind of action
        /// </summary>
        public const string UndoActionKind = "glspUndo";

        /// <inheritdoc/>
        public override string Kind => UndoActionKind;

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
