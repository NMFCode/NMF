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
    /// The client sends a ChangeContainerOperation to the server to request the execution of a changeContainer operation.
    /// </summary>
    public class ChangeContainerOperation : Operation
    {
        /// <inheritdoc/>
        public override string Kind => "changeContainer";

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
        public override void Execute(IClientSession session)
        {
            var element = session.Root.Resolve(ElementId);
            var container = session.Root.Resolve(TargetContainerId);

            if (element == null || container == null)
            {
                throw new InvalidOperationException("Could not find the element or the container");
            }

            element.Parent = container;
        }
    }
}
