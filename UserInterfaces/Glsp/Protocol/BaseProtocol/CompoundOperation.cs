using NMF.Glsp.Server.Contracts;
using System.Collections.Generic;

namespace NMF.Glsp.Protocol.BaseProtocol
{
    /// <summary>
    ///  An operation that executes a list of operations.
    /// </summary>
    public class CompoundOperation : Operation
    {
        /// <summary>
        /// The kind value used for this kind of action
        /// </summary>
        public const string CompoundOperationKind = "compound";

        /// <inheritdoc/>
        public override string Kind => CompoundOperationKind;

        /// <summary>
        ///  List of operations that should be executed.
        /// </summary>
        public IList<Operation> OperationList { get; } = new List<Operation>();

        /// <inheritdoc/>
        public override void Execute(IClientSession session)
        {
            foreach (var operation in OperationList)
            {
                operation.Execute(session);
            }
        }
    }
}
