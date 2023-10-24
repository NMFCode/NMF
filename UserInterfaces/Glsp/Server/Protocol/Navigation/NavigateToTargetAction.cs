using NMF.Glsp.Server.Protocol.BaseProtocol;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NMF.Glsp.Server.Protocol.Navigation
{
    /// <summary>
    /// Action that triggers the navigation to a particular navigation target. This may be used by the client internally or 
    /// may be sent from the server.
    /// </summary>
    public class NavigateToTargetAction : BaseAction
    {
        /// <inheritdoc/>
        public override string Kind => "navigateToTarget";

        /// <summary>
         ///  The target to which we navigate.
         /// </summary>
        public NavigationTarget Target { get; init; }
    }
}
