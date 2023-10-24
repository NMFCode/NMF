using NMF.Glsp.Server.Protocol.BaseProtocol;
using NMF.Glsp.Server.Protocol.Types;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NMF.Glsp.Server.Protocol.Selection
{
    /// <summary>
    /// Triggered when the user hovers the mouse pointer over an element to get a popup with details on that element. 
    /// This action is sent from the client to the server. The response is a SetPopupModelAction.
    /// </summary>
    public class RequestPopupModelAction : BaseAction
    {
        /// <inheritdoc/>
        public override string Kind => "requestPopupModel";

        /// <summary>
        /// The identifier of the elements for which a popup is requested.
        /// </summary>
        public string ElementId { get; set; }

        /// <summary>
        /// The bounds.
        /// </summary>
        public Bounds Bounds { get; set; }
    }
}
