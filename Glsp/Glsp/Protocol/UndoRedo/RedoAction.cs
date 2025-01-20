using NMF.Glsp.Contracts;
using NMF.Glsp.Protocol.BaseProtocol;
using System;
using System.Threading.Tasks;

namespace NMF.Glsp.Protocol.UndoRedo
{
    /// <summary>
    /// Trigger a redo of the latest undone command.
    /// </summary>
    public class RedoAction : ExecutableAction
    {
        /// <summary>
        /// The kind value used for this kind of action
        /// </summary>
        public const string RedoActionKind = "glspRedo";

        /// <inheritdoc/>
        public override string Kind => RedoActionKind;

        /// <inheritdoc/>
        public override Task ExecuteAsync(IGlspSession session)
        {
            if (!session.CanRedo)
            {
                throw new InvalidOperationException("Cannot redo");
            }

            session.Redo();
            session.Synchronize();
            return Task.CompletedTask;
        }
    }
}
