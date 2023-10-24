using NMF.Glsp.Server.Protocol.BaseProtocol;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NMF.Glsp.Server.Protocol.ModelData
{
    /// <summary>
    /// Sent from the client to the server to set the model into a specific editor mode, allowing the 
    /// server to react to certain requests differently depending on the mode. A client may also listen 
    /// to this action to prevent certain user interactions preemptively.
    /// </summary>
    public class SetEditModeAction : BaseAction
    {
        /// <inheritdoc/>
        public override string Kind => "setEditMode";


        /// <summary>
         ///  The new edit mode of the diagram.
         /// </summary>
        public string EditMode { get; set; }
    }
}
