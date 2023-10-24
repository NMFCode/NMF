using NMF.Glsp.Graph;
using NMF.Glsp.Server.Protocol.BaseProtocol;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NMF.Glsp.Server.Protocol.Selection
{
    /// <summary>
    /// Sent from the server to the client to display a popup in response to a RequestPopupModelAction. This action 
    /// can also be used to remove any existing popup by choosing EMPTY_ROOT as root element.
    /// </summary>
    public class SetPopupModelAction : BaseAction
    {
        /// <inheritdoc/>
        public override string Kind => "setPopupModel";

        /// <summary>
        /// The model elements composing the popup to display.
        /// </summary>
        public GGraph NewRoot { get; set; }
    }
}
