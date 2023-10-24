using NMF.Glsp.Graph;
using NMF.Glsp.Server.Contracts;
using NMF.Glsp.Server.Protocol.BaseProtocol;
using NMF.Glsp.Server.Protocol.Types;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NMF.Glsp.Server.Protocol.Modification
{
    /// <summary>
    /// In order to create a node in the model the client can send a CreateNodeOperation with the necessary information to create that node.
    /// </summary>
    public class CreateNodeOperation : Operation
    {
        /// <inheritdoc/>
        public override string Kind => "createNode";

        /// <summary> 
        ///  The location at which the operation shall be executed.
        /// </summary>
        public Point Location { get; init; }

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
            if (container == null)
            {
                var node = new GElement
                {
                    Type = ElementTypeId,
                    Position = Location
                };
                foreach (var config in Args)
                {
                    node.Details.Add(config.Key, config.Value);
                }
                container.Children.Add(node);
            }
        }
    }
}
