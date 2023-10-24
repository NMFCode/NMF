using NMF.Glsp.Server.Protocol.BaseProtocol;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NMF.Glsp.Server.Protocol.ModelData
{
    /// <summary>
    /// The server sends a SetDirtyStateAction to indicate to the client that the current model state on the server 
    /// does not correspond to the persisted model state of the source model. A client may ignore such an action or 
    /// use it to indicate to the user the dirty state.
    /// </summary>
    public class SetDirtyAction : BaseAction
    {
        /// <inheritdoc/>
        public override string Kind => "setDirtyState";


        /// <summary>
         ///  True if the current model state is dirty
         /// </summary>
        public bool IsDirty { get; set; }

        /// <summary>
         ///  A string indicating the reason for the dirty state change e.g 'operation', 'undo' ...
         /// </summary>
        public string Reason { get; set; }
    }
}
