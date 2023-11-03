using NMF.Glsp.Protocol.BaseProtocol;
using NMF.Glsp.Protocol.Types;

namespace NMF.Glsp.Protocol.Selection
{
    /// <summary>
    /// Triggered when the user hovers the mouse pointer over an element to get a popup with details on that element. 
    /// This action is sent from the client to the server. The response is a SetPopupModelAction.
    /// </summary>
    public class RequestPopupModelAction : BaseAction
    {
        /// <summary>
        /// The kind value used for this kind of action
        /// </summary>
        public const string RequestPopupModelActionKind = "requestPopupModel";

        /// <inheritdoc/>
        public override string Kind => RequestPopupModelActionKind;

        /// <summary>
        /// The identifier of the elements for which a popup is requested.
        /// </summary>
        public string ElementId { get; set; }

        /// <summary>
        /// The bounds.
        /// </summary>
        public Bounds Bounds { get; set; }
    }
}
