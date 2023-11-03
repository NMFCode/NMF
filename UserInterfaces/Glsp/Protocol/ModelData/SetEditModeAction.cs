using NMF.Glsp.Protocol.BaseProtocol;

namespace NMF.Glsp.Protocol.ModelData
{
    /// <summary>
    /// Sent from the client to the server to set the model into a specific editor mode, allowing the 
    /// server to react to certain requests differently depending on the mode. A client may also listen 
    /// to this action to prevent certain user interactions preemptively.
    /// </summary>
    public class SetEditModeAction : BaseAction
    {
        /// <summary>
        /// The kind value used for this kind of action
        /// </summary>
        public const string SetEditModeActionKind = "setEditMode";

        /// <inheritdoc/>
        public override string Kind => SetEditModeActionKind;


        /// <summary>
        ///  The new edit mode of the diagram.
        /// </summary>
        public string EditMode { get; set; }
    }
}
