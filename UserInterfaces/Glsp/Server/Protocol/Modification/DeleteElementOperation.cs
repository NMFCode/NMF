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
    /// The client sends a DeleteElementOperation to the server to request the deletion of an element from the model.
    /// </summary>
    public class DeleteElementOperation : Operation
    {
        /// <inheritdoc/>
        public override string Kind => "deleteElement";

        /// <summary>
        ///  The elements to be deleted.
        /// </summary>
        public string[] ElementIds { get; init; }

        /// <inheritdoc/>
        public override void Execute(IClientSession session)
        {
            if (ElementIds != null)
            {
                foreach (var elementId in ElementIds)
                {
                    var element = session.Root.Resolve(elementId);
                    element?.Delete();
                }
            }
        }
    }
}
