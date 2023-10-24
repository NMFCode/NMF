using NMF.Glsp.Graph;
using NMF.Glsp.Server.Contracts;
using NMF.Glsp.Server.Protocol.BaseProtocol;
using NMF.Glsp.Server.Protocol.Types;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NMF.Glsp.Server.Protocol.Modification
{
    /// <summary>
    /// An edge may have zero or more routing points that “re-direct” the edge between the source and the target element.
    /// In order to set these routing points the client may send a ChangeRoutingPointsOperation.
    /// </summary>
    public class ChangeRoutingPointsOperation : Operation
    {
        /// <inheritdoc/>
        public override string Kind => "changeRoutingPoints";

        /// <summary>
        ///  The routing points of the edge (may be empty).
        /// </summary>
        public ElementAndRoutingPoints[] NewRoutingPoints { get; init; }

        /// <inheritdoc/>
        public override void Execute(IClientSession session)
        {
            if (NewRoutingPoints != null)
            {
                foreach (var item in NewRoutingPoints)
                {
                    var element = session.Root.Resolve(item.ElementId) as GEdge;
                    if (element != null)
                    {
                        element.RoutingPoints.Clear();
                        if (item.NewRoutingPoints == null) continue;
                        foreach (var routingPoint in item.NewRoutingPoints)
                        {
                            element.RoutingPoints.Add(routingPoint);
                        }
                    }
                }
            }
        }
    }
}
