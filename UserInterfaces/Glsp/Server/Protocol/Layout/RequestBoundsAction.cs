using NMF.Glsp.Graph;
using NMF.Glsp.Server.Contracts;
using NMF.Glsp.Server.Protocol.BaseProtocol;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NMF.Glsp.Server.Protocol.Layout
{
    /// <summary>
    /// Sent from the server to the client to request bounds for the given model. The model is rendered invisibly 
    /// so the bounds can derived from the DOM. The response is a ComputedBoundsAction. This hidden rendering 
    /// round-trip is necessary if the client is responsible for parts of the layout.
    /// </summary>
    public class RequestBoundsAction : RequestAction
    {
        /// <inheritdoc/>
        public override string Kind => "requestBounds";


        /// <summary>
         ///  The model elements to consider to compute the new bounds.
         /// </summary>
        public GGraph NewRoot { get; set; }

        /// <inheritdoc/>
        public override void Execute(IClientSession session)
        {
            throw new NotSupportedException();
        }
    }
}
