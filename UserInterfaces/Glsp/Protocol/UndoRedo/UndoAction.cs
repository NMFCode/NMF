using NMF.Glsp.Contracts;
using NMF.Glsp.Protocol.BaseProtocol;
using NMF.Glsp.Protocol.ModelData;
using System;
using System.Threading.Tasks;

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
        public override Task ExecuteAsync(IGlspSession session)
        {
            if (!session.CanUndo)
            {
                throw new InvalidOperationException("Cannot undo");
            }

            session.Undo();
            session.Synchronize();
            return Task.CompletedTask;
        }
    }
}
