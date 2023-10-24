using NMF.Glsp.Graph;
using NMF.Glsp.Server.Protocol.BaseProtocol;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NMF.Glsp.Server.Protocol.ModelData
{
    /// <summary>
    /// ent from the server to the client in order to set the model. If a model is already present, it is replaced.
    /// </summary>
    public class SetModelAction : ResponseAction
    {
        /// <inheritdoc/>
        public override string Kind => "setModel";

        /// <summary>
         ///  The new graphical model elements.
         /// </summary>
        public GGraph NewRoot { get; set; }
    }
}
