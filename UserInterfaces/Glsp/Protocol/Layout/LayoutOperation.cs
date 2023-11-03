using NMF.Glsp.Protocol.BaseProtocol;
using NMF.Glsp.Server.Contracts;
using System;

namespace NMF.Glsp.Protocol.Layout
{
    /// <summary>
    /// Request a layout of the diagram or selected elements from the server.
    /// </summary>
    public class LayoutOperation : Operation
    {
        /// <summary>
        /// The kind value used for this kind of action
        /// </summary>
        public const string LayoutOperationKind = "layout";

        /// <inheritdoc/>
        public override string Kind => LayoutOperationKind;

        /// <summary>
        ///  The identifiers of the elements that should be layouted, will default to the root element if not defined.
        /// </summary>
        public string[] ElementIds { get; set; }

        /// <inheritdoc/>
        public override void Execute(IClientSession session)
        {
            throw new NotImplementedException();
        }
    }
}
