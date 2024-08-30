using NMF.Glsp.Protocol.BaseProtocol;
using NMF.Glsp.Server.Contracts;
using System;
using System.Threading.Tasks;

namespace NMF.Glsp.Protocol.Modification
{
    /// <summary>
    /// The client sends a ChangeContainerOperation to the server to request the execution of a changeContainer operation.
    /// </summary>
    public class ChangeContainerOperation : Operation
    {
        /// <summary>
        /// The kind value used for this kind of action
        /// </summary>
        public const string ChangeContainerOperationKind = "changeContainer";

        /// <inheritdoc/>
        public override string Kind => ChangeContainerOperationKind;

        /// <summary>
        ///  The element to be changed.
        /// </summary>
        public string ElementId { get; init; }

        /// <summary>
        ///  The element container of the changeContainer operation.
        /// </summary>
        public string TargetContainerId { get; init; }

        /// <summary>
        ///  The graphical location.
        /// </summary>
        public string Location { get; init; }

        /// <inheritdoc/>
        public override Task ExecuteAsync(IGlspSession session)
        {
            var element = session.Root.Resolve(ElementId);
            var container = session.Root.Resolve(TargetContainerId);

            if (element == null || container == null)
            {
                throw new InvalidOperationException("Could not find the element or the container");
            }

            var targetSkeleton = container.Skeleton;
            throw new NotImplementedException();
        }
    }
}
