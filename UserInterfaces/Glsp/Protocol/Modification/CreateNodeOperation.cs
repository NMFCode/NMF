using NMF.Glsp.Protocol.BaseProtocol;
using NMF.Glsp.Protocol.Layout;
using NMF.Glsp.Protocol.Types;
using NMF.Glsp.Server.Contracts;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace NMF.Glsp.Protocol.Modification
{
    /// <summary>
    /// In order to create a node in the model the client can send a CreateNodeOperation with the necessary information to create that node.
    /// </summary>
    public class CreateNodeOperation : Operation
    {
        /// <summary>
        /// The kind value used for this kind of action
        /// </summary>
        public const string CreateNodeOperationKind = "createNode";

        /// <inheritdoc/>
        public override string Kind => CreateNodeOperationKind;

        /// <summary> 
        ///  The location at which the operation shall be executed.
        /// </summary>
        public Point? Location { get; init; }

        /// <summary>
        ///  The container in which the operation shall be executed.
        /// </summary>
        public string ContainerId { get; init; }


        /// <summary>
        ///  The type of edge that should be created by the edge creation tool.
        /// </summary>
        public string ElementTypeId { get; init; }


        /// <summary>
        ///  Custom arguments.
        /// </summary>
        public IDictionary<string, object> Args { get; init; }

        /// <inheritdoc/>
        public override async Task Execute(IGlspSession session)
        {
            var container = ContainerId == null ? session.Root : session.Root.Resolve(ContainerId);
            if (container != null)
            {
                var node = container.Skeleton.CreateNode(container, this);
                if (node != null)
                {
                    var layoutUpdate = await session.RequestAsync(new RequestBoundsAction
                    {
                        NewRoot = session.Root
                    });
                    if (layoutUpdate is ComputedBoundsAction updatedBounds)
                    {
                        updatedBounds.UpdateBounds(session);
                    }
                }
            }
            else
            {
                throw new InvalidOperationException("Container element not found.");
            }
        }
    }
}
