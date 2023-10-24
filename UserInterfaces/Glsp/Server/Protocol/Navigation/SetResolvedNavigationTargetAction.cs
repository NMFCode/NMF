using NMF.Glsp.Server.Protocol.BaseProtocol;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NMF.Glsp.Server.Protocol.Navigation
{
    /// <summary>
    /// An action sent from the server in response to a ResolveNavigationTargetAction. The response contains the 
    /// resolved element ids for the given target and may contain additional information in the args property.
    /// </summary>
    public class SetResolvedNavigationTargetAction : ResponseAction
    {
        /// <inheritdoc/>
        public override string Kind => "setResolvedNavigationTarget";


        /// <summary>
         ///  The element ids of the resolved navigation target.
         /// </summary>
        public string[] ElementIds { get; set; }

        /// <summary>
         ///  Custom arguments that may be interpreted by the client.
         /// </summary>
        public IDictionary<string, string> Args { get; } = new Dictionary<string, string>();
    }
}
