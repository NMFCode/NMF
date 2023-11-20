using NMF.Glsp.Graph;
using NMF.Glsp.Protocol.BaseProtocol;
using NMF.Glsp.Server.Contracts;
using System;

namespace NMF.Glsp.Protocol.Layout
{
    /// <summary>
    /// Sent from the server to the client to request bounds for the given model. The model is rendered invisibly 
    /// so the bounds can derived from the DOM. The response is a ComputedBoundsAction. This hidden rendering 
    /// round-trip is necessary if the client is responsible for parts of the layout.
    /// </summary>
    public class RequestBoundsAction : RequestAction
    {
        /// <summary>
        /// The kind value used for this kind of action
        /// </summary>
        public const string RequestBoundsActionKind = "requestBounds";

        /// <inheritdoc/>
        public override string Kind => RequestBoundsActionKind;


        /// <summary>
        ///  The model elements to consider to compute the new bounds.
        /// </summary>
        public GGraph NewRoot { get; set; }

        /// <inheritdoc/>
        public override void Execute(IGlspSession session)
        {
            throw new NotSupportedException();
        }
    }
}
