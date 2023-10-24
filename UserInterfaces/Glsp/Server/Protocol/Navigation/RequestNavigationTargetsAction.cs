using NMF.Glsp.Server.Contracts;
using NMF.Glsp.Server.Protocol.BaseProtocol;
using NMF.Glsp.Server.Protocol.Types;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NMF.Glsp.Server.Protocol.Navigation
{
    /// <summary>
    /// Action that is usually sent from the client to the server to request navigation targets for a specific navigation 
    /// type such as documentation or implementation in the given editor context.
    /// </summary>
    public class RequestNavigationTargetsAction : RequestAction
    {
        /// <inheritdoc/>
        public override string Kind => "requestNavigationTargets";


        /// <summary>
        ///  Identifier of the type of navigation targets we want to retrieve, e.g., 'documentation', 'implementation', etc.
        /// </summary>
        public string TargetTypeId { get; init; }

        /// <summary>
        ///  The current editor context.
        /// </summary>
        public EditorContext EditorContext { get; init; }

        /// <inheritdoc/>
        public override void Execute(IClientSession session)
        {
            throw new NotImplementedException();
        }
    }
}
