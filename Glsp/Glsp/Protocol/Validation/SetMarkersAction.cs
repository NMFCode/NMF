using NMF.Glsp.Protocol.BaseProtocol;

namespace NMF.Glsp.Protocol.Validation
{
    /// <summary>
    /// Response to the RequestMarkersAction containing all validation markers. Sent from the server to the client. 
    /// This action always sends the entire list of markers. Thus, clients can replace all markers for a specific 
    /// reason with the new ones that have been sent with the same reason.
    /// </summary>
    public class SetMarkersAction : BaseAction
    {
        /// <summary>
        /// The kind value used for this kind of action
        /// </summary>
        public const string SetMarkersActionKind = "setMarkers";

        /// <inheritdoc/>
        public override string Kind => SetMarkersActionKind;

        /// <summary>
        ///  The list of markers to be set in the diagram editor.
        /// </summary>
        public Marker[] Markers { get; init; }

        /// <summary>
        ///  The reason for this response, e.g. a `batch` validation or a `live` validation.
        /// </summary>
        public string Reason { get; init; }
    }
}
