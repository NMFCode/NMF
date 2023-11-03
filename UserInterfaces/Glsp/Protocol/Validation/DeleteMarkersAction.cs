using NMF.Glsp.Protocol.BaseProtocol;

namespace NMF.Glsp.Protocol.Validation
{
    /// <summary>
    /// To remove markers for elements a client or server may send a DeleteMarkersAction with all markers that should be removed.
    /// </summary>
    public class DeleteMarkersAction : BaseAction
    {
        /// <summary>
        /// The kind value used for this kind of action
        /// </summary>
        public const string DeleteMarkersActionKind = "deleteMarkers";

        /// <inheritdoc/>
        public override string Kind => DeleteMarkersActionKind;


        /// <summary>
        ///  The list of markers that should be deleted.
        /// </summary>
        public Marker[] Markers { get; init; }
    }
}
