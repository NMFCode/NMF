using NMF.Glsp.Graph;
using NMF.Glsp.Protocol.BaseProtocol;

namespace NMF.Glsp.Protocol.Selection
{
    /// <summary>
    /// Sent from the server to the client to display a popup in response to a RequestPopupModelAction. This action 
    /// can also be used to remove any existing popup by choosing EMPTY_ROOT as root element.
    /// </summary>
    public class SetPopupModelAction : BaseAction
    {
        /// <summary>
        /// The kind value used for this kind of action
        /// </summary>
        public const string SetPopupModelActionKind = "setPopupModel";

        /// <inheritdoc/>
        public override string Kind => SetPopupModelActionKind;

        /// <summary>
        /// The model elements composing the popup to display.
        /// </summary>
        public GGraph NewRoot { get; set; }
    }
}
