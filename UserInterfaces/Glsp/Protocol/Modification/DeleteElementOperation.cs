using NMF.Glsp.Contracts;
using NMF.Glsp.Protocol.BaseProtocol;
using System.Threading.Tasks;

namespace NMF.Glsp.Protocol.Modification
{
    /// <summary>
    /// The client sends a DeleteElementOperation to the server to request the deletion of an element from the model.
    /// </summary>
    public class DeleteElementOperation : Operation
    {
        /// <summary>
        /// The kind value used for this kind of action
        /// </summary>
        public const string DeleteElementOperationKind = "deleteElement";

        /// <inheritdoc/>
        public override string Kind => DeleteElementOperationKind;

        /// <summary>
        ///  The elements to be deleted.
        /// </summary>
        public string[] ElementIds { get; init; }

        /// <inheritdoc/>
        public override Task ExecuteAsync(IGlspSession session)
        {
            if (ElementIds != null)
            {
                foreach (var elementId in ElementIds)
                {
                    var element = session.Root.Resolve(elementId);
                    element?.Delete();
                }
            }
            return Task.CompletedTask;
        }
    }
}
