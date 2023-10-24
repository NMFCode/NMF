using NMF.Glsp.Graph;
using NMF.Glsp.Server.Contracts;
using NMF.Glsp.Server.Protocol.BaseProtocol;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NMF.Glsp.Server.Protocol.Modification
{
    /// <summary>
    /// In order to create an edge in the model the client can send a CreateEdgeOperation with the necessary information to create that edge.
    /// </summary>
    public class CreateEdgeOperation : Operation
    {
        /// <inheritdoc/>
        public override string Kind => "createEdge";

        /// <summary>
        ///  The source element.
        /// </summary>
        public string SourceElementId { get; init; }

        /// <summary>
        ///  The target element.
        /// </summary>
        public string TargetElementId { get; init; }


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
            var sourceElement = session.Root.Resolve(SourceElementId);
            var targetElement = session.Root.Resolve(TargetElementId);

            if (sourceElement != null && targetElement != null)
            {
                var transition = new GEdge
                {
                    SourceId = SourceElementId,
                    TargetId = TargetElementId,
                    Type = ElementTypeId,
                };
                foreach ( var config in Args)
                {
                    transition.Details.Add(config.Key, config.Value);
                }
                sourceElement.Parent.Children.Add(transition);
            }
            else
            {
                throw new InvalidOperationException("Source or target element not found.");
            }
        }
    }
}
