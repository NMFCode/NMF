using NMF.Glsp.Server.Contracts;
using NMF.Glsp.Server.Protocol.BaseProtocol;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NMF.Glsp.Server.Protocol.ModelData
{
    /// <summary>
    /// A RequestExportSvgAction is sent by the client (or the server) to initiate the SVG export of the current diagram. 
    /// The handler of this action is expected to retrieve the diagram SVG and should send an ExportSvgAction as response.
    /// Typically the ExportSvgAction is handled directly on client side.
    /// </summary>
    public class RequestExportSvgAction : RequestAction
    {
        /// <inheritdoc/>
        public override string Kind => throw new NotImplementedException();

        /// <inheritdoc/>
        public override void Execute(IClientSession session)
        {
            throw new NotSupportedException();
        }
    }
}
