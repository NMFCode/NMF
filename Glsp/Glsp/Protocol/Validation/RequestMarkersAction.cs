using NMF.Glsp.Contracts;
using NMF.Glsp.Protocol.BaseProtocol;
using System.Threading.Tasks;

namespace NMF.Glsp.Protocol.Validation
{
    /// <summary>
    /// Action to retrieve markers for the specified model elements. Sent from the client to the server.
    /// </summary>
    public class RequestMarkersAction : RequestAction
    {
        /// <summary>
        /// The kind value used for this kind of action
        /// </summary>
        public const string RequestMarkersActionKind = "requestMarkers";

        /// <inheritdoc/>
        public override string Kind => RequestMarkersActionKind;

        /// <summary>
        ///  The elements for which markers are requested, may be just the root element.
        /// </summary>
        public string[] ElementsIDs { get; set; }

        /// <summary>
        ///  The reason for this request, e.g. a `batch` validation or a `live` validation.
        /// </summary>
        public string Reason { get; set; }

        /// <inheritdoc/>
        public override Task ExecuteAsync(IGlspSession session)
        {
            session.SendToClient(new SetMarkersAction
            {
                Reason = "live",
                Markers = new Marker[0],
            });
            return Task.CompletedTask;
        }
    }
}
