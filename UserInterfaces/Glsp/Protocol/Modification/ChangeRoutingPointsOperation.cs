using NMF.Glsp.Graph;
using NMF.Glsp.Protocol.BaseProtocol;
using NMF.Glsp.Protocol.Types;
using NMF.Glsp.Server.Contracts;
using System.Threading.Tasks;

namespace NMF.Glsp.Protocol.Modification
{
    /// <summary>
    /// An edge may have zero or more routing points that “re-direct” the edge between the source and the target element.
    /// In order to set these routing points the client may send a ChangeRoutingPointsOperation.
    /// </summary>
    public class ChangeRoutingPointsOperation : Operation
    {
        /// <summary>
        /// The kind value used for this kind of action
        /// </summary>
        public const string ChangeRoutingPointsOperationKind = "changeRoutingPoints";

        /// <inheritdoc/>
        public override string Kind => ChangeRoutingPointsOperationKind;

        /// <summary>
        ///  The routing points of the edge (may be empty).
        /// </summary>
        public ElementAndRoutingPoints[] NewRoutingPoints { get; init; }

        /// <inheritdoc/>
        public override Task ExecuteAsync(IGlspSession session)
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
            return Task.CompletedTask;
        }
    }
}
