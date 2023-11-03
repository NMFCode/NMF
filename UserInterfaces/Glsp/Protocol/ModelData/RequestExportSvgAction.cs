using NMF.Glsp.Protocol.BaseProtocol;
using NMF.Glsp.Server.Contracts;
using System;

namespace NMF.Glsp.Protocol.ModelData
{
    /// <summary>
    /// A RequestExportSvgAction is sent by the client (or the server) to initiate the SVG export of the current diagram. 
    /// The handler of this action is expected to retrieve the diagram SVG and should send an ExportSvgAction as response.
    /// Typically the ExportSvgAction is handled directly on client side.
    /// </summary>
    public class RequestExportSvgAction : RequestAction
    {
        /// <summary>
        /// The kind value used for this kind of action
        /// </summary>
        public const string RequestExportSvgActionKind = "requestExportsvg";

        /// <inheritdoc/>
        public override string Kind => RequestExportSvgActionKind;

        /// <inheritdoc/>
        public override void Execute(IClientSession session)
        {
            throw new NotSupportedException();
        }
    }
}
