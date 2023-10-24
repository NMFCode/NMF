using NMF.Glsp.Server.Protocol.BaseProtocol;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NMF.Glsp.Server.Protocol.Navigation
{
    /// <summary>
    /// Response action from the server following a RequestNavigationTargetsAction. It contains all available navigation targets 
    /// for the queried target type in the provided editor context. The server may also provide additional information using the 
    /// arguments, e.g., warnings, that can be interpreted by the client.
    /// </summary>
    public class SetNavigationTargetsAction : ResponseAction
    {
        /// <inheritdoc/>
        public override string Kind => "setNavigationTargets";


        /// <summary>
         ///  A list of navigation targets.
         /// </summary>
        public NavigationTarget[] Targets { get; init; }

        /// <summary>
         ///  Custom arguments that may be interpreted by the client.
         /// </summary>
        public IDictionary<string, string> Args { get; init; }
    }
}
