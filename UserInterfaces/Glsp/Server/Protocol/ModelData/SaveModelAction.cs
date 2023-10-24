using NMF.Glsp.Server.Protocol.BaseProtocol;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NMF.Glsp.Server.Protocol.ModelData
{
    /// <summary>
    /// Sent from the client to the server in order to persist the current model state back to the 
    /// source model. A new fileUri can be defined to save the model to a new destination different 
    /// from its original source model.
    /// </summary>
    public class SaveModelAction : BaseAction
    {
        /// <inheritdoc/>
        public override string Kind => "saveModel";

        /// <summary>
         ///   The optional destination file uri.
         /// </summary>
        public string FileUri { get; set; }
    }
}
