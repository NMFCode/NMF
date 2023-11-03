using NMF.Glsp.Graph;
using NMF.Glsp.Protocol.BaseProtocol;
using NMF.Glsp.Protocol.Types;
using NMF.Glsp.Server.Contracts;
using System;
using System.Collections.Generic;

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
        public IDictionary<string, string> Args { get; } = new Dictionary<string, string>();

        /// <inheritdoc/>
        public override void Execute(IClientSession session)
        {
            var container = session.Root.Resolve(ContainerId);
            if (container != null)
            {
                container.Skeleton.CreateNode(container, this);
            }
            else
            {
                throw new InvalidOperationException("Container element not found.");
            }
        }
    }
}
