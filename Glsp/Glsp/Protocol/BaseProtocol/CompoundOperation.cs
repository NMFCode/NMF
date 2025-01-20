using NMF.Glsp.Contracts;
using System.Linq;
using System.Threading.Tasks;

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
        public BaseAction[] OperationList { get; init; }

        /// <inheritdoc/>
        public override async Task ExecuteAsync(IGlspSession session)
        {
            if (OperationList != null)
            {
                foreach (var operation in OperationList.OfType<Operation>())
                {
                    await operation.ExecuteAsync(session);
                }
            }
        }
    }
}
