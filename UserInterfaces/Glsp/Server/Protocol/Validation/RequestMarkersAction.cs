using NMF.Glsp.Server.Contracts;
using NMF.Glsp.Server.Protocol.BaseProtocol;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NMF.Glsp.Server.Protocol.Validation
{
    /// <summary>
    /// Action to retrieve markers for the specified model elements. Sent from the client to the server.
    /// </summary>
    public class RequestMarkersAction : RequestAction
    {
        /// <inheritdoc/>
        public override string Kind => "requestMarkers";

        /// <summary>
         ///  The elements for which markers are requested, may be just the root element.
         /// </summary>
        public string[] ElementsIDs { get; set; }

        /// <summary>
         ///  The reason for this request, e.g. a `batch` validation or a `live` validation.
         /// </summary>
        public string Reason { get; set; }

        /// <inheritdoc/>
        public override void Execute(IClientSession session)
        {
            throw new NotImplementedException();
        }
    }
}
