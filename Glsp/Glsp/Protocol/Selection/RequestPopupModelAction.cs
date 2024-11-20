using NMF.Glsp.Contracts;
using NMF.Glsp.Protocol.BaseProtocol;
using NMF.Glsp.Protocol.Types;
using System.Threading.Tasks;

namespace NMF.Glsp.Protocol.Selection
{
    /// <summary>
    /// Triggered when the user hovers the mouse pointer over an element to get a popup with details on that element. 
    /// This action is sent from the client to the server. The response is a SetPopupModelAction.
    /// </summary>
    public class RequestPopupModelAction : RequestAction
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

        /// <inheritdoc />
        public override Task ExecuteAsync(IGlspSession session)
        {
            var element = session.Root.Resolve(ElementId);
            if (element != null)
            {
                var popup = element.Skeleton.CreatePopup(this, element);
                if (popup != null)
                {
                    session.SendToClient(new SetPopupModelAction
                    {
                        ResponseId = RequestId,
                        NewRoot = popup
                    });
                }
            }
            return Task.CompletedTask;
        }
    }
}
