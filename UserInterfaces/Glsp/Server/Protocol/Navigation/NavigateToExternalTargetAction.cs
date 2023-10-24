using NMF.Glsp.Server.Protocol.BaseProtocol;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NMF.Glsp.Server.Protocol.Navigation
{
    /// <summary>
    /// If a navigation target cannot be resolved or the resolved target is something that is not part of our source 
    /// model, e.g., a separate documentation file, a NavigateToExternalTargetAction may be sent. Since the target 
    /// it outside of the model scope such an action would be typically handled by an integration layer (such as 
    /// the surrounding IDE).
    /// </summary>
    public class NavigateToExternalTargetAction : BaseAction
    {
        /// <inheritdoc/>
        public override string Kind => "navigateToExternalTarget";

        /// <summary>
         ///  The target to which we navigate.
         /// </summary>
        public NavigationTarget NavigationTarget { get; init; }
    }
}
