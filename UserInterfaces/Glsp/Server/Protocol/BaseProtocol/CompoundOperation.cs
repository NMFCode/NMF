using NMF.Glsp.Server.Contracts;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NMF.Glsp.Server.Protocol.BaseProtocol
{
    /// <summary>
     ///  An operation that executes a list of operations.
     /// </summary>
    public class CompoundOperation : Operation
    {
        /// <inheritdoc/>
        public override string Kind => "compound";

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
